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

        public const string ROOT_LAYER_NAME = "Root";

        private Dictionary<string, EntityLayer> _layersByName = new Dictionary<string, EntityLayer>();

        private Dictionary<string, LayerState> _renderLayerStatesByName = new Dictionary<string, LayerState>();
        private Dictionary<string, LayerState> _scriptLayerStatesByName = new Dictionary<string, LayerState>();
        private Dictionary<string, LayerState> _physicsLayerStatesByName = new Dictionary<string, LayerState>();
        private Dictionary<string, LayerState> _selectLayerStatesByName = new Dictionary<string, LayerState>();

        public EntityLayer RootLayer { get; } = new EntityLayer(ROOT_LAYER_NAME);

        public List<string> RenderLayerNames { get; } = new List<string>();
        public List<string> ScriptLayerNames { get; } = new List<string>();
        public List<string> PhysicsLayerNames { get; } = new List<string>();
        public List<string> SelectLayerNames { get; } = new List<string>();

        public IEnumerable<int> EntityRenderIDs => GetEntityIDs(RenderLayerNames, _renderLayerStatesByName);
        public IEnumerable<int> EntityScriptIDs => GetEntityIDs(ScriptLayerNames, _scriptLayerStatesByName);
        public IEnumerable<int> EntityPhysicsIDs => GetEntityIDs(PhysicsLayerNames, _physicsLayerStatesByName);
        public IEnumerable<int> EntitySelectIDs => GetEntityIDs(SelectLayerNames, _selectLayerStatesByName);

        public LayerManager()
        {
            RootLayer = new EntityLayer(ROOT_LAYER_NAME);
            AddEntityLayer(RootLayer);
        }

        public void AddToLayer(string layerName, int entityID) => _layersByName[layerName].Add(entityID);

        public void RemoveFromLayer(string layerName, int entityID) => _layersByName[layerName].Remove(entityID);

        public bool ContainsLayer(string layerName) => _layersByName.ContainsKey(layerName);

        public void AddLayer(string name) => AddEntityLayer(new EntityLayer(name));

        private void AddEntityLayer(EntityLayer layer)
        {
            _layersByName.Add(layer.Name, layer);

            _renderLayerStatesByName.Add(layer.Name, LayerState.Enabled);
            _scriptLayerStatesByName.Add(layer.Name, LayerState.Enabled);
            _physicsLayerStatesByName.Add(layer.Name, LayerState.Enabled);
            _selectLayerStatesByName.Add(layer.Name, LayerState.Enabled);

            RenderLayerNames.Add(layer.Name);
            ScriptLayerNames.Add(layer.Name);
            PhysicsLayerNames.Add(layer.Name);
            SelectLayerNames.Add(layer.Name);
        }

        public void Clear()
        {
            _layersByName.Clear();

            _renderLayerStatesByName.Clear();
            _scriptLayerStatesByName.Clear();
            _physicsLayerStatesByName.Clear();
            _selectLayerStatesByName.Clear();

            RenderLayerNames.Clear();
            ScriptLayerNames.Clear();
            PhysicsLayerNames.Clear();
            SelectLayerNames.Clear();
        }

        public IEnumerable<int> GetLayerEntityIDs(string layerName) => _layersByName.ContainsKey(layerName)
            ? _layersByName[layerName].EntityIDs
            : Enumerable.Empty<int>();

        public LayerState GetRenderLayerState(string name) => _renderLayerStatesByName[name];
        public LayerState GetScriptLayerState(string name) => _scriptLayerStatesByName[name];
        public LayerState GetPhysicsLayerState(string name) => _physicsLayerStatesByName[name];
        public LayerState GetSelectLayerState(string name) => _selectLayerStatesByName[name];

        public void SetRenderLayerState(string name, LayerState state) => _renderLayerStatesByName[name] = state;
        public void SetScriptLayerState(string name, LayerState state) => _scriptLayerStatesByName[name] = state;
        public void SetPhysicsLayerState(string name, LayerState state) => _physicsLayerStatesByName[name] = state;
        public void SetSelectLayerState(string name, LayerState state) => _selectLayerStatesByName[name] = state;

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
