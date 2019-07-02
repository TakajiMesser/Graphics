using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;
using SpiceEngine.Entities;

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
        private const string SHAPE_LAYER_NAME = "Shape";
        private const string FACE_LAYER_NAME = "Face";
        private const string TRIANGLE_LAYER_NAME = "Triangle";
        private const string VERTEX_LAYER_NAME = "Vertex";

        public ModelToolPanelViewModel() : base(DockTypes.Tool) { }

        public ISelectEntities EntitySelector { get; set; }
        public ModelComponent ModelComponent { get; set; }
        public ModelToolTypes ModelToolType { get; set; }

        public void OnModelToolTypeChanged()
        {
            EnableLayer(ModelToolType);
            DisableLayers(ModelToolType);
        }

        private void EnableLayer(ModelToolTypes enableToolType)
        {
            switch (enableToolType)
            {
                case ModelToolTypes.Shape:
                    EntitySelector.EnableLayer(SHAPE_LAYER_NAME, ModelComponent.GetShapeEntities());
                    break;
                case ModelToolTypes.Face:
                    EntitySelector.EnableLayer(FACE_LAYER_NAME, ModelComponent.GetFaceEntities());
                    break;
                case ModelToolTypes.Triangle:
                    EntitySelector.EnableLayer(TRIANGLE_LAYER_NAME, ModelComponent.GetTriangleEntities());
                    break;
                case ModelToolTypes.Vertex:
                    EntitySelector.EnableLayer(VERTEX_LAYER_NAME, ModelComponent.GetVertexEntities());
                    break;
            }
        }

        private void DisableLayers(ModelToolTypes excludeToolType)
        {
            EntitySelector.DisableLayer(LayerManager.ROOT_LAYER_NAME);

            if (excludeToolType != ModelToolTypes.Shape) EntitySelector.DisableLayer(SHAPE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Face) EntitySelector.DisableLayer(FACE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Triangle) EntitySelector.DisableLayer(TRIANGLE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Vertex) EntitySelector.DisableLayer(VERTEX_LAYER_NAME);
        }
    }
}