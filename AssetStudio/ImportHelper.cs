using Org.Brotli.Dec;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using K4os.Compression.LZ4;

namespace AssetStudio
{
    public static class ImportHelper
    {
        public static void MergeSplitAssets(string path, bool allDirectories = false)
        {
            var splitFiles = Directory.GetFiles(path, "*.split0", allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var splitFile in splitFiles)
            {
                var destFile = Path.GetFileNameWithoutExtension(splitFile);
                var destPath = Path.GetDirectoryName(splitFile);
                var destFull = Path.Combine(destPath, destFile);
                if (!File.Exists(destFull))
                {
                    var splitParts = Directory.GetFiles(destPath, destFile + ".split*");
                    using (var destStream = File.Create(destFull))
                    {
                        for (int i = 0; i < splitParts.Length; i++)
                        {
                            var splitPart = destFull + ".split" + i;
                            using (var sourceStream = File.OpenRead(splitPart))
                            {
                                sourceStream.CopyTo(destStream);
                            }
                        }
                    }
                }
            }
        }

        public static string[] ProcessingSplitFiles(List<string> selectFile)
        {
            var splitFiles = selectFile.Where(x => x.Contains(".split"))
                .Select(x => Path.Combine(Path.GetDirectoryName(x), Path.GetFileNameWithoutExtension(x)))
                .Distinct()
                .ToList();
            selectFile.RemoveAll(x => x.Contains(".split"));
            foreach (var file in splitFiles)
            {
                if (File.Exists(file))
                {
                    selectFile.Add(file);
                }
            }
            return selectFile.Distinct().ToArray();
        }

        public static FileReader DecompressGZip(FileReader reader)
        {
            try
            {
                using (reader)
                {
                    var stream = new MemoryStream();
                    using (var gs = new GZipStream(reader.BaseStream, CompressionMode.Decompress))
                    {
                        gs.CopyTo(stream);
                    }
                    stream.Position = 0;
                    return new FileReader(reader.FullPath, stream);
                }
            }
            catch (System.Exception e)
            {
                Logger.Warning($"Error while decompressing gzip file {reader.FullPath}\r\n{e}");
                reader.Dispose();
                return null;
            }
        }
        
        public static uint CompressGZipAndGetSize(byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return (uint) memoryStream.Length;
            }
        }
        
        public static uint CompressLZ4AndGetSize(byte[] data)
        {
            return (uint) LZ4Pickler.Pickle(data, LZ4Level.L05_HC).LongLength;
        }

        public static FileReader DecompressBrotli(FileReader reader)
        {
            using (reader)
            {
                var stream = new MemoryStream();
                using (var brotliStream = new BrotliInputStream(reader.BaseStream))
                {
                    brotliStream.CopyTo(stream);
                }
                stream.Position = 0;
                return new FileReader(reader.FullPath, stream);
            }
        }
    }
}
