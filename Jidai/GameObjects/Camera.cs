using OpenTK;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Utilities;

namespace Jidai.GameObjects
{
    public class Camera : MapCamera
    {
        public const string NAME = "MainCamera";

        public Camera()
        {
            Name = NAME;
            AttachedActorName = "Player";
            Position = new Vector3(0.0f, 0.0f, 20.0f);
            Type = ProjectionTypes.Perspective;
            ZNear = 0.1f;
            ZFar = 1000.0f;
            FieldOfViewY = UnitConversions.ToRadians(45.0f);
        }
    }
}
