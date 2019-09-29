using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Animations
{
    public class KeyFrame
    {
        public float Time { get; set; }
        public List<JointTransform> Transforms { get; private set; } = new List<JointTransform>();
        //public Vector4 Bezier { get; set; }
    }
}
