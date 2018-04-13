﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TakoEngine.Entities.Cameras;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Meshes;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Utilities;

namespace TakoEngine.Rendering.Processing
{
    public class SkyboxRenderer : Renderer
    {
        public Texture SkyTexture2D { get; protected set; }
        public Texture SkyTexture { get; protected set; }
        
        internal ShaderProgram _program;
        private ShaderProgram _2DProgram;

        private SimpleMesh _cubeMesh;
        private SimpleMesh _squareMesh;
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

            _2DProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SKYBOX_2D_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SKYBOX_2D_FRAGMENT_PATH))
            );
        }

        protected override void LoadTextures(Resolution resolution)
        {
            if (_texturePaths.Any())
            {
                SkyTexture = Texture.LoadFromFile(_texturePaths, TextureTarget.TextureCubeMap, true, true);
                SkyTexture2D = Texture.LoadFromFile(_texturePaths.First(), true, true);
            }
        }

        public override void ResizeTextures(Resolution resolution) { }

        protected override void LoadBuffers()
        {
            _cubeMesh = SimpleMesh.LoadFromFile(FilePathHelper.CUBE_MESH_PATH, _program);
            _squareMesh = SimpleMesh.LoadFromFile(FilePathHelper.SQUARE_MESH_PATH, _2DProgram);
        }

        public void Render(Camera camera)
        {
            switch (camera)
            {
                case PerspectiveCamera p:
                    Render(p);
                    break;
                case OrthographicCamera o:
                    Render(o);
                    break;
            }
        }

        private void Render(PerspectiveCamera camera)
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

        private void Render(OrthographicCamera camera)
        {
            if (SkyTexture2D != null)
            {
                _2DProgram.Use();

                GL.Enable(EnableCap.DepthTest);
                //GL.CullFace(CullFaceMode.Front);
                GL.DepthFunc(DepthFunction.Lequal);

                _2DProgram.BindTexture(SkyTexture2D, "mainTexture", 0);

                //camera.Draw(_2DProgram);
                camera._viewMatrix.Set(_2DProgram);
                var projection = Matrix4.CreateOrthographic(1.0f, 1.0f, camera._projectionMatrix.ZNear, camera._projectionMatrix.ZFar);
                _2DProgram.SetUniform(ProjectionMatrix.NAME, projection);

                var originalDirection = new Vector3(0.0f, 0.0f, -1.0f);
                var direction = camera._viewMatrix.LookAt - camera._viewMatrix.Translation;

                var cosTheta = Vector3.Dot(originalDirection, direction);
                var rotationAxis = Vector3.Cross(originalDirection, direction);
                var rotation = (rotationAxis == Vector3.Zero)
                    ? Quaternion.Identity
                    : Quaternion.FromAxisAngle(rotationAxis, UnitConversions.ToDegrees((float)Math.Acos(cosTheta)));

                _2DProgram.SetUniform(ModelMatrix.NAME, Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(camera.Position));
                _squareMesh.Draw();
            }
        }
    }
}