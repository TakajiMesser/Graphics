using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Entities.Layers;

namespace SauceEditor.ViewModels.Trees.Entities
{
    public interface IDisplayEntities
    {
        void UpdateFromModel(MapComponent mapComponent, IEntityFactory entityFactory);
    }
}