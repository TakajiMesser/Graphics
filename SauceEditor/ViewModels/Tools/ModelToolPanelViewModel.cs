using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;
using SpiceEngine.Entities.Layers;

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

        public ILayerSetter LayerSetter { get; set; }
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
                    LayerSetter.ClearLayer(SHAPE_LAYER_NAME);
                    LayerSetter.AddToLayer(SHAPE_LAYER_NAME, ModelComponent.GetShapeEntities());
                    LayerSetter.EnableLayer(SHAPE_LAYER_NAME);
                    break;
                case ModelToolTypes.Face:
                    LayerSetter.ClearLayer(FACE_LAYER_NAME);
                    LayerSetter.AddToLayer(FACE_LAYER_NAME, ModelComponent.GetFaceEntities());
                    LayerSetter.EnableLayer(FACE_LAYER_NAME);
                    break;
                case ModelToolTypes.Triangle:
                    LayerSetter.ClearLayer(TRIANGLE_LAYER_NAME);
                    LayerSetter.AddToLayer(TRIANGLE_LAYER_NAME, ModelComponent.GetTriangleEntities());
                    LayerSetter.EnableLayer(TRIANGLE_LAYER_NAME);
                    break;
                case ModelToolTypes.Vertex:
                    LayerSetter.ClearLayer(VERTEX_LAYER_NAME);
                    LayerSetter.AddToLayer(VERTEX_LAYER_NAME, ModelComponent.GetVertexEntities());
                    LayerSetter.EnableLayer(VERTEX_LAYER_NAME);
                    break;
            }
        }

        private void DisableLayers(ModelToolTypes excludeToolType)
        {
            LayerSetter.DisableLayer(LayerManager.ROOT_LAYER_NAME);

            if (excludeToolType != ModelToolTypes.Shape) LayerSetter.DisableLayer(SHAPE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Face) LayerSetter.DisableLayer(FACE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Triangle) LayerSetter.DisableLayer(TRIANGLE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Vertex) LayerSetter.DisableLayer(VERTEX_LAYER_NAME);
        }
    }
}