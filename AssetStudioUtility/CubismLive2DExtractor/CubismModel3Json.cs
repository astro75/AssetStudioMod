using Newtonsoft.Json.Linq;

namespace CubismLive2DExtractor
{
    public class CubismModel3Json
    {
        public int Version;
        public string Name;
        public SerializableFileReferences FileReferences;
        public SerializableGroup[] Groups;

        public class SerializableFileReferences
        {
            public string Moc;
            public string[] Textures;
            public string DisplayInfo;
            public string Physics;
            public JObject Motions;
            public JArray Expressions;
        }

        public class SerializableGroup
        {
            public string Target;
            public string Name;
            public string[] Ids;
        }
    }
}
