using SpiceEngineCore.Components.Animations;
using System.Collections.Generic;

namespace SweetGraphicsCore.Rendering.Animations
{
    public class KeyFrame : IKeyFrame
    {
        public float Time { get; set; }
        public List<JointTransform> Transforms { get; private set; } = new List<JointTransform>();
        //public Vector4 Bezier { get; set; }
    }
}
