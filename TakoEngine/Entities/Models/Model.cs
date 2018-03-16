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
        public Quaternion OriginalRotation { get; set; } = Quaternion.Identity;

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public Quaternion Rotation
        {
            get => OriginalRotation * _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = OriginalRotation * value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public abstract void Load(ShaderProgram program);
        public abstract void Draw(ShaderProgram program, TextureManager textureManager);

        public abstract void ClearLights();
        public abstract void AddPointLights(IEnumerable<PointLight> lights);
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
