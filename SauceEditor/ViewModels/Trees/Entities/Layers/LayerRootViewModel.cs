using SauceEditor.Views.Trees.Entities;
using SpiceEngineCore.Entities.Layers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SauceEditor.ViewModels.Trees.Entities.Layers
{
    public class LayerRootViewModel : ViewModel
    {
        private LayerTypes _layerType;
        private ILayerProvider _layerProvider;
        private IRearrange _rearranger;

        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<LayerViewModel> Children { get; set; }

        public LayerRootViewModel(LayerTypes layerType, ILayerProvider layerProvider, IRearrange rearranger)
        {
            _layerType = layerType;
            _layerProvider = layerProvider;
            _rearranger = rearranger;

            Name = "Layers";

            var children = GetChildren().ToList();
            Children = new ReadOnlyCollection<LayerViewModel>(children);
        }

        public void MoveChild(string layerName, int moveIndex)
        {
            _layerProvider.MoveLayerOrder(_layerType, layerName, moveIndex);
            var children = GetChildren().ToList();
            Children = new ReadOnlyCollection<LayerViewModel>(children);
        }

        private IEnumerable<LayerViewModel> GetChildren()
        {
            foreach (var layerName in _layerProvider.GetLayerNames(_layerType))
            {
                yield return new LayerViewModel(layerName, _layerType, _layerProvider, _rearranger);
                yield return new LayerViewModel(layerName + "2", _layerType, _layerProvider, _rearranger);
            }
        }
    }
}