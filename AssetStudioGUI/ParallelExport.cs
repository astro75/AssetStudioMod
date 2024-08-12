﻿using AssetStudio;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetStudioGUI
{
    internal static class ParallelExporter
    {
        private static ConcurrentDictionary<string, bool> savePathHash = new ConcurrentDictionary<string, bool>();

        public static bool ExportTexture2D(AssetItem item, string exportPath, out string debugLog)
        {
            debugLog = "";
            var m_Texture2D = (Texture2D)item.Asset;
            if (Properties.Settings.Default.convertTexture)
            {
                var type = Properties.Settings.Default.convertType;
                if (!TryExportFile(exportPath, item, "." + type.ToString().ToLower(), out var exportFullPath))
                    return false;

                if (GUILogger.ShowDebugMessage)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Converting {item.TypeString} \"{m_Texture2D.m_Name}\" to {type}..");
                    sb.AppendLine($"Width: {m_Texture2D.m_Width}");
                    sb.AppendLine($"Height: {m_Texture2D.m_Height}");
                    sb.AppendLine($"Format: {m_Texture2D.m_TextureFormat}");
                    switch (m_Texture2D.m_TextureSettings.m_FilterMode)
                    {
                        case 0: sb.AppendLine("Filter Mode: Point "); break;
                        case 1: sb.AppendLine("Filter Mode: Bilinear "); break;
                        case 2: sb.AppendLine("Filter Mode: Trilinear "); break;
                    }
                    sb.AppendLine($"Anisotropic level: {m_Texture2D.m_TextureSettings.m_Aniso}");
                    sb.AppendLine($"Mip map bias: {m_Texture2D.m_TextureSettings.m_MipBias}");
                    switch (m_Texture2D.m_TextureSettings.m_WrapMode)
                    {
                        case 0: sb.AppendLine($"Wrap mode: Repeat"); break;
                        case 1: sb.AppendLine($"Wrap mode: Clamp"); break;
                    }
                    debugLog += sb.ToString();
                }

                var image = m_Texture2D.ConvertToImage(flip: true);
                if (image == null)
                {
                    Logger.Warning($"Failed to convert texture \"{m_Texture2D.m_Name}\" into image");
                    return false;
                }
                using (image)
                {
                    using (var file = File.OpenWrite(exportFullPath))
                    {
                        image.WriteToStream(file, type);
                    }
                    debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                    return true;
                }
            }
            else
            {
                if (!TryExportFile(exportPath, item, ".tex", out var exportFullPath))
                    return false;
                File.WriteAllBytes(exportFullPath, m_Texture2D.image_data.GetData());
                debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                return true;
            }
        }

        public static bool ExportSprite(AssetItem item, string exportPath, out string debugLog)
        {
            debugLog = "";
            var type = Properties.Settings.Default.convertType;
            var spriteMaskMode = Properties.Settings.Default.exportSpriteWithMask ? SpriteMaskMode.Export : SpriteMaskMode.Off;
            if (!TryExportFile(exportPath, item, "." + type.ToString().ToLower(), out var exportFullPath))
                return false;
            var image = ((Sprite)item.Asset).GetImage(spriteMaskMode: spriteMaskMode);
            if (image != null)
            {
                using (image)
                {
                    using (var file = File.OpenWrite(exportFullPath))
                    {
                        image.WriteToStream(file, type);
                    }
                    debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                    return true;
                }
            }
            return false;
        }

        public static bool ExportAudioClip(AssetItem item, string exportPath, out string debugLog)
        {
            debugLog = "";
            string exportFullPath;
            var m_AudioClip = (AudioClip)item.Asset;
            var m_AudioData = BigArrayPool<byte>.Shared.Rent(m_AudioClip.m_AudioData.Size);
            try
            {
                m_AudioClip.m_AudioData.GetData(m_AudioData);
                if (m_AudioData == null || m_AudioData.Length == 0)
                {
                    Logger.Warning($"Failed to export \"{item.Text}\": AudioData was not found");
                    return false;
                }
                var converter = new AudioClipConverter(m_AudioClip);
                if (Properties.Settings.Default.convertAudio && converter.IsSupport)
                {
                    if (!TryExportFile(exportPath, item, ".wav", out exportFullPath))
                        return false;

                    if (GUILogger.ShowDebugMessage)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine($"Converting {item.TypeString} \"{m_AudioClip.m_Name}\" to wav..");
                        sb.AppendLine(m_AudioClip.version < 5
                            ? $"AudioClip type: {m_AudioClip.m_Type}"
                            : $"AudioClip compression format: {m_AudioClip.m_CompressionFormat}");
                        sb.AppendLine($"AudioClip channel count: {m_AudioClip.m_Channels}");
                        sb.AppendLine($"AudioClip sample rate: {m_AudioClip.m_Frequency}");
                        sb.AppendLine($"AudioClip bit depth: {m_AudioClip.m_BitsPerSample}");
                        debugLog += sb.ToString();
                    }

                    var buffer = converter.ConvertToWav(m_AudioData, out var debugLogConverter);
                    debugLog += debugLogConverter;
                    if (buffer == null)
                    {
                        Logger.Warning($"{debugLog}Failed to export \"{item.Text}\": Failed to convert fmod audio to Wav");
                        return false;
                    }
                    File.WriteAllBytes(exportFullPath, buffer);
                }
                else
                {
                    if (!TryExportFile(exportPath, item, converter.GetExtensionName(), out exportFullPath))
                        return false;

                    if (GUILogger.ShowDebugMessage)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine($"Exporting non-fmod {item.TypeString} \"{m_AudioClip.m_Name}\"..");
                        sb.AppendLine(m_AudioClip.version < 5
                            ? $"AudioClip type: {m_AudioClip.m_Type}"
                            : $"AudioClip compression format: {m_AudioClip.m_CompressionFormat}");
                        sb.AppendLine($"AudioClip channel count: {m_AudioClip.m_Channels}");
                        sb.AppendLine($"AudioClip sample rate: {m_AudioClip.m_Frequency}");
                        sb.AppendLine($"AudioClip bit depth: {m_AudioClip.m_BitsPerSample}");
                        debugLog += sb.ToString();
                    }
                    File.WriteAllBytes(exportFullPath, m_AudioData);
                }
                debugLog += $"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"";
                return true;
            }
            finally
            {
                BigArrayPool<byte>.Shared.Return(m_AudioData, clearArray: true);
            }
        }

        private static bool TryExportFile(string dir, AssetItem item, string extension, out string fullPath)
        {
            var fileName = FixFileName(item.Text);
            var filenameFormatIndex = Properties.Settings.Default.filenameFormat;
            switch (filenameFormatIndex)
            {
                case 1: //assetName@pathID
                    fileName = $"{fileName} @{item.m_PathID}";
                    break;
                case 2: //pathID
                    fileName = item.m_PathID.ToString();
                    break;
            }
            fullPath = Path.Combine(dir, fileName + extension);
            if (savePathHash.TryAdd(fullPath.ToLower(), true) && !File.Exists(fullPath))
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            if (filenameFormatIndex == 0) //assetName
            {
                fullPath = Path.Combine(dir, fileName + item.UniqueID + extension);
                if (!File.Exists(fullPath))
                {
                    Directory.CreateDirectory(dir);
                    return true;
                }
            }
            Logger.Warning($"Export failed. File \"{fullPath.Color(ColorConsole.BrightYellow)}\" already exist");
            return false;
        }

        public static bool ParallelExportConvertFile(AssetItem item, string exportPath, out string debugLog)
        {
            switch (item.Type)
            {
                case ClassIDType.Texture2D:
                case ClassIDType.Texture2DArrayImage:
                    return ExportTexture2D(item, exportPath, out debugLog);
                case ClassIDType.Sprite:
                    return ExportSprite(item, exportPath, out debugLog);
                case ClassIDType.AudioClip:
                    return ExportAudioClip(item, exportPath, out debugLog);
                default:
                    throw new NotImplementedException();
            }
        }

        private static string FixFileName(string str)
        {
            return str.Length >= 260
                ? Path.GetRandomFileName()
                : Path.GetInvalidFileNameChars().Aggregate(str, (current, c) => current.Replace(c, '_'));
        }

        public static void ClearHash()
        {
            savePathHash.Clear();
        }
    }
}
