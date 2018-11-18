using OpenTK;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Inputs;

namespace SpiceEngine.Helpers
{
    public static class GeometryHelper
    {
        public static Vector3 GetHeldTranslation(Camera camera, float speed, InputManager inputManager, InputBinding inputMapping)
        {
            Vector3 translation = new Vector3();

            // Project the "Up" vector of the camera's view onto the XY plane, since that is what we restrict our movement translation to
            var flattenedUp = camera._viewMatrix.Up.Xy;
            var up = new Vector3(flattenedUp.X, flattenedUp.Y, 0.0f);
            var right = new Vector3(flattenedUp.Y, -flattenedUp.X, 0.0f);

            if (inputManager.IsDown(inputMapping.Forward))
            {
                translation += up.Normalized() * speed;
            }

            if (inputManager.IsDown(inputMapping.Left))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputManager.IsDown(inputMapping.Backward))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputManager.IsDown(inputMapping.Right))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }

        public static Vector3 GetPressedTranslation(Camera camera, float speed, InputManager inputManager, InputBinding inputMapping)
        {
            Vector3 translation = new Vector3();

            // Project the "Up" vector of the camera's view onto the XY plane, since that is what we restrict our movement translation to
            var flattenedUp = camera._viewMatrix.Up.Xy;
            var up = new Vector3(flattenedUp.X, flattenedUp.Y, 0.0f);
            var right = new Vector3(flattenedUp.Y, -flattenedUp.X, 0.0f);

            if (inputManager.IsDown(inputMapping.Forward))
            {
                translation += up.Normalized() * speed;
            }

            if (inputManager.IsDown(inputMapping.Left))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputManager.IsDown(inputMapping.Backward))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputManager.IsDown(inputMapping.Right))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }
    }
}
