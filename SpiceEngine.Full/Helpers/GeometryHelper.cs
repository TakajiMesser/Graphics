﻿using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using TangyHIDCore;

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

            if (inputProvider.IsDown("Forward"))
            {
                translation += up.Normalized() * speed;
            }

            if (inputProvider.IsDown("Left"))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputProvider.IsDown("Backward"))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputProvider.IsDown("Right"))
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

            if (inputProvider.IsDown("Forward"))
            {
                translation += up.Normalized() * speed;
            }

            if (inputProvider.IsDown("Left"))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputProvider.IsDown("Backward"))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputProvider.IsDown("Right"))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }
    }
}
