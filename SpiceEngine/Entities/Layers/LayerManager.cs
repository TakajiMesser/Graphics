using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities.Layers
{
    public enum LayerStates
    {
        Neutral,
        Enabled,
        Disabled
    }

    public class LayerManager
    {
        public const string ROOT_LAYER_NAME = "Root";

        private Dictionary<string, EntityLayer> _layersByName = new Dictionary<string, EntityLayer>();

        private Dictionary<string, LayerStates> _renderLayerStatesByName = new Dictionary<string, LayerStates>();
        private Dictionary<string, LayerStates> _scriptLayerStatesByName = new Dictionary<string, LayerStates>();
        private Dictionary<string, LayerStates> _physicsLayerStatesByName = new Dictionary<string, LayerStates>();
        private Dictionary<string, LayerStates> _selectLayerStatesByName = new Dictionary<string, LayerStates>();

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

            _renderLayerStatesByName[layer.Name] = LayerStates.Neutral;
            _scriptLayerStatesByName[layer.Name] = LayerStates.Neutral;
            _physicsLayerStatesByName[layer.Name] = LayerStates.Neutral;
            _selectLayerStatesByName[layer.Name] = LayerStates.Neutral;

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

        public LayerStates GetRenderLayerState(string name) => _renderLayerStatesByName[name];
        public LayerStates GetScriptLayerState(string name) => _scriptLayerStatesByName[name];
        public LayerStates GetPhysicsLayerState(string name) => _physicsLayerStatesByName[name];
        public LayerStates GetSelectLayerState(string name) => _selectLayerStatesByName[name];

        public void SetRenderLayerState(string name, LayerStates state) => _renderLayerStatesByName[name] = state;
        public void SetScriptLayerState(string name, LayerStates state) => _scriptLayerStatesByName[name] = state;
        public void SetPhysicsLayerState(string name, LayerStates state) => _physicsLayerStatesByName[name] = state;
        public void SetSelectLayerState(string name, LayerStates state) => _selectLayerStatesByName[name] = state;

        public IEnumerable<EntityLayer> GetLayers(int entityID) => _layersByName.Values.Where(l => l.Contains(entityID));

        private IEnumerable<int> GetEntityIDs2(IEnumerable<string> layerNames, Dictionary<string, LayerStates> layerStatesByName)
        {
            var enabledIDs = GetIDSet(LayerStates.Enabled, layerNames, layerStatesByName);
            var disabledIDs = GetIDSet(LayerStates.Disabled, layerNames, layerStatesByName);
            var neutralIDs = GetIDSet(LayerStates.Neutral, layerNames, layerStatesByName);

            // Return ALL enabled ID's
            foreach (var id in enabledIDs)
            {
                yield return id;
            }

            foreach (var id in neutralIDs.Except(enabledIDs).Except(disabledIDs))
            {
                yield return id;
            }
        }

        private HashSet<int> GetIDSet(LayerStates layerState, IEnumerable<string> layerNames, Dictionary<string, LayerStates> layerStatesByName)
        {
            var ids = new HashSet<int>();

            foreach (var layerName in layerNames)
            {
                var state = layerStatesByName[layerName];
                if (layerState == state)
                {
                    foreach (var id in _layersByName[layerName].EntityIDs)
                    {
                        ids.Add(id);
                    }
                }
            }

            return ids;
        }

        private IEnumerable<int> GetEntityIDs(IEnumerable<string> layerNames, Dictionary<string, LayerStates> layerStatesByName)
        {
            return GetEntityIDs2(layerNames, layerStatesByName);
            /*var entityIDs = new HashSet<int>();
            var excludeIDs = new HashSet<int>();

            foreach (var layerName in layerNames)
            {
                switch (layerStatesByName[layerName])
                {
                    case LayerStates.Neutral:
                        foreach (var entityID in _layersByName[layerName].EntityIDs)
                        {
                            if (!entityIDs.Contains(entityID) && !excludeIDs.Contains(entityID))
                            {
                                entityIDs.Add(entityID);
                                yield return entityID;
                            }
                        }
                        break;
                    case LayerStates.Enabled:
                        foreach (var entityID in _layersByName[layerName].EntityIDs)
                        {
                            if (!entityIDs.Contains(entityID))
                            {
                                entityIDs.Add(entityID);
                                yield return entityID;
                            }
                        }
                        break;
                    case LayerStates.Disabled:
                        excludeIDs.UnionWith(_layersByName[layerName].EntityIDs);
                        break;
                    default:
                        throw new NotImplementedException("Could not handle LayerStates " + layerStatesByName[layerName]);
                }
            }*/
        }
    }
}
