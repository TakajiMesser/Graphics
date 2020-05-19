using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities.Layers
{
    public enum LayerStates
    {
        Neutral,
        Enabled,
        Disabled
    }

    public enum LayerTypes
    {
        Render,
        Script,
        Physics,
        Select
    }

    public interface ILayerProvider
    {
        void AddLayer(string name);
        bool ContainsLayer(string name);
        void RemoveLayer(string name);
        void ClearLayer(string name);

        void AddToLayer(string name, int entityID);
        void RemoveFromLayer(string name, int entityID);

        IEnumerable<int> GetEntityIDs(LayerTypes layerType);
        IEnumerable<string> GetLayerNames(LayerTypes layerType);
        LayerStates GetLayerState(LayerTypes layerType, string name);
        IEnumerable<int> GetLayerEntityIDs(string name);

        void MoveLayerOrder(LayerTypes layerType, String layerName, int moveIndex);

        void SetLayerState(string name, LayerStates state);
        void SetLayerState(LayerTypes layerType, string name, LayerStates state);
        
        void Clear();
    }
}
