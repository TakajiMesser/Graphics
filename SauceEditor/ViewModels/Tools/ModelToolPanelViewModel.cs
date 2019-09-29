using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Layers;
using SpiceEngineCore.Game.Loading;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditor.ViewModels.Tools
{
    public enum ModelToolTypes
    {
        Mesh,
        Face,
        Triangle,
        Vertex
    }

    public class ModelToolPanelViewModel : DockViewModel
    {
        private const string MESH_LAYER_NAME = "Mesh";
        private const string FACE_LAYER_NAME = "Face";
        private const string TRIANGLE_LAYER_NAME = "Triangle";
        private const string VERTEX_LAYER_NAME = "Vertex";

        private List<ITexturedEntity> _texturedEntities = new List<ITexturedEntity>();
        private object _entityLock = new object();

        public ModelToolPanelViewModel() : base(DockTypes.Tool) { }

        public ILayerSetter LayerSetter { get; set; }
        public ModelComponent ModelComponent { get; set; }
        public ModelToolTypes ModelToolType { get; set; }
        public bool TextureMode { get; set; }

        public void OnModelToolTypeChanged()
        {
            EnableLayer(ModelToolType);
            DisableLayers(ModelToolType);
        }

        public void OnTextureModeChanged()
        {
            lock (_entityLock)
            {
                foreach (var texturedEntity in _texturedEntities)
                {
                    texturedEntity.IsInTextureMode = TextureMode;
                }
            }
        }

        private void SetTexturedEntities(IEnumerable<ITexturedEntity> texturedEntities)
        {
            lock (_entityLock)
            {
                _texturedEntities.Clear();
                _texturedEntities.AddRange(texturedEntities);
            }
        }

        private void EnableLayer(string layerName, IEnumerable<IEntityBuilder> entityBuilders)
        {
            LayerSetter.ClearLayer(layerName);
            LayerSetter.AddToLayer(layerName, entityBuilders);
            LayerSetter.EnableLayer(layerName);
        }

        private void EnableLayer(ModelToolTypes enableToolType)
        {
            switch (enableToolType)
            {
                case ModelToolTypes.Mesh:
                    var meshEntities = ModelComponent.GetMeshEntities().ToList();
                    EnableLayer(MESH_LAYER_NAME, meshEntities);
                    SetTexturedEntities(meshEntities);
                    break;
                case ModelToolTypes.Face:
                    var faceEntities = ModelComponent.GetFaceEntities().ToList();
                    EnableLayer(FACE_LAYER_NAME, faceEntities);
                    SetTexturedEntities(faceEntities);
                    break;
                case ModelToolTypes.Triangle:
                    var triangleEntities = ModelComponent.GetTriangleEntities().ToList();
                    EnableLayer(TRIANGLE_LAYER_NAME, triangleEntities);
                    SetTexturedEntities(triangleEntities);
                    break;
                case ModelToolTypes.Vertex:
                    EnableLayer(VERTEX_LAYER_NAME, ModelComponent.GetVertexEntities().ToList());
                    break;
            }
        }

        private void DisableLayers(ModelToolTypes excludeToolType)
        {
            LayerSetter.DisableLayer(LayerManager.ROOT_LAYER_NAME);

            if (excludeToolType != ModelToolTypes.Mesh) LayerSetter.DisableLayer(MESH_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Face) LayerSetter.DisableLayer(FACE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Triangle) LayerSetter.DisableLayer(TRIANGLE_LAYER_NAME);
            if (excludeToolType != ModelToolTypes.Vertex) LayerSetter.DisableLayer(VERTEX_LAYER_NAME);
        }
    }
}