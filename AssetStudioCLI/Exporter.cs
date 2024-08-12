using AssetStudio;
using AssetStudioCLI.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetStudioCLI
{
    internal static class Exporter
    {
        public static bool ExportVideoClip(AssetItem item, string exportPath)
        {
            var m_VideoClip = (VideoClip)item.Asset;
            if (m_VideoClip.m_ExternalResources.m_Size > 0)
            {
                if (!TryExportFile(exportPath, item, Path.GetExtension(m_VideoClip.m_OriginalPath), out var exportFullPath))
                    return false;

                if (CLIOptions.o_logLevel.Value <= LoggerEvent.Debug)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"VideoClip format: {m_VideoClip.m_Format}");
                    sb.AppendLine($"VideoClip width: {m_VideoClip.Width}");
                    sb.AppendLine($"VideoClip height: {m_VideoClip.Height}");
                    sb.AppendLine($"VideoClip frame rate: {m_VideoClip.m_FrameRate:.0##}");
                    sb.AppendLine($"VideoClip split alpha: {m_VideoClip.m_HasSplitAlpha}");
                    Logger.Debug(sb.ToString());
                }

                m_VideoClip.m_VideoData.WriteData(exportFullPath);
                Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
                return true;
            }
            return false;
        }

        public static bool ExportMovieTexture(AssetItem item, string exportPath)
        {
            var m_MovieTexture = (MovieTexture)item.Asset;
            if (!TryExportFile(exportPath, item, ".ogv", out var exportFullPath))
                return false;
            File.WriteAllBytes(exportFullPath, m_MovieTexture.m_MovieData);

            Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
            return true;
        }

        public static bool ExportShader(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".shader", out var exportFullPath))
                return false;
            var m_Shader = (Shader)item.Asset;
            var str = m_Shader.Convert();
            File.WriteAllText(exportFullPath, str);

            Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
            return true;
        }
        
        public static bool ExportTextAsset(AssetItem item, string exportPath)
        {
            var m_TextAsset = (TextAsset)item.Asset;
            var extension = ".txt";
            var assetExtension = Path.GetExtension(m_TextAsset.m_Name);
            if (!CLIOptions.f_notRestoreExtensionName.Value)
            {
                if (!string.IsNullOrEmpty(assetExtension))
                {
                    extension = "";
                }
                else if (!string.IsNullOrEmpty(item.Container))
                {
                    var ext = Path.GetExtension(item.Container);
                    if (!string.IsNullOrEmpty(item.Container))
                    {
                        extension = ext;
                    }
                }
            }
            if (!TryExportFile(exportPath, item, extension, out var exportFullPath))
                return false;
            File.WriteAllBytes(exportFullPath, m_TextAsset.m_Script);

            Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
            return true;
        }
        
        public static bool ExportMonoBehaviour(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".json", out var exportFullPath))
                return false;
            var m_MonoBehaviour = (MonoBehaviour)item.Asset;
            var type = m_MonoBehaviour.ToType();
            if (type == null)
            {
                var m_Type = m_MonoBehaviour.ConvertToTypeTree(Studio.assemblyLoader);
                type = m_MonoBehaviour.ToType(m_Type);
            }
            if (type != null)
            {
                var str = JsonConvert.SerializeObject(type, Formatting.Indented);
                File.WriteAllText(exportFullPath, str);

                Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
                return true;
            }
            return false;
        }

        public static bool ExportFont(AssetItem item, string exportPath)
        {
            var m_Font = (Font)item.Asset;
            if (m_Font.m_FontData != null)
            {
                var extension = ".ttf";
                if (m_Font.m_FontData[0] == 79 && m_Font.m_FontData[1] == 84 && m_Font.m_FontData[2] == 84 && m_Font.m_FontData[3] == 79)
                {
                    extension = ".otf";
                }
                if (!TryExportFile(exportPath, item, extension, out var exportFullPath))
                    return false;
                File.WriteAllBytes(exportFullPath, m_Font.m_FontData);

                Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
                return true;
            }
            return false;
        }

        public static void ExportGameObject(GameObject gameObject, string exportPath, List<AssetItem> animationList = null)
        {
            var convert = animationList != null
                ? new ModelConverter(gameObject, CLIOptions.o_imageFormat.Value, animationList.Select(x => (AnimationClip)x.Asset).ToArray())
                : new ModelConverter(gameObject, CLIOptions.o_imageFormat.Value);
            exportPath = exportPath + FixFileName(gameObject.m_Name) + ".fbx";
            ExportFbx(convert, exportPath);
        }

        private static void ExportFbx(IImported convert, string exportPath)
        {
            var eulerFilter = true;
            var filterPrecision = 0.25f;
            var exportAllNodes = true;
            var exportSkins = true;
            var exportAnimations = true;
            var exportBlendShape = true;
            var castToBone = false;
            var boneSize = CLIOptions.o_fbxBoneSize.Value;
            var exportAllUvsAsDiffuseMaps = false;
            var scaleFactor = CLIOptions.o_fbxScaleFactor.Value;
            var fbxVersion = 3;
            var fbxFormat = 0;
            ModelExporter.ExportFbx(exportPath, convert, eulerFilter, filterPrecision,
                exportAllNodes, exportSkins, exportAnimations, exportBlendShape, castToBone, boneSize, exportAllUvsAsDiffuseMaps, scaleFactor, fbxVersion, fbxFormat == 1);
        }

        public static bool ExportRawFile(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".dat", out var exportFullPath, mode: "ExportRaw"))
                return false;
            File.WriteAllBytes(exportFullPath, item.Asset.GetRawData());

            Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
            return true;
        }

        public static bool ExportDumpFile(AssetItem item, string exportPath)
        {
            if (!TryExportFile(exportPath, item, ".txt", out var exportFullPath, mode: "Dump"))
                return false;
            var str = item.Asset.Dump();
            if (str == null && item.Asset is MonoBehaviour m_MonoBehaviour)
            {
                var m_Type = m_MonoBehaviour.ConvertToTypeTree(Studio.assemblyLoader);
                str = m_MonoBehaviour.Dump(m_Type);
            }
            if (string.IsNullOrEmpty(str))
            {
                str = item.Asset.DumpObject();
            }
            if (str != null)
            {
                File.WriteAllText(exportFullPath, str);
                Logger.Debug($"{item.TypeString} \"{item.Text}\" saved to \"{exportFullPath}\"");
                return true;
            }
            return false;
        }

        private static bool TryExportFile(string dir, AssetItem item, string extension, out string fullPath, string mode = "Export")
        {
            var fileName = FixFileName(item.Text);
            var filenameFormat = CLIOptions.o_filenameFormat.Value;
            switch (filenameFormat)
            {
                case FilenameFormat.AssetName_PathID:
                    fileName = $"{fileName} @{item.m_PathID}";
                    break;
                case FilenameFormat.PathID:
                    fileName = item.m_PathID.ToString();
                    break;
            }
            fullPath = Path.Combine(dir, fileName + extension);
            if (!File.Exists(fullPath))
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            if (filenameFormat == FilenameFormat.AssetName)
            {
                fullPath = Path.Combine(dir, fileName + item.UniqueID + extension);
                if (!File.Exists(fullPath))
                {
                    Directory.CreateDirectory(dir);
                    return true;
                }
            }
            Logger.Error($"{mode} error. File \"{fullPath.Color(ColorConsole.BrightRed)}\" already exist");
            return false;
        }

        private static bool ExportMesh(AssetItem item, string exportPath)
        {
            var m_Mesh = (Mesh)item.Asset;
            if (m_Mesh.m_VertexCount <= 0)
                return false;
            if (!TryExportFile(exportPath, item, ".obj", out var exportFullPath))
                return false;
            var sb = new StringBuilder();
            sb.AppendLine("g " + m_Mesh.m_Name);

            #region Vertices

            if (m_Mesh.m_Vertices == null || m_Mesh.m_Vertices.Length == 0)
            {
                return false;
            }

            int c = 3;
            if (m_Mesh.m_Vertices.Length == m_Mesh.m_VertexCount * 4)
            {
                c = 4;
            }

            for (int v = 0; v < m_Mesh.m_VertexCount; v++)
            {
                sb.Append($"v {-m_Mesh.m_Vertices[v * c]} {m_Mesh.m_Vertices[v * c + 1]} {m_Mesh.m_Vertices[v * c + 2]}\r\n");
            }

            #endregion

            #region UV

            if (m_Mesh.m_UV0?.Length > 0)
            {
                c = 4;
                if (m_Mesh.m_UV0.Length == m_Mesh.m_VertexCount * 2)
                {
                    c = 2;
                }
                else if (m_Mesh.m_UV0.Length == m_Mesh.m_VertexCount * 3)
                {
                    c = 3;
                }

                for (int v = 0; v < m_Mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vt {0} {1}\r\n", m_Mesh.m_UV0[v * c], m_Mesh.m_UV0[v * c + 1]);
                }
            }

            #endregion

            #region Normals

            if (m_Mesh.m_Normals?.Length > 0)
            {
                if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 3)
                {
                    c = 3;
                }
                else if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 4)
                {
                    c = 4;
                }

                for (int v = 0; v < m_Mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vn {0} {1} {2}\r\n", -m_Mesh.m_Normals[v * c], m_Mesh.m_Normals[v * c + 1], m_Mesh.m_Normals[v * c + 2]);
                }
            }

            #endregion

            #region Face

            int sum = 0;
            for (var i = 0; i < m_Mesh.m_SubMeshes.Length; i++)
            {
                sb.AppendLine($"g {m_Mesh.m_Name}_{i}");
                int indexCount = (int)m_Mesh.m_SubMeshes[i].indexCount;
                var end = sum + indexCount / 3;
                for (int f = sum; f < end; f++)
                {
                    sb.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\r\n", m_Mesh.m_Indices[f * 3 + 2] + 1, m_Mesh.m_Indices[f * 3 + 1] + 1, m_Mesh.m_Indices[f * 3] + 1);
                }

                sum = end;
            }

            #endregion

            sb.Replace("NaN", "0");
            File.WriteAllText(exportFullPath, sb.ToString());
            Logger.Debug($"{item.TypeString} \"{item.Text}\" exported to \"{exportFullPath}\"");
            return true;
        }

        public static bool ExportConvertFile(AssetItem item, string exportPath)
        {
            switch (item.Type)
            {
                case ClassIDType.Texture2D:
                case ClassIDType.Texture2DArray:
                case ClassIDType.Sprite:
                case ClassIDType.AudioClip:
                    throw new System.NotImplementedException();
                case ClassIDType.VideoClip:
                    return ExportVideoClip(item, exportPath);
                case ClassIDType.MovieTexture:
                    return ExportMovieTexture(item, exportPath);
                case ClassIDType.Shader:
                    return ExportShader(item, exportPath);
                case ClassIDType.TextAsset:
                    return ExportTextAsset(item, exportPath);
                case ClassIDType.MonoBehaviour:
                    return ExportMonoBehaviour(item, exportPath);
                case ClassIDType.Font:
                    return ExportFont(item, exportPath);
                case ClassIDType.Mesh:
                    return ExportMesh(item, exportPath);
                default:
                    return ExportRawFile(item, exportPath);
            }
        }

        public static string FixFileName(string str)
        {
            return str.Length >= 260
                ? Path.GetRandomFileName()
                : Path.GetInvalidFileNameChars().Aggregate(str, (current, c) => current.Replace(c, '_'));
        }
    }
}
