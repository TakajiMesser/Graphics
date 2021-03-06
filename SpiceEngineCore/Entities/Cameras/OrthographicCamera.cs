﻿using OpenTK;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngineCore.Entities.Cameras
{
    public class OrthographicCamera : Camera
    {
        public const float ZNEAR = -100.0f;
        public const float ZFAR = 100.0f;

        public float Width
        {
            get => _projectionMatrix.Width;
            set => _projectionMatrix.Width = value;
        }

        public OrthographicCamera(string name, float zNear, float zFar, float startingWidth) : base(name, ProjectionTypes.Orthographic)
        {
            _projectionMatrix.UpdateOrthographic(startingWidth, zNear, zFar);
        }

        public Matrix4 CalculateProjection()
        {
            var width = 0.8f;
            var height = width / _projectionMatrix.AspectRatio;
            return Matrix4.CreateOrthographic(width, height, _projectionMatrix.ZNear, _projectionMatrix.ZFar);
        }
    }
}
