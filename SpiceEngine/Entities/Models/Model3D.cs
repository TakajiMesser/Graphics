using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Rendering.Meshes;
using System.Linq;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Utilities;

namespace SpiceEngine.Entities.Models
{
    /*public class Model3D<T> : IModel3D where T : IVertex3D
    {
        internal ModelMatrix _modelMatrix = new ModelMatrix();

        public Matrix4 ModelMatrix => _modelMatrix.Matrix;

        public Model3D() { }

        public List<Mesh3D<T>> Meshes { get; private set; } = new List<Mesh3D<T>>();
        public virtual List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();

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

        private Vector3 _originalRotation;
        public Vector3 OriginalRotation
        {
            get => _originalRotation;
            set
            {
                _originalRotation = value;
                _modelMatrix.Rotation = Quaternion.FromEulerAngles(value);
            }
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public virtual void Load()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Load();
            }
        }

        public virtual void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }
        }

        public virtual void SetUniforms(ShaderProgram program)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.SetUniforms(program);
            }
        }

        public virtual void SetUniformsAndDraw(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.SetUniforms(program, textureManager);
                mesh.Draw();
            }
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }

        public static IModel3D LoadFromFile(string filePath, TextureManager textureManager = null)
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
                    ? new AnimatedModel3D(filePath, scene, textureManager)
                    : LoadFromScene(scene);
            }
        }

        public static IModel3D LoadFromScene(Assimp.Scene scene)
        {
            var model = new Model3D<Vertex3D>();

            foreach (var mesh in scene.Meshes)
            {
                var material = new Material(scene.Materials[mesh.MaterialIndex]);
                var vertices = new List<Vertex3D>();

                for (var i = 0; i < mesh.VertexCount; i++)
                {
                    var position = mesh.Vertices[i].ToVector3();
                    var normals = mesh.HasNormals ? mesh.Normals[i].ToVector3() : new Vector3();
                    var tangents = mesh.HasTangentBasis ? mesh.Tangents[i].ToVector3() : new Vector3();
                    var textureCoords = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i].ToVector2() : new Vector2();

                    vertices.Add(new Vertex3D(position, normals, tangents, textureCoords));
                }

                var mesh3D = new Mesh3D<Vertex3D>(vertices, material, mesh.GetIndices().ToList());
                model.Meshes.Add(mesh3D);
            }

            return model;
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }
    }*/
}
