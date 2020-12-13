﻿using SpiceEngineCore.Entities.Cameras;
using TangyHIDCore;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngine.Helpers
{
    public static class GeometryHelper
    {
        public static Vector3 GetHeldTranslation(ICamera camera, float speed, IInputProvider inputProvider)
        {
            Vector3 translation = new Vector3();

            // Project the "Up" vector of the camera's view onto the XY plane, since that is what we restrict our movement translation to
            // TODO - This is mad suspect...
            var flattenedUp = camera is Camera cameraInstance ? cameraInstance.Up.Xy : Vector2.One;
            var up = new Vector3(flattenedUp.X, flattenedUp.Y, 0.0f);
            var right = new Vector3(flattenedUp.Y, -flattenedUp.X, 0.0f);

            if (inputProvider.IsDown(inputProvider.InputMapping.Forward))
            {
                translation += up.Normalized() * speed;
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Left))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Backward))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Right))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }

        public static Vector3 GetPressedTranslation(ICamera camera, float speed, IInputProvider inputProvider)
        {
            Vector3 translation = new Vector3();

            // Project the "Up" vector of the camera's view onto the XY plane, since that is what we restrict our movement translation to
            // TODO - This is mad suspect...
            var flattenedUp = camera is Camera cameraInstance ? cameraInstance.Up.Xy : Vector2.One;
            var up = new Vector3(flattenedUp.X, flattenedUp.Y, 0.0f);
            var right = new Vector3(flattenedUp.Y, -flattenedUp.X, 0.0f);

            if (inputProvider.IsDown(inputProvider.InputMapping.Forward))
            {
                translation += up.Normalized() * speed;
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Left))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Backward))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Right))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }
    }
}
