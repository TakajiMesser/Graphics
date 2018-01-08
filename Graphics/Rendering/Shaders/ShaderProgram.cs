using Graphics.Rendering.Textures;
using Graphics.Rendering.Vertices;
using OpenTK.Graphics.OpenGL;

namespace Graphics.Rendering.Shaders
{
    public class ShaderProgram
    {
        private readonly int _handle;

        public ShaderProgram(params Shader[] shaders)
        {
            _handle = GL.CreateProgram();

            foreach (var shader in shaders)
            {
                GL.AttachShader(_handle, shader._handle);
            }

            GL.LinkProgram(_handle);

            foreach (var shader in shaders)
            {
                GL.DetachShader(_handle, shader._handle);
            }
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(_handle, name);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(_handle, name);
        }

        public void BindUniformBlock(string name, int binding)
        {
            int blockIndex = GL.GetUniformBlockIndex(_handle, name);
            GL.UniformBlockBinding(_handle, blockIndex, binding);
        }

        public void BindShaderStorageBlock(string name, int binding)
        {
            int blockIndex = 0;// GL.ShaderSto;
            GL.ShaderStorageBlockBinding(_handle, blockIndex, binding);
        }

        public void BindTexture(Texture texture, string name, int index)
        {
            int location = GetUniformLocation(name);

            GL.ActiveTexture(TextureUnit.Texture0 + index);
            texture.Bind();
            GL.Uniform1(location, index);
        }

        public void BindImageTexture(Texture texture, string name, int index)
        {
            int location = GetUniformLocation(name);

            GL.ActiveTexture(TextureUnit.Texture0 + index);
            texture.BindImageTexture(index);
            GL.Uniform1(location, index);
        }

        public int GetVertexAttributeLocation(string name)
        {
            int index = GetAttributeLocation(name);
            if (index == -1)
            {
                // Note that any attributes not explicitly used in the shaders will be optimized out by the shader compiler, resulting in (index == -1)
                //throw new ArgumentOutOfRangeException(_name + " not found in program attributes");
            }

            return index;
        }
    }
}
