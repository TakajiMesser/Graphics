using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngineCore.Entities;
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
                new EntityTypeViewModel("", mapComponent.GetMapEntityIDs(), entityFactory)
                /*new EntityTypeViewModel<ILight>(mapComponent.GetLights(), entityFactory),
                new EntityTypeViewModel<Brush>(mapComponent.GetBrushes(), entityFactory),
                new EntityTypeViewModel<IActor>(mapComponent.GetActors(), entityFactory),
                new EntityTypeViewModel<Volume>(mapComponent.GetVolumes(), entityFactory)*/
            });
        }
    }
}