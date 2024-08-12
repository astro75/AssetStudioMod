using System.Collections.Generic;
using System.Linq;
using AssetStudio;

namespace CubismLive2DExtractor
{
    public sealed class CubismObjectList
    {
        public static SerializedFile AssetsFile { get; set; }
        public HashSet<ObjectData> CubismExpressionObjects { get; set; }
        public HashSet<ObjectData> CubismFadeMotionObjects { get; set; }

        public class ObjectData
        {
            private long _pathID;
            public Object Asset { get; set; }
            public int m_FileID { get; set; }
            public long m_PathID
            {
                get => _pathID;
                set
                {
                    _pathID = value;
                    Asset = GetObjByPathID(_pathID);
                }
            }

            public override bool Equals(object obj)
            {
                return obj is ObjectData objectData && _pathID == objectData.m_PathID;
            }

            public override int GetHashCode()
            {
                return _pathID.GetHashCode();
            }
        }

        public List<MonoBehaviour> GetFadeMotionAssetList()
        {
            return CubismFadeMotionObjects?.Where(x => x.Asset != null).Select(x => (MonoBehaviour)x.Asset).ToList();
        }

        public List<MonoBehaviour> GetExpressionList()
        {
            return CubismExpressionObjects?.Where(x => x.Asset != null).Select(x => (MonoBehaviour)x.Asset).ToList();
        }

        private static Object GetObjByPathID(long pathID)
        {
            var assetFileList = AssetsFile.assetsManager.assetsFileList;
            foreach (var assetFile in assetFileList)
            {
                if (assetFile.ObjectsDic.TryGetValue(pathID, out var obj))
                {
                    return obj;
                }
            }
            return null;
        }
    }
}
