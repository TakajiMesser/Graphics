using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;

namespace SweetGraphicsCore.Shaders
{
    public class ShaderProgram : IShader
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

        public void Use() => GL.UseProgram(_handle);

        public int GetAttributeLocation(string name) => GL.GetAttribLocation(_handle, name);

        public int GetUniformLocation(string name) => GL.GetUniformLocation(_handle, name);

        public void BindUniformBlock(string name, int binding)
        {
            var blockIndex = GL.GetUniformBlockIndex(_handle, name);
            GL.UniformBlockBinding(_handle, blockIndex, binding);
        }

        public void BindShaderStorageBlock(string name, int binding)
        {
            var blockIndex = 0;// GL.ShaderSto;
            GL.ShaderStorageBlockBinding(_handle, blockIndex, binding);
        }

        public void BindTexture(ITexture texture, string name, int index)
        {
            var location = GetUniformLocation(name);

            GL.ActiveTexture(TextureUnit.Texture0 + index);
            texture.Bind();
            GL.Uniform1(location, index);
        }

        /*var diffuseMap = textureProvider.RetrieveTexture(textureMapping.DiffuseIndex);
        GL.Uniform1(GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
        if (diffuseMap != null)
        {
            BindTexture(diffuseMap, "diffuseMap", 0);
        }*/

        public void BindImageTexture(ITexture texture, string name, int index)
        {
            var location = GetUniformLocation(name);

            GL.ActiveTexture(TextureUnit.Texture0 + index);
            texture.BindImageTexture(index);
            GL.Uniform1(location, index);
        }

        public void SetUniform<T>(string name, T value) where T : struct
        {
            switch (value)
            {
                case Matrix4 matrix4:
                    SetUniform(name, matrix4);
                    break;
                case Matrix4[] matrices:
                    SetUniform(name, matrices);
                    break;
                case Vector2 vector2:
                    SetUniform(name, vector2);
                    break;
                case Vector3 vector3:
                    SetUniform(name, vector3);
                    break;
                case Vector4 vector4:
                    SetUniform(name, vector4);
                    break;
                case Color4 color4:
                    SetUniform(name, color4);
                    break;
                case float floatValue:
                    SetUniform(name, floatValue);
                    break;
                case int intValue:
                    SetUniform(name, intValue);
                    break;
            }
        }

