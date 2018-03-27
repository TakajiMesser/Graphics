using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TakoEngine.Entities.Cameras;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Meshes;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Rendering.Processing
{
    public class SkyboxRenderer : Renderer
    {
        public Texture SkyTexture { get; protected set; }
        
        internal ShaderProgram _program;
        private SimpleMesh _cubeMesh;
        private List<string> _texturePaths = new List<string>();

        public void SetTextures(IEnumerable<string> texturePath)
        {
            _texturePaths.Clear();
            _texturePaths.AddRange(texturePath);
        }

        protected override void LoadPrograms()
        {
            _program = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SKYBOX_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SKYBOX_FRAGMENT_PATH))
            );
        }

        protected override void LoadTextures(Resolution resolution)
        {
            if (_texturePaths.Any())
            {
                SkyTexture = Texture.LoadFromFile(_texturePaths, TextureTarget.TextureCubeMap, true, true);
            }
        }

        public override void ResizeTextures(Resolution resolution) { }

        protected override void LoadBuffers()
        {
            _cubeMesh = SimpleMesh.LoadFromFile(FilePathHelper.CUBE_MESH_PATH, _program);
        }

        public void Render(Camera camera)
        {
            if (SkyTexture != null)
            {
                _program.Use();

                int oldCullFaceMode = GL.GetInteger(GetPName.CullFaceMode);
                int oldDepthFunc = GL.GetInteger(GetPName.DepthFunc);

                GL.Enable(EnableCap.DepthTest);
                GL.CullFace(CullFaceMode.Front);
                GL.DepthFunc(DepthFunction.Lequal);

                _program.BindTexture(SkyTexture, "mainTexture", 0);

                camera.Draw(_program);
                _program.SetUniform(ModelMatrix.NAME, Matrix4.CreateTranslation(camera.Position));
                _cubeMesh.Draw();

                GL.CullFace((CullFaceMode)oldCullFaceMode);
                GL.DepthFunc((DepthFunction)oldDepthFunc);
            }
        }
    }
}
