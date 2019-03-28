using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Volumes
{
    public class TriggerVolume : Volume
    {
        public TriggerVolume() { }
        /*public TriggerVolume(List<Vector3> vertices, List<int> triangleIndices, Color4 color) : base(vertices, triangleIndices, color)
        {

        }*/

        public void OnTriggered(IEntity entity)
        {
            
        }
    }
}
