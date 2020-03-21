using OpenTK;
using SpiceEngine.Maps;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;
using TowerWarfare.Helpers;

namespace TowerWarfare.Entities.Cameras
{
    public class Camera : MapCamera
    {
        public const string NAME = "MainCamera";

        public Camera()
        {
            Name = NAME;
            //AttachedEntityName = "Player";
            Position = new Vector3(0.0f, 0.0f, 20.0f);
            Rotation = new Vector3();
            Type = ProjectionTypes.Perspective;
            ZNear = 0.1f;
            ZFar = 1000.0f;
            FieldOfViewY = UnitConversions.ToRadians(45.0f);
            Behavior = MapBehavior.Load(FilePathHelper.CAMERA_BEHAVIOR_PATH);
        }

        /*Camera = new PerspectiveCamera("", 0.1f, 1000.0f, UnitConversions.ToRadians(45.0f));
        Camera.DetachFromEntity();
        Camera.Position = new Vector3(0.0f, -10.0f, 10.0f);
        Camera._viewMatrix.Up = Vector3.UnitZ;
        Camera._viewMatrix.LookAt = Camera.Position + Vector3.UnitY;
        _yaw = MathExtensions.HALF_PI;
        _pitch = 0.0f;*/
    }
}
