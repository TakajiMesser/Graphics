﻿using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities.Lights
{
    public class SpotLight : Light<SLight>, IRotate
    {
        private float _radius = 1.0f;
        private float _height = 1.0f;

        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                _modelMatrix.Scale = new Vector3(_radius, _radius, _height);
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                _modelMatrix.Scale = new Vector3(_radius, _radius, _height);
            }
        }

        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }

        public Vector3 Direction => (new Vector4(0.0f, 0.0f, -Height, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;
        public float CutoffAngle
        {
            get
            {
                var cutoffVector = new Vector2(Radius, Height);
                return Vector2.Dot(new Vector2(0, Height), cutoffVector) / (Height * cutoffVector.Length);
            }
        }

        public Matrix4 View => Matrix4.LookAt(Position, Position + Direction.Normalized(), Vector3.UnitZ);
        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView((float)Math.Atan2(_radius, Height) * 2.0f, 1.0f, 0.1f, Height);

        public void Rotate(Quaternion rotation) => _modelMatrix.Rotation = rotation * _modelMatrix.Rotation;

        public override SLight ToStruct() => new SLight(Position, Radius, Color.Xyz, Intensity);
    }
}