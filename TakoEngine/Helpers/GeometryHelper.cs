using OpenTK;
using TakoEngine.Entities.Cameras;
using TakoEngine.Inputs;

namespace TakoEngine.Helpers
{
    public static class GeometryHelper
    {
        public static Vector3 GetTranslation(Camera camera, float speed, InputState inputState, InputMapping inputMapping)
        {
            Vector3 translation = new Vector3();

            // Project the "Up" vector of the camera's view onto the XY plane, since that is what we restrict our movement translation to
            var flattenedUp = camera._viewMatrix.Up.Xy;
            var up = new Vector3(flattenedUp.X, flattenedUp.Y, 0.0f);
            var right = new Vector3(flattenedUp.Y, -flattenedUp.X, 0.0f);

            if (inputState.IsHeld(inputMapping.Forward))
            {
                translation += up.Normalized() * speed;
            }

            if (inputState.IsHeld(inputMapping.Left))
            {
                translation -= right.Normalized() * speed;
            }

            if (inputState.IsHeld(inputMapping.Backward))
            {
                translation -= up.Normalized() * speed;
            }

            if (inputState.IsHeld(inputMapping.Right))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }
    }
}
