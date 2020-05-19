using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public class EntityRootViewModel : ViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<EntityTypeViewModel> Children { get; set; }

        public EntityRootViewModel(MapComponent mapComponent, IEntityFactory entityFactory)
        {
            Name = mapComponent.Name;

            Children = new ReadOnlyCollection<EntityTypeViewModel>(new List<EntityTypeViewModel>()
            {
                new EntityTypeViewModel("Actors", mapComponent.GetMapActorEntityIDs(), entityFactory),
                new EntityTypeViewModel("Brushes", mapComponent.GetMapBrushEntityIDs(), entityFactory),
                new EntityTypeViewModel("Volumes", mapComponent.GetMapVolumeEntityIDs(), entityFactory),
                new EntityTypeViewModel("Lights", mapComponent.GetMapLightEntityIDs(), entityFactory)
            });
        }
    }
}