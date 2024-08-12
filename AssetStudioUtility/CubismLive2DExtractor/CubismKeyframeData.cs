namespace CubismLive2DExtractor
{
    public class CubismKeyframeData
    {
        public float time { get; set; }
        public float value { get; set; }
        public float inSlope { get; set; }
        public float outSlope { get; set; }
        public int weightedMode { get; set; }
        public float inWeight { get; set; }
        public float outWeight { get; set; }

        public CubismKeyframeData() { }

        public CubismKeyframeData(ImportedKeyframe<float> keyframe)
        {
            time = keyframe.time;
            value = keyframe.value;
            inSlope = keyframe.inSlope;
            outSlope = keyframe.outSlope;
            weightedMode = 0;
            inWeight = 0;
            outWeight = 0;
        }
    }
}
