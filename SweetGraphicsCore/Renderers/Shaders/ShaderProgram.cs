using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Geometry.Colors;
using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;

namespace SweetGraphicsCore.Renderers.Shaders
{
    public class ShaderProgram
    {
        private readonly int _handle;
        private int _textureIndex = 0;

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

        public void SetUniform(IUniform uniform)
        {
            switch (uniform)
            {
                case IUniform<Matrix4> matrix4:
                    SetUniform(uniform.Name, matrix4.Value);
                    break;
                case IUniform<Matrix4[]> matrices:
                    SetUniform(uniform.Name, matrices.Value);
                    break;
                case IUniform<Vector2> vector2:
                    SetUniform(uniform.Name, vector2.Value);
                    break;
                case IUniform<Vector3> vector3:
                    SetUniform(uniform.Name, vector3.Value);
                    break;
                case IUniform<Vector4> vector4:
                    SetUniform(uniform.Name, vector4.Value);
                    break;
                case IUniform<Color4> color4:
                    SetUniform(uniform.Name, color4.Value);
                    break;
                case IUniform<float> floatValue:
                    SetUniform(uniform.Name, floatValue.Value);
                    break;
                case IUniform<int> intValue:
                    SetUniform(uniform.Name, intValue.Value);
                    break;
            }
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
            GL.UniformMatrix4(location, 16, false, matrix.Values);
        }

        public void SetUniform(string name, Matrix4[] matrices)
        {
            for (var i = 0; i < matrices.Length; i++)
            {
                var iLocation = GetUniformLocation(name + "[" + i + "]");
                GL.UniformMatrix4(iLocation, 16, false, matrices[i].Values);
            }
        }

        public void SetUniform(string name, Vector2 vector)
        {
            var location = GetUniformLocation(name);
            GL.Uniform2(location, vector.X, vector.Y);
        }

        public void SetUniform(string name, Vector3 vector)
        {
            var location = GetUniformLocation(name);
            GL.Uniform3(location, vector.X, vector.Y, vector.Z);
        }

        public void SetUniform(string name, Vector4 vector)
        {
            var location = GetUniformLocation(name);
            GL.Uniform4(location, vector.X, vector.Y, vector.Z, vector.W);
        }

