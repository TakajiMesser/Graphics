using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public class EntityTreePanelViewModel : DockViewModel
    {
        private List<EntityRootViewModel> _roots = new List<EntityRootViewModel>();

        public EntityTreePanelViewModel() : base(DockTypes.Property) => Roots = new ReadOnlyCollection<EntityRootViewModel>(_roots);

        public ReadOnlyCollection<EntityRootViewModel> Roots { get; set; }

        public void UpdateFromModel(MapComponent mapComponent, IEntityFactory entityFactory)
        {
            _roots.Add(new EntityRootViewModel(mapComponent, entityFactory));
            Roots = new ReadOnlyCollection<EntityRootViewModel>(_roots);
        }
    }
}