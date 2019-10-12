using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public interface IDisplayEntities
    {
        void UpdateFromModel(MapComponent mapComponent, IEntityFactory entityFactory);
    }
}