using OpenTK;
using System.Collections.Generic;
using TakoEngine.Entities.Lights;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Entities.Models
{
    public abstract class Model
    {
        internal ModelMatrix _modelMatrix = new ModelMatrix();

        public abstract List<Vector3> Vertices { get; }

        /// <summary>
        /// All models are assumed to have their "forward" direction in the positive X direction.
        /// If the model is oriented in a different direction, this quaternion should orient it from the assumed direction to the correct one.
        /// If the model is already oriented correctly, this should be the quaternion identity.
        /// </summary>
        public Quaternion Orientation { get; set; } = Quaternion.Identity;

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public Quaternion Rotation
        {
            get => Orientation * _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = Orientation * value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public abstract void Load();
        public abstract void Draw(ShaderProgram program, TextureManager textureManager);

        //public abstract void ClearLights();
        //public abstract void AddPointLights(IEnumerable<PointLight> lights);
        public abstract void AddTestColors();

        public static Model LoadFromFile(string filePath, TextureManager textureManager)
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                return scene.HasAnimations
                    ? (Model)new AnimatedModel(filePath, scene, textureManager)
                    : new SimpleModel(scene);
            }
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }
    }
}
