using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Trees.Entities.Layers;
using SauceEditor.Views.Factories;
using SauceEditor.Views.Trees.Entities;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Entities.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public class EntityTreePanelViewModel : DockableViewModel, IDisplayEntities
    {
        private List<EntityRootViewModel> _entityRoots = new List<EntityRootViewModel>();
        private List<LayerRootViewModel> _layerRoots = new List<LayerRootViewModel>();

        public EntityTreePanelViewModel()
        {
            EntityRoots = new ReadOnlyCollection<EntityRootViewModel>(_entityRoots);
            LayerRoots = new ReadOnlyCollection<LayerRootViewModel>(_layerRoots);
        }

        public IRearrange Rearranger { get; set; }
        public ILayerProvider LayerProvider { get; set; }
        public LayerTypes LayerType { get; set; }
        public ReadOnlyCollection<EntityRootViewModel> EntityRoots { get; set; }
        public ReadOnlyCollection<LayerRootViewModel> LayerRoots { get; set; }

        public void OnLayerTypeChanged()
        {
            _layerRoots.Clear();
            _layerRoots.Add(new LayerRootViewModel(LayerType, LayerProvider, Rearranger));

            LayerRoots = new ReadOnlyCollection<LayerRootViewModel>(_layerRoots);
        }

        public void UpdateFromModel(MapComponent mapComponent, IEntityFactory entityFactory)
        {
            _entityRoots.Clear();
            _layerRoots.Clear();

            _entityRoots.Add(new EntityRootViewModel(mapComponent, entityFactory));
            _layerRoots.Add(new LayerRootViewModel(LayerType, LayerProvider, Rearranger));

            EntityRoots = new ReadOnlyCollection<EntityRootViewModel>(_entityRoots);
            LayerRoots = new ReadOnlyCollection<LayerRootViewModel>(_layerRoots);

            IsActive = true;
        }
    }
}