using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;

namespace SauceEditor.ViewModels.Tools
{
    public enum ModelToolTypes
    {
        Shape,
        Face,
        Triangle,
        Vertex,
        Texture
    }

    public class ModelToolPanelViewModel : DockViewModel
    {
        public ModelToolPanelViewModel() : base(DockTypes.Tool) { }

        public ISelectEntities EntitySelector { get; set; }
        public ModelComponent ModelComponent { get; set; }
        public ModelToolTypes ModelToolType { get; set; }

        public void OnModelToolTypeChanged()
        {
            switch (ModelToolType)
            {
                case ModelToolTypes.Shape:
                    EntitySelector.SetSelectableEntities("Shape", ModelComponent.GetShapeEntities());
                    break;
                case ModelToolTypes.Face:
                    EntitySelector.SetSelectableEntities("Face", ModelComponent.GetFaceEntities());
                    break;
                case ModelToolTypes.Triangle:
                    EntitySelector.SetSelectableEntities("Triangle", ModelComponent.GetTriangleEntities());
                    break;
                case ModelToolTypes.Vertex:
                    EntitySelector.SetSelectableEntities("Vertex", ModelComponent.GetVertexEntities());
                    break;
            }
        }
    }
}