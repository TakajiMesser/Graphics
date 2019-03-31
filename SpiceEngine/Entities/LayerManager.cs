using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities
{
    public class LayerManager
    {
        public enum LayerState
        {
            Enabled,
            Disabled
        }

        private Dictionary<string, EntityLayer> _layersByName = new Dictionary<string, EntityLayer>();

        private Dictionary<string, LayerState> _renderLayerStatesByName = new Dictionary<string, LayerState>();
        private Dictionary<string, LayerState> _scriptLayerStatesByName = new Dictionary<string, LayerState>();
        private Dictionary<string, LayerState> _physicsLayerStatesByName = new Dictionary<string, LayerState>();

        public EntityLayer RootLayer { get; } = new EntityLayer("Root");

        public List<string> RenderLayerNames { get; } = new List<string>();
        public List<string> ScriptLayerNames { get; } = new List<string>();
        public List<string> PhysicsLayerNames { get; } = new List<string>();

        public IEnumerable<int> EntityRenderIDs => GetEntityIDs(RenderLayerNames, _renderLayerStatesByName);
        public IEnumerable<int> EntityScriptIDs => GetEntityIDs(ScriptLayerNames, _scriptLayerStatesByName);
        public IEnumerable<int> EntityPhysicsIDs => GetEntityIDs(PhysicsLayerNames, _physicsLayerStatesByName);

        public LayerManager()
        {
            RootLayer = new EntityLayer("Root");
            AddEntityLayer(RootLayer);
        }

        public void AddEntityLayer(EntityLayer layer)
        {
            _layersByName.Add(layer.Name, layer);

            _renderLayerStatesByName.Add(layer.Name, LayerState.Enabled);
            _scriptLayerStatesByName.Add(layer.Name, LayerState.Enabled);
            _physicsLayerStatesByName.Add(layer.Name, LayerState.Enabled);

            RenderLayerNames.Add(layer.Name);
            ScriptLayerNames.Add(layer.Name);
            PhysicsLayerNames.Add(layer.Name);
        }

        public void Clear()
        {
            _layersByName.Clear();

            _renderLayerStatesByName.Clear();
            _scriptLayerStatesByName.Clear();
            _physicsLayerStatesByName.Clear();

            RenderLayerNames.Clear();
            ScriptLayerNames.Clear();
            PhysicsLayerNames.Clear();
        }

        public LayerState GetRenderLayerState(string name) => _renderLayerStatesByName[name];
        public LayerState GetScriptLayerState(string name) => _scriptLayerStatesByName[name];
        public LayerState GetPhysicsLayerState(string name) => _physicsLayerStatesByName[name];

        public void SetRenderLayerState(string name, LayerState state) => _renderLayerStatesByName[name] = state;
        public void SetScriptLayerState(string name, LayerState state) => _scriptLayerStatesByName[name] = state;
        public void SetPhysicsLayerState(string name, LayerState state) => _physicsLayerStatesByName[name] = state;

        public IEnumerable<EntityLayer> GetLayers(int entityID) => _layersByName.Values.Where(l => l.Contains(entityID));

        private IEnumerable<int> GetEntityIDs(IEnumerable<string> layerNames, Dictionary<string, LayerState> layerStatesByName)
        {
            var entityIDs = new HashSet<int>();
            var excludeIDs = new HashSet<int>();

            foreach (var layerName in layerNames)
            {
                switch (layerStatesByName[layerName])
                {
                    case LayerState.Enabled:
                        foreach (var entityID in _layersByName[layerName].EntityIDs)
                        {
                            if (!entityIDs.Contains(entityID) && !excludeIDs.Contains(entityID))
                            {
                                entityIDs.Add(entityID);
                                yield return entityID;
                            }
                        }
                        break;
                    case LayerState.Disabled:
                        excludeIDs.UnionWith(_layersByName[layerName].EntityIDs);
                        break;
                    default:
                        throw new NotImplementedException("Could not handle LayerState " + layerStatesByName[layerName]);
                }
            }
        }
    }
}
