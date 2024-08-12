using System.Collections.Generic;

namespace AssetStudio
{
    public class AssetInfo
    {
        public int preloadIndex;
        public int preloadSize;
        public PPtr<Object> asset;

        public AssetInfo(ObjectReader reader)
        {
            preloadIndex = reader.ReadInt32();
            preloadSize = reader.ReadInt32();
            asset = new PPtr<Object>(reader);
        }
    }

    public sealed class AssetBundle : NamedObject
    {
        public PPtr<Object>[] m_PreloadTable;
        public KeyValuePair<string, AssetInfo>[] m_Container;
        public string m_AssetBundleName;
        public string[] m_Dependencies;
        public bool m_IsStreamedSceneAssetBundle;

        public AssetBundle(ObjectReader reader) : base(reader)
        {
            var m_PreloadTableSize = reader.ReadInt32();
            m_PreloadTable = new PPtr<Object>[m_PreloadTableSize];
            for (var i = 0; i < m_PreloadTableSize; i++)
            {
                m_PreloadTable[i] = new PPtr<Object>(reader);
            }

            var m_ContainerSize = reader.ReadInt32();
            m_Container = new KeyValuePair<string, AssetInfo>[m_ContainerSize];
            for (var i = 0; i < m_ContainerSize; i++)
            {
                m_Container[i] = new KeyValuePair<string, AssetInfo>(reader.ReadAlignedString(), new AssetInfo(reader));
            }

            var m_MainAsset = new AssetInfo(reader);

            if (version == (5, 4)) //5.4.x
            {
                var m_ClassVersionMapSize = reader.ReadInt32();
                for (var i = 0; i < m_ClassVersionMapSize; i++)
                {
                    var first = reader.ReadInt32();
                    var second = reader.ReadInt32();
                }
            }

            if (version >= (4, 2)) //4.2 and up
            {
                var m_RuntimeCompatibility = reader.ReadUInt32();
            }

            if (version >= 5) //5.0 and up
            {
                m_AssetBundleName = reader.ReadAlignedString();

                var m_DependenciesSize = reader.ReadInt32();
                m_Dependencies = new string[m_DependenciesSize];

                for (var i = 0; i < m_DependenciesSize; i++)
                {
                    m_Dependencies[i] = reader.ReadAlignedString();
                }

                m_IsStreamedSceneAssetBundle = reader.ReadBoolean();
            }
        }
    }
}
