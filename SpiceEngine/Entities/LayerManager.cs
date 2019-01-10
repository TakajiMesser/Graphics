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
    public class LayerManager
    {
        private Dictionary<string, EntityLayer> _layersByName = new Dictionary<string, EntityLayer>();

        public List<string> RenderLayerNames { get; } = new List<string>();
        public List<string> ScriptLayerNames { get; } = new List<string>();
        public List<string> PhysicsLayerNames { get; } = new List<string>();

        public IEnumerable<int> EntityRenderIDs => GetEntityIDs(RenderLayerNames);
        public IEnumerable<int> EntityScriptIDs => GetEntityIDs(ScriptLayerNames);
        public IEnumerable<int> EntityPhysicsIDs => GetEntityIDs(PhysicsLayerNames);

        public void AddEntityLayer(EntityLayer layer)
        {
            _layersByName.Add(layer.Name, layer);

            RenderLayerNames.Add(layer.Name);
            ScriptLayerNames.Add(layer.Name);
            PhysicsLayerNames.Add(layer.Name);
        }

        public void Clear()
        {
            _layersByName.Clear();

            RenderLayerNames.Clear();
            ScriptLayerNames.Clear();
            PhysicsLayerNames.Clear();
        }

        private IEnumerable<int> GetEntityIDs(IEnumerable<string> layerNames)
        {
            var entityIDs = new HashSet<int>();

                foreach (var name in layerNames)
                {
                    foreach (var id in _layersByName[name].EntityIDs)
                    {
                        if (!entityIDs.Contains(id))
                        {
                            entityIDs.Add(id);
                            yield return id;
                        }
                    }
                }
        }
    }
}
