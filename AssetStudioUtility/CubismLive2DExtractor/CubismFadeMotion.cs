using System;

namespace CubismLive2DExtractor
{
    public sealed class AnimationCurve
    {
        public CubismKeyframeData[] m_Curve { get; set; }
        public int m_PreInfinity {  get; set; }
        public int m_PostInfinity { get; set; }
        public int m_RotationOrder { get; set; }
    }

    public sealed class CubismFadeMotion
    {
        public string m_Name { get; set; }
        public string MotionName { get; set; }
        public float FadeInTime { get; set; }
        public float FadeOutTime { get; set; }
        public string[] ParameterIds { get; set; }
        public AnimationCurve[] ParameterCurves { get; set; }
        public float[] ParameterFadeInTimes { get; set; }
        public float[] ParameterFadeOutTimes { get; set; }
        public float MotionLength { get; set; }

        public CubismFadeMotion()
        {
            ParameterIds = Array.Empty<string>();
        }
    }
}
