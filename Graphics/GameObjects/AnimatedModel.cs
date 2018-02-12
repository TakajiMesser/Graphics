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
using Graphics.Rendering.Animations;
using Graphics.Rendering.Vertices;
using OpenTK.Graphics;

namespace Graphics.GameObjects
{
    public class AnimatedModel : Model
    {
        public List<Mesh<Vertex>> Meshes { get; private set; } = new List<Mesh<Vertex>>();
        public override List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();
        public Joint RootJoint { get; set; }

        public override void Load(ShaderProgram program) => Meshes.ForEach(m => m.Load(program));
        public override void ClearLights() => Meshes.ForEach(m => m.ClearLights());
        public override void AddPointLights(IEnumerable<PointLight> lights) => Meshes.ForEach(m => m.AddPointLights(lights));

        public override void AddTestColors()
        {
            /*var vertices = new List<JointVertex>();

            for (var i = 0; i < Mesh.Vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Lime));
                }
                else if (i % 3 == 1)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Red));
                }
                else if (i % 3 == 2)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Blue));
                }
            }

            Mesh.ClearVertices();
            Mesh.AddVertices(vertices);
            Mesh.RefreshVertices();*/
        }

        public override void Draw(ShaderProgram program)
        {
            _modelMatrix.Set(program);
            program.SetUniform("jointTransforms", new Matrix4[32]);
            Meshes.ForEach(m => m.Draw(program));
        }

        public new static AnimatedModel LoadFromFile(string filePath)
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath);

                // For now, assume every file has just one animation
                var animation = scene.Animations.First();
                var mesh = scene.Meshes.First();

                //mesh.
            }
            

            return new AnimatedModel()
            {
                //Mesh = Mesh<JointVertex>.LoadFromFile(file)
            };
        }
    }
}
