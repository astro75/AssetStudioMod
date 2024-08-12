// Shortcut (.lnk) file reader
// by aelurum
// Based on https://github.com/libyal/liblnk/blob/main/documentation/Windows%20Shortcut%20File%20(LNK)%20format.asciidoc

using AssetStudio;
using System;
using System.IO;
using System.Text;

namespace AssetStudioGUI
{
    public static class LnkReader
    {
        [Flags]
        private enum LnkDataFlags
        {
            //The LNK file contains a link target identifier
            HasTargetIDList = 0x00000001,
            //The LNK file contains location information
            HasLinkInfo = 0x00000002,
        }

        [Flags]
        private enum LnkLocFlags
        {
            //The linked file is on a volume
            //If set the volume information and the local path contain data
            VolumeIDAndLocalBasePath = 0x0001,
            //The linked file is on a network share
            //If set the network share information and common path contain data
            CommonNetworkRelativeLinkAndPathSuffix = 0x0002
        }

        [Flags]
        private enum PathTypeFlags
        {
            IsUnicodeLocalPath = 0x01,
            IsUnicodeNetShareName = 0x02,
            IsUnicodeCommonPath = 0x04
        }

        public static string GetLnkTarget(string filePath)
        {
            var targetPath = string.Empty;
            var pathType = (PathTypeFlags)0;
            Encoding sysEncoding;
            try
            {
                sysEncoding = GetSysEncoding();
                Logger.Debug($"System default text encoding: {sysEncoding.CodePage}");
            }
            catch (Exception ex)
            {
                Logger.Error("Text encoding error", ex);
                return null;
            }

            using (var reader = new FileReader(filePath))
            {
                reader.Endian = EndianType.LittleEndian;

                var headerSize = reader.ReadUInt32(); //76 bytes
                reader.Position = 20; //skip LNK class identifier (GUID)
                var dataFlags = (LnkDataFlags)reader.ReadUInt32();
                if ((dataFlags & LnkDataFlags.HasLinkInfo) == 0)
                {
                    Logger.Warning("Unsupported type of .lnk file. Link info was not found.");
                    return null;
                }
                reader.Position = headerSize;

                //Skip the shell item ID list
                if ((dataFlags & LnkDataFlags.HasTargetIDList) != 0)
                {
                    var itemIDListSize = reader.ReadUInt16();
                    reader.Position += itemIDListSize;
                }

                //The offsets is relative to the start of the location information block
                var locInfoPos = reader.Position;
                var locInfoFullSize = reader.ReadUInt32();
                if (locInfoFullSize == 0)
                {
                    Logger.Warning("Unsupported type of .lnk file. Link info was not found.");
                    return null;
                }
                var locInfoHeaderSize = reader.ReadUInt32();
                var locFlags = (LnkLocFlags)reader.ReadUInt32();
                //Offset to the volume information block
                var offsetVolumeInfo = reader.ReadUInt32();
                //Offset to the ANSI local path
                var offsetLocalPath = reader.ReadUInt32();
                //Offset to the network share information block
                var offsetNetInfo = reader.ReadUInt32();
                //Offset to the ANSI common path. 0 if not available
                var offsetCommonPath = reader.ReadUInt32();
                if (locInfoHeaderSize > 28)
                {
                    //Offset to the Unicode local path
                    offsetLocalPath = reader.ReadUInt32();
                    pathType |= PathTypeFlags.IsUnicodeLocalPath;
                }
                if (locInfoHeaderSize > 32)
                {
                    //Offset to the Unicode common path
                    offsetCommonPath = reader.ReadUInt32();
                    pathType |= PathTypeFlags.IsUnicodeCommonPath;
                }

                //Read local path, if exist                
                if (offsetLocalPath > 0)
                {
                    reader.Position = locInfoPos + offsetLocalPath;
                    targetPath = (pathType & PathTypeFlags.IsUnicodeLocalPath) != 0
                        ? reader.ReadStringToNull(encoding: Encoding.Unicode)
                        : reader.ReadStringToNull(encoding: sysEncoding);
                }

                //Read network path, if exist
                if (locFlags == LnkLocFlags.CommonNetworkRelativeLinkAndPathSuffix)
                {
                    reader.Position = locInfoPos + offsetNetInfo;
                    var netInfoSize = reader.ReadUInt32();
                    var netInfoFlags = reader.ReadUInt32();
                    //Offset to the ANSI network share name. The offset is relative to the start of the network share information block
                    var offsetNetShareName = reader.ReadUInt32();
                    if (offsetNetShareName > 20)
                    {
                        reader.Position = locInfoPos + offsetNetInfo + 20;
                        //Offset to the Unicode network share name
                        offsetNetShareName = reader.ReadUInt32();
                        pathType |= PathTypeFlags.IsUnicodeNetShareName;
                    }
                    if (offsetNetShareName > 0)
                    {
                        reader.Position = locInfoPos + offsetNetInfo + offsetNetShareName;
                        targetPath = (pathType & PathTypeFlags.IsUnicodeNetShareName) != 0
                            ? reader.ReadStringToNull(encoding: Encoding.Unicode)
                            : reader.ReadStringToNull(encoding: sysEncoding);
                    }
                }

                //Read common path, if exist
                if (offsetCommonPath > 0)
                {
                    reader.Position = locInfoPos + offsetCommonPath;
                    var commonPath = (pathType & PathTypeFlags.IsUnicodeCommonPath) != 0
                        ? reader.ReadStringToNull(encoding: Encoding.Unicode)
                        : reader.ReadStringToNull(encoding: sysEncoding);
                    targetPath = Path.Combine(targetPath, commonPath);
                }
            }
            return targetPath;
        }

        private static Encoding GetSysEncoding()
        {
#if !NETFRAMEWORK
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            return Encoding.GetEncoding(0);
        }
    }
}
