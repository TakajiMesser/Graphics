using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Animations
{
    public class JointIndex
    {
        public int MeshIndex { get; private set; }
        public int BoneIndex { get; private set; }

        public JointIndex(int meshIndex, int boneIndex)
        {
            MeshIndex = meshIndex;
            BoneIndex = boneIndex;
        }
    }
}
