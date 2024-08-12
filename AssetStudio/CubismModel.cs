using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static AssetStudio.EndianSpanReader;

namespace AssetStudio
{
    public enum CubismSDKVersion : byte
    {
        V30 = 1,
        V33,
        V40,
        V42,
        V50
    }

    public sealed class CubismModel : IDisposable
    {
        public CubismSDKVersion Version { get; }
        public string VersionDescription { get; }
        public float CanvasWidth { get; }
        public float CanvasHeight { get; }
        public float CentralPosX { get; }
        public float CentralPosY { get; }
        public float PixelPerUnit { get; }
        public uint PartCount { get; }
        public uint ParamCount { get; }
        public HashSet<string> PartNames { get; }
        public HashSet<string> ParamNames { get; }
        
        private byte[] modelData;
        private int modelDataSize;
        private bool isBigEndian;

        public CubismModel(MonoBehaviour moc)
        {
            var reader = moc.reader;
            reader.Reset();
            reader.Position += 28; //PPtr<GameObject> m_GameObject, m_Enabled, PPtr<MonoScript>
            reader.ReadAlignedString(); //m_Name
            modelDataSize = (int)reader.ReadUInt32();
            modelData = BigArrayPool<byte>.Shared.Rent(modelDataSize);
            _ = reader.Read(modelData, 0, modelDataSize);

            var sdkVer = modelData[4];
            if (Enum.IsDefined(typeof(CubismSDKVersion), sdkVer))
            {
                Version = (CubismSDKVersion)sdkVer;
                VersionDescription = ParseVersion();
            }
            else
            {
                var msg = $"Unknown SDK version ({sdkVer})";
                VersionDescription = msg;
                Version = 0;
                Logger.Warning($"Live2D model \"{moc.m_Name}\": " + msg);
                return;
            }
            isBigEndian = BitConverter.ToBoolean(modelData, 5);

            //offsets
            var countInfoTableOffset = (int)SpanToUint32(modelData, 64, isBigEndian);
            var canvasInfoOffset = (int)SpanToUint32(modelData, 68, isBigEndian);
            var partIdsOffset = SpanToUint32(modelData, 76, isBigEndian);
            var parameterIdsOffset = SpanToUint32(modelData, 264, isBigEndian);

            //canvas
            PixelPerUnit = ToSingle(modelData, canvasInfoOffset, isBigEndian);
            CentralPosX = ToSingle(modelData, canvasInfoOffset + 4, isBigEndian);
            CentralPosY = ToSingle(modelData, canvasInfoOffset + 8, isBigEndian);
            CanvasWidth = ToSingle(modelData, canvasInfoOffset + 12, isBigEndian);
            CanvasHeight = ToSingle(modelData, canvasInfoOffset + 16, isBigEndian);

            //model
            PartCount = SpanToUint32(modelData, countInfoTableOffset, isBigEndian);
            ParamCount = SpanToUint32(modelData, countInfoTableOffset + 20, isBigEndian);
            PartNames = ReadMocStringHashSet(modelData, (int)partIdsOffset, (int)PartCount);
            ParamNames = ReadMocStringHashSet(modelData, (int)parameterIdsOffset, (int)ParamCount);
        }

        public void SaveMoc3(string savePath)
        {
            if (!savePath.EndsWith(".moc3"))
                savePath += ".moc3";

            using (var file = File.OpenWrite(savePath))
            {
                file.Write(modelData, 0, modelDataSize);
            }
        }

        private string ParseVersion()
        {
            switch (Version)
            {
                case CubismSDKVersion.V30:
                    return "SDK3.0/Cubism3.0(3.2)";
                case CubismSDKVersion.V33:
                    return "SDK3.3/Cubism3.3";
                case CubismSDKVersion.V40:
                    return "SDK4.0/Cubism4.0";
                case CubismSDKVersion.V42:
                    return "SDK4.2/Cubism4.2";
                case CubismSDKVersion.V50:
                    return "SDK5.0/Cubism5.0";
                default:
                    return "";
            }
        }

        private static float ToSingle(ReadOnlySpan<byte> data, int index, bool isBigEndian)  //net framework ver
        {
            var bytes = data.Slice(index, index + 4).ToArray();
            if ((isBigEndian && BitConverter.IsLittleEndian) || (!isBigEndian && !BitConverter.IsLittleEndian))
                (bytes[0], bytes[1], bytes[2], bytes[3]) = (bytes[3], bytes[2], bytes[1], bytes[0]);

            return BitConverter.ToSingle(bytes, 0);
        }

        private static HashSet<string> ReadMocStringHashSet(ReadOnlySpan<byte> data, int index, int count)
        {
            const int strLen = 64;
            var strHashSet = new HashSet<string>();
            for (var i = 0; i < count; i++)
            {
                if (index + i * strLen <= data.Length)
                {
                    var buff = data.Slice(index + i * strLen, strLen);
                    strHashSet.Add(Encoding.UTF8.GetString(buff.ToArray()).TrimEnd('\0'));
                }
            }
            return strHashSet;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                BigArrayPool<byte>.Shared.Return(modelData, clearArray: true);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