        public void SetUniform(string name, Color4 color)
        {
            var location = GetUniformLocation(name);
            GL.Uniform4(location, color.R, color.G, color.B, color.A);
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

        /*public void SetBatchUniforms(IBatcher batcher, IBatch batch, ITextureProvider textureProvider = null)
        {
            var entities = batcher.GetEntitiesForBatch(batch);
            var entity = entities.First();

            // TODO - This is janky to set this uniform based on entity type...
            if (entity is IBrush)
            {
                SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.Identity);
                SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            }
            else if (!(entity is IUIItem) && !(batch is BillboardBatch))
            {
                SetUniform(ModelMatrix.CURRENT_NAME, entity.WorldMatrix.CurrentValue);
                SetUniform(ModelMatrix.PREVIOUS_NAME, entity.WorldMatrix.PreviousValue);
            }

            SetRenderableUniforms(batch.Renderable, textureProvider);

            if (batch.Renderable is ITexturedMesh texturedMesh)
            {
                texturedMesh.Material.SetUniforms(shaderProgram);
            }

            BindTextures(textureProvider, batch.);

            switch (batch)
            {
                case MeshBatch meshBatch:
                    break;
                case ModelBatch modelBatch:
                    break;
                case BillboardBatch billboardBatch:
                    break;
                case VertexBatch vertexBatch:
                    break;
            }
        }

        public void SetRenderableUniforms(IRenderable renderable, ITextureProvider textureProvider)
        {
            if (renderable is IModel model)
            {
                
            }
            if (renderable is ITexturedMesh texturedMesh)
            {
                SetMaterial(texturedMesh.Material);

                if (texturedMesh.TextureMapping.HasValue)
                {
                    BindTextures(textureProvider, texturedMesh.TextureMapping.Value);
                }
                else
                {
                    UnbindTextures();
                }
            }
            else if (renderable is IBillboard billboard)
            {
                // TODO - Also set texture Alpha value here
                var texture = textureProvider.RetrieveTexture(billboard.TextureIndex);
                BindTexture(texture, "mainTexture", 0);
            }
        }

        public void SetRenderableTextures(IRenderable renderable, ITextureProvider textureProvider)
        {

        }*/

        public void SetMaterial(Material material)
        {
            SetUniform(Material.AMBIENT_NAME, material.Ambient);
            SetUniform(Material.DIFFUSE_NAME, material.Diffuse);
            SetUniform(Material.SPECULAR_NAME, material.Specular);
            SetUniform(Material.SPECULAR_EXPONENT_NAME, material.SpecularExponent);
        }

        public void SetCamera(ICamera camera)
        {
            SetUniform(ViewMatrix.CURRENT_NAME, camera.ViewMatrix);
            SetUniform(ViewMatrix.PREVIOUS_NAME, camera.PreviousViewMatrix);
            SetUniform(ProjectionMatrix.CURRENT_NAME, camera.ProjectionMatrix);
            SetUniform(ProjectionMatrix.PREVIOUS_NAME, camera.PreviousProjectionMatrix);
        }

        public void SetLight(ILight light)
        {
            if (light is PointLight pointLight)
            {
                var shadowViews = new List<Matrix4>();

                for (var i = 0; i < 6; i++)
                {
                    shadowViews.Add(GetView(pointLight, TextureTarget.TextureCubeMapPositiveX + i) * pointLight.Projection);
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

        public void RenderLightFromCameraPerspective(ILight light, ICamera camera)
        {
            if (light is PointLight pointLight)
            {
                var shadowViews = new List<Matrix4>();

                for (var i = 0; i < 6; i++)
                {
                    shadowViews.Add(GetView(pointLight, TextureTarget.TextureCubeMapPositiveX + i) * pointLight.Projection);
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

        public void RenderLightForStencilPass(ILight light)
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
                SetUniform("lightHeight", spotLight.Height);

                var lightVector = (new Vector4(0.0f, 0.0f, -spotLight.Height, 1.0f) * Matrix4.FromQuaternion(spotLight.Rotation)).Xyz;
                SetUniform("lightVector", lightVector);

                var cutoffVector = new Vector2(spotLight.Radius, spotLight.Height);
                var cosAngle = Vector2.Dot(new Vector2(0, spotLight.Height), cutoffVector) / (spotLight.Height * cutoffVector.Length);
                SetUniform("lightCutoffAngle", cosAngle);

                SetUniform("lightColor", spotLight.Color);
                SetUniform("lightIntensity", spotLight.Intensity);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void RenderLightForLightPass(ILight light)
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
                SetUniform("lightMatrix", spotLight.View * spotLight.Projection);

                SetUniform("lightPosition", spotLight.Position);
                SetUniform("lightRadius", spotLight.Radius);
                SetUniform("lightHeight", spotLight.Height);

                var lightVector = (new Vector4(0.0f, 0.0f, -spotLight.Height, 1.0f) * Matrix4.FromQuaternion(spotLight.Rotation)).Xyz;
                SetUniform("lightVector", lightVector);

                var cutoffVector = new Vector2(spotLight.Radius, spotLight.Height);
                var cosAngle = Vector2.Dot(new Vector2(0, spotLight.Height), cutoffVector) / (spotLight.Height * cutoffVector.Length);
                SetUniform("lightCutoffAngle", cosAngle);

                SetUniform("lightColor", spotLight.Color.Xyz);
                SetUniform("lightIntensity", spotLight.Intensity);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private Matrix4 GetView(PointLight pointLight, TextureTarget target)
        {
            switch (target)
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    return Matrix4.LookAt(pointLight.Position, pointLight.Position + Vector3.UnitX, -Vector3.UnitY);
                case TextureTarget.TextureCubeMapNegativeX:
                    return Matrix4.LookAt(pointLight.Position, pointLight.Position - Vector3.UnitX, -Vector3.UnitY);
                case TextureTarget.TextureCubeMapPositiveY:
                    return Matrix4.LookAt(pointLight.Position, pointLight.Position + Vector3.UnitY, Vector3.UnitZ);
                case TextureTarget.TextureCubeMapNegativeY:
                    return Matrix4.LookAt(pointLight.Position, pointLight.Position - Vector3.UnitY, -Vector3.UnitZ);
                case TextureTarget.TextureCubeMapPositiveZ:
                    return Matrix4.LookAt(pointLight.Position, pointLight.Position + Vector3.UnitZ, -Vector3.UnitY);
                case TextureTarget.TextureCubeMapNegativeZ:
                    return Matrix4.LookAt(pointLight.Position, pointLight.Position - Vector3.UnitZ, -Vector3.UnitY);
                default:
                    throw new NotImplementedException("Could not handle target " + target);
            }
        }

        /*public void SetUniforms(ShaderProgram program, ILight light)
        {
            if (light is PointLight pointLight)
            {
                var shadowViews = new List<Matrix4>();

                for (var i = 0; i < 6; i++)
                {
                    shadowViews.Add(pointLight.GetView(TextureTarget.TextureCubeMapPositiveX + i) * pointLight.Projection);
                }

                program.SetUniform(SpiceEngineCore.Rendering.Matrices.ViewMatrix.SHADOW_NAME, shadowViews.ToArray());
            }
            else if (light is SpotLight spotLight)
            {
                program.SetUniform(SpiceEngineCore.Rendering.Matrices.ProjectionMatrix.NAME, spotLight.Projection);
                program.SetUniform(SpiceEngineCore.Rendering.Matrices.ViewMatrix.NAME, spotLight.View);
            }
            else
            {
                throw new NotImplementedException();
            }
        }*/

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

        public void BindTextures(TextureBinding textureBinding)
        {
            if (textureBinding.Texture != null)
            {
                BindTexture(textureBinding.Texture, textureBinding.Name, _textureIndex);
                _textureIndex++;
            }
        }

        public void ClearTextures()
        {
            // TODO - Do we need to explicitly unbind textures too?
            _textureIndex = 0;
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
