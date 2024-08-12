using System;
using System.IO;
using static AssetStudio.EndianSpanReader;

namespace AssetStudio
{
    public class FileReader : EndianBinaryReader
    {
        public string FullPath;
        public string FileName;
        public FileType FileType;

        private static readonly byte[] gzipMagic = { 0x1f, 0x8b };
        private static readonly byte[] brotliMagic = { 0x62, 0x72, 0x6F, 0x74, 0x6C, 0x69 };
        private static readonly byte[] zipMagic = { 0x50, 0x4B, 0x03, 0x04 };
        private static readonly byte[] zipSpannedMagic = { 0x50, 0x4B, 0x07, 0x08 };

        public FileReader(string path) : this(path, File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) { }

        public FileReader(string path, Stream stream) : base(stream, EndianType.BigEndian)
        {
            FullPath = Path.GetFullPath(path);
            FileName = Path.GetFileName(path);
            FileType = CheckFileType();
        }

        private FileType CheckFileType()
        {
            var signature = this.ReadStringToNull(20);
            Position = 0;
            switch (signature)
            {
                case "UnityWeb":
                case "UnityRaw":
                case "UnityArchive":
                case "UnityFS":
                    return FileType.BundleFile;
                case "UnityWebData1.0":
                    return FileType.WebFile;
                default:
                {
                    var buff = ReadBytes(40).AsSpan();
                    var magic = Span<byte>.Empty;
                    Position = 0;

                    magic = buff.Length > 2 ? buff.Slice(0, 2) : magic;
                    if (magic.SequenceEqual(gzipMagic))
                    {
                        return FileType.GZipFile;
                    }

                    magic = buff.Length > 38 ? buff.Slice(32, 6) : magic;
                    if (magic.SequenceEqual(brotliMagic))
                    {
                        return FileType.BrotliFile;
                    }

                    if (IsSerializedFile(buff))
                    {
                        return FileType.AssetsFile;
                    }

                    magic = buff.Length > 4 ? buff.Slice(0, 4): magic;
                    if (magic.SequenceEqual(zipMagic) || magic.SequenceEqual(zipSpannedMagic))
                    {
                        return FileType.ZipFile;
                    }

                    return FileType.ResourceFile;
                }
            }
        }

        private bool IsSerializedFile(Span<byte> buff)
        {
            var fileSize = BaseStream.Length;
            if (fileSize < 20)
            {
                return false;
            }
            var isBigEndian = Endian == EndianType.BigEndian;

            //var m_MetadataSize = SpanToUint32(buff, 0, isBigEndian);
            long m_FileSize = SpanToUint32(buff, 4, isBigEndian);
            var m_Version = SpanToUint32(buff, 8, isBigEndian);
            long m_DataOffset = SpanToUint32(buff, 12, isBigEndian);
            //var m_Endianess = buff[16];
            //var m_Reserved = buff.Slice(17, 3);
            if (m_Version >= 22)
            {
                if (fileSize < 48)
                {
                    return false;
                }
                //m_MetadataSize = SpanToUint32(buff, 20, isBigEndian);
                m_FileSize = SpanToInt64(buff, 24, isBigEndian);
                m_DataOffset = SpanToInt64(buff, 32, isBigEndian);
            }
            if (m_FileSize != fileSize || m_DataOffset > fileSize)
            {
                return false;
            }
            
            return true;
        }
    }
}
