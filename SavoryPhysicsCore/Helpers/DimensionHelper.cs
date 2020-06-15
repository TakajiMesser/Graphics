using OpenTK;

namespace SavoryPhysicsCore.Helpers
{
    public static class DimensionHelper
    {
        public static Vector3 Flattened(this Vector3 vector) => new Vector3()
        {
            X = vector.X,
            Y = vector.Y,
            Z = 0.0f
        };

        public static Vector3 ToVector3(this Vector2 vector) => new Vector3()
        {
            X = vector.X,
            Y = vector.Y,
            Z = 0.0f
        };
    }
}
