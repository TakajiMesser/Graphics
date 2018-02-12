using Graphics.Inputs;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Matrices;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Graphics.Lighting;
using Graphics.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Vertices;
using OpenTK.Graphics;
using System.IO;

namespace Graphics.GameObjects
{
    public abstract class Model
    {
        internal ModelMatrix _modelMatrix = new ModelMatrix();

        public abstract List<Vector3> Vertices { get; }

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        //public void ClearLights() => Mesh.ClearLights();
        //public void AddPointLights(IEnumerable<PointLight> lights) => Mesh.AddPointLights(lights);

        public abstract void Load(ShaderProgram program);
        public abstract void ClearLights();
        public abstract void AddPointLights(IEnumerable<PointLight> lights);
        public abstract void AddTestColors();
        public abstract void Draw(ShaderProgram program);

        public static Model LoadFromFile(string filePath)
        {
            switch (Path.GetExtension(filePath))
            {
                case ".obj":
                    return SimpleModel.LoadFromFile(filePath);
                case ".dae":
                    return AnimatedModel.LoadFromFile(filePath);
                default:
                    throw new NotImplementedException("Could not handle file type " + filePath);
            }
        }

        // Define how this object's state will be saved, if desired
        //public override void OnSaveState() { }
    }
}
