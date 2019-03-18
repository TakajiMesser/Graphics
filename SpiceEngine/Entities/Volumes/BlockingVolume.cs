using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Volumes
{
    public class BlockingVolume : Volume
    {
        public BlockingVolume() { }
        public BlockingVolume(List<Vector3> vertices, List<int> triangleIndices, Color4 color) : base(vertices, triangleIndices, color)
        {

        }
    }
}
