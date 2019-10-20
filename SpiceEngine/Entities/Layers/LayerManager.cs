using SpiceEngineCore.Entities.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using SpiceEngineCore.Utilities;

namespace SpiceEngine.Entities.Layers
{
    public class LayerManager : ILayerProvider
    {
        public const string ROOT_LAYER_NAME = "Root";

        private Dictionary<string, EntityLayer> _layersByName = new Dictionary<string, EntityLayer>();

        private List<string> _renderLayerNames = new List<string>();
        private List<string> _scriptLayerNames = new List<string>();
        private List<string> _physicsLayerNames = new List<string>();
        private List<string> _selectLayerNames = new List<string>();

        private Dictionary<string, LayerStates> _renderLayerStatesByName = new Dictionary<string, LayerStates>();
        private Dictionary<string, LayerStates> _scriptLayerStatesByName = new Dictionary<string, LayerStates>();
        private Dictionary<string, LayerStates> _physicsLayerStatesByName = new Dictionary<string, LayerStates>();
        private Dictionary<string, LayerStates> _selectLayerStatesByName = new Dictionary<string, LayerStates>();

        public EntityLayer RootLayer { get; } = new EntityLayer(ROOT_LAYER_NAME);

        public LayerManager()
        {
            RootLayer = new EntityLayer(ROOT_LAYER_NAME);
            AddEntityLayer(RootLayer);
        }

        public void AddLayer(string name) => AddEntityLayer(new EntityLayer(name));

        public bool ContainsLayer(string name) => _layersByName.ContainsKey(name);

        public void RemoveLayer(string name) => _layersByName.Remove(name);

        public void ClearLayer(string name)
        {
            if (_layersByName.ContainsKey(name))
            {
                _layersByName[name].Clear();
            }
        }

        public void AddToLayer(string layerName, int entityID) => _layersByName[layerName].Add(entityID);

        public void RemoveFromLayer(string layerName, int entityID) => _layersByName[layerName].Remove(entityID);

        public IEnumerable<int> GetEntityIDs(LayerTypes layerType) => GetEntityIDs(GetLayerNames(layerType), GetLayerStatesByName(layerType));

        public void MoveLayerOrder(LayerTypes layerType, String layerName, int moveIndex)
        {
            switch (layerType)
            {
                case LayerTypes.Render:
                    _renderLayerNames.Move(_renderLayerNames.IndexOf(layerName), moveIndex);
                    break;
                case LayerTypes.Script:
                    _scriptLayerNames.Move(_scriptLayerNames.IndexOf(layerName), moveIndex);
                    break;
                case LayerTypes.Physics:
                    _physicsLayerNames.Move(_physicsLayerNames.IndexOf(layerName), moveIndex);
                    break;
                case LayerTypes.Select:
                    _selectLayerNames.Move(_selectLayerNames.IndexOf(layerName), moveIndex);
                    break;
            }

            throw new ArgumentOutOfRangeException("Could not handle layerType " + layerType);
        }

        public IEnumerable<string> GetLayerNames(LayerTypes layerType)
        {
            switch (layerType)
            {
                case LayerTypes.Render:
                    return _renderLayerNames;
                case LayerTypes.Script:
                    return _scriptLayerNames;
                case LayerTypes.Physics:
                    return _physicsLayerNames;
                case LayerTypes.Select:
                    return _selectLayerNames;
            }

            throw new ArgumentOutOfRangeException("Could not handle layerType " + layerType);
        }

        public LayerStates GetLayerState(LayerTypes layerType, string name) => GetLayerStatesByName(layerType)[name];

        public IEnumerable<int> GetLayerEntityIDs(string name) => _layersByName.ContainsKey(name)
            ? _layersByName[name].EntityIDs
            : Enumerable.Empty<int>();

        public void SetLayerState(string name, LayerStates state)
        {
            SetLayerState(LayerTypes.Render, name, state);
            SetLayerState(LayerTypes.Script, name, state);
            SetLayerState(LayerTypes.Physics, name, state);
            SetLayerState(LayerTypes.Select, name, state);
        }

        public void SetLayerState(LayerTypes layerType, string name, LayerStates state) => GetLayerStatesByName(layerType)[name] = state;

        public void Clear()
        {
            _layersByName.Clear();

            _renderLayerNames.Clear();
            _scriptLayerNames.Clear();
            _physicsLayerNames.Clear();
            _selectLayerNames.Clear();

            _renderLayerStatesByName.Clear();
            _scriptLayerStatesByName.Clear();
            _physicsLayerStatesByName.Clear();
            _selectLayerStatesByName.Clear();
        }

        private void AddEntityLayer(EntityLayer layer)
        {
            _layersByName.Add(layer.Name, layer);

            _renderLayerNames.Add(layer.Name);
            _scriptLayerNames.Add(layer.Name);
            _physicsLayerNames.Add(layer.Name);
            _selectLayerNames.Add(layer.Name);

            _renderLayerStatesByName[layer.Name] = LayerStates.Neutral;
            _scriptLayerStatesByName[layer.Name] = LayerStates.Neutral;
            _physicsLayerStatesByName[layer.Name] = LayerStates.Neutral;
            _selectLayerStatesByName[layer.Name] = LayerStates.Neutral;
        }

        private Dictionary<string, LayerStates> GetLayerStatesByName(LayerTypes layerType)
        {
            switch (layerType)
            {
                case LayerTypes.Render:
                    return _renderLayerStatesByName;
                case LayerTypes.Script:
                    return _scriptLayerStatesByName;
                case LayerTypes.Physics:
                    return _physicsLayerStatesByName;
                case LayerTypes.Select:
                    return _selectLayerStatesByName;
            }

            throw new ArgumentOutOfRangeException("Could not handle layerType " + layerType);
        }

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
