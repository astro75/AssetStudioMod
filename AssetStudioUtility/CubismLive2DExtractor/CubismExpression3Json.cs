namespace CubismLive2DExtractor
{
    public enum BlendType
    {
        Add,
        Multiply,
        Overwrite,
    }

    public class CubismExpression3Json
    {
        public string Type;
        public float FadeInTime;
        public float FadeOutTime;
        public SerializableExpressionParameter[] Parameters;

        public class SerializableExpressionParameter
        {
            public string Id;
            public float Value;
            public BlendType Blend;
        }
    }
}
