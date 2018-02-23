using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Animations
{
    public class MeshTransforms
    {
        public int MeshIndex { get; private set; }
        public Dictionary<int, Matrix4> TransformsByBoneIndex { get; private set; } = new Dictionary<int, Matrix4>();

        public MeshTransforms(int meshIndex)
        {
            MeshIndex = meshIndex;
        }
    }
}
