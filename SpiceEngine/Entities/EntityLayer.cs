using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Entities
{
    public class EntityLayer
    {
        private List<int> _entityIDs = new List<int>();

        public string Name { get; private set; }
        public IEnumerable<int> EntityIDs => _entityIDs;

        public EntityLayer(string name)
        {
            Name = name;
        }
    }
}
