using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Animations
{
    public class Animation
    {
        public string Name { get; private set; }
        public float Duration { get; private set; }
        public List<KeyFrame> KeyFrames { get; private set; } = new List<KeyFrame>();

        public Animation(string name)
        {
            Name = name;
        }
    }
}
