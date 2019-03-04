using OpenTK;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Volumes
{
    public class TriggerVolume : Volume
    {
        public TriggerVolume() { }
        public TriggerVolume(List<Vector3> vertices, List<int> triangleIndices, Vector4 color) : base(vertices, triangleIndices, color)
        {

        }
    }
}