        public void SetUniform(string name, Matrix4 matrix)
        {
            var location = GetUniformLocation(name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetUniform(string name, Matrix4[] matrices)
        {
            for (var i = 0; i < matrices.Length; i++)
            {
                var iLocation = GetUniformLocation(name + "[" + i + "]");
                GL.UniformMatrix4(iLocation, false, ref matrices[i]);
            }

            /*int location = GetUniformLocation(name);
            float[] values = new float[16 * matrices.Length];

            for (var i = 0; i < matrices.Length; i++)
            {
                var columns = new Vector4[]
                {
                    matrices[i].Column0,
                    matrices[i].Column1,
                    matrices[i].Column2,
                    matrices[i].Column3
                };

                for (var j = 0; j < 4; j++)
                {
                    values[i * 16 + j * 4] = columns[j].X;
                    values[i * 16 + j * 4 + 1] = columns[j].Y;
                    values[i * 16 + j * 4 + 2] = columns[j].Z;
                    values[i * 16 + j * 4 + 3] = columns[j].W;
                }
            }

            GL.UniformMatrix4(location, matrices.Length, true, values);*/
        }

        public void SetUniform(string name, Vector2 vector)
        {
            var location = GetUniformLocation(name);
            GL.Uniform2(location, vector);
        }

        public void SetUniform(string name, Vector3 vector)
        {
            var location = GetUniformLocation(name);
            GL.Uniform3(location, vector);
        }

        public void SetUniform(string name, Vector4 vector)
        {
            var location = GetUniformLocation(name);
            GL.Uniform4(location, vector);
        }

        public void SetUniform(string name, Color4 color)
        {
            var location = GetUniformLocation(name);
            GL.Uniform4(location, color);
        }

        public void SetUniform(string name, float value)
        {
            var location = GetUniformLocation(name);
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, int value)
        {
            var location = GetUniformLocation(name);
            GL.Uniform1(location, value);
        }

        public void SetMaterial(Material material)
        {
            SetUniform("ambientColor", material.Ambient);
            SetUniform("diffuseColor", material.Diffuse);
            SetUniform("specularColor", material.Specular);
            SetUniform("specularExponent", material.SpecularExponent);
        }

        public void SetCamera(ICamera camera)
        {
            SetUniform(ViewMatrix.CURRENT_NAME, camera.CurrentModelMatrix);
            SetUniform(ViewMatrix.PREVIOUS_NAME, camera.PreviousModelMatrix);
            SetUniform(ProjectionMatrix.CURRENT_NAME, camera.CurrentProjectionMatrix);
            SetUniform(ProjectionMatrix.PREVIOUS_NAME, camera.PreviousProjectionMatrix);
        }

        public void SetLight(ILight light)
        {
            SetUniform(ModelMatrix.CURRENT_NAME, light.CurrentModelMatrix);
            SetUniform(ModelMatrix.PREVIOUS_NAME, light.PreviousModelMatrix);
        }

        public void SetLightView(ILight light)
        {
            if (light is PointLight pointLight)
            {
                var shadowViews = new List<Matrix4>();

                for (var i = 0; i < 6; i++)
                {
                    shadowViews.Add(pointLight.GetView(TextureTarget.TextureCubeMapPositiveX + i) * pointLight.Projection);
                }

                SetUniform(ViewMatrix.SHADOW_NAME, shadowViews.ToArray());
            }
            else if (light is SpotLight spotLight)
            {
                SetUniform(ProjectionMatrix.CURRENT_NAME, spotLight.Projection);
                SetUniform(ViewMatrix.CURRENT_NAME, spotLight.View);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void StencilPass(ILight light)
        {
            if (light is PointLight pointLight)
            {
                SetUniform("lightPosition", pointLight.Position);
                SetUniform("lightRadius", pointLight.Radius);
                SetUniform("lightColor", pointLight.Color);
                SetUniform("lightIntensity", pointLight.Intensity);
            }
            else if (light is SpotLight spotLight)
            {
                SetUniform("lightPosition", spotLight.Position);
                SetUniform("lightRadius", spotLight.Radius);
                SetUniform("lightColor", spotLight.Color);
                SetUniform("lightIntensity", spotLight.Intensity);
                SetUniform("lightHeight", spotLight.Height);
                SetUniform("lightVector", spotLight.Direction);
                SetUniform("lightCutoffAngle", spotLight.CutoffAngle);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void LightPass(ILight light)
        {
            if (light is PointLight pointLight)
            {
                SetUniform("lightPosition", pointLight.Position);
                SetUniform("lightRadius", pointLight.Radius);
                SetUniform("lightColor", pointLight.Color.Xyz);
                SetUniform("lightIntensity", pointLight.Intensity);
            }
            else if (light is SpotLight spotLight)
            {
                SetUniform("lightPosition", spotLight.Position);
                SetUniform("lightRadius", spotLight.Radius);
                SetUniform("lightColor", spotLight.Color.Xyz);
                SetUniform("lightIntensity", spotLight.Intensity);
                SetUniform("lightHeight", spotLight.Height);
                SetUniform("lightVector", spotLight.Direction);
                SetUniform("lightCutoffAngle", spotLight.CutoffAngle);
                SetUniform("lightMatrix", spotLight.View * spotLight.Projection);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public int GetVertexAttributeLocation(string name)
        {
            var index = GetAttributeLocation(name);
            if (index == -1)
            {
                // Note that any attributes not explicitly used in the shaders will be optimized out by the shader compiler, resulting in (index == -1)
                //throw new ArgumentOutOfRangeException(_name + " not found in program attributes");
            }

            return index;
        }

        public void BindTextures(ITextureProvider textureProvider, TextureMapping textureMapping)
        {
            // TODO - Order brush rendering in a way that allows us to not re-bind duplicate textures repeatedly
            // Check brush's texture mapping to see which textures we need to bind
            var diffuseMap = textureProvider.RetrieveTexture(textureMapping.DiffuseIndex);
            GL.Uniform1(GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
            if (diffuseMap != null)
            {
                BindTexture(diffuseMap, "diffuseMap", 0);
            }

            var normalMap = textureProvider.RetrieveTexture(textureMapping.NormalIndex);
            GL.Uniform1(GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
            if (normalMap != null)
            {
                BindTexture(normalMap, "normalMap", 1);
            }

            var specularMap = textureProvider.RetrieveTexture(textureMapping.SpecularIndex);
            GL.Uniform1(GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
            if (specularMap != null)
            {
                BindTexture(specularMap, "specularMap", 2);
            }

            var parallaxMap = textureProvider.RetrieveTexture(textureMapping.ParallaxIndex);
            GL.Uniform1(GetUniformLocation("useParallaxMap"), (parallaxMap != null) ? 1 : 0);
            if (parallaxMap != null)
            {
                BindTexture(parallaxMap, "parallaxMap", 3);
            }
        }

        public void UnbindTextures()
        {
            GL.Uniform1(GetUniformLocation("useDiffuseMap"), 0);
            GL.Uniform1(GetUniformLocation("useNormalMap"), 0);
            GL.Uniform1(GetUniformLocation("useSpecularMap"), 0);
            GL.Uniform1(GetUniformLocation("useParallaxMap"), 0);
        }
    }
}
