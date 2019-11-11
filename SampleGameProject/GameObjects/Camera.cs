using OpenTK;
using SampleGameProject.Helpers;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;

namespace SampleGameProject.GameObjects
{
    public class Camera : MapCamera
    {
        public const string NAME = "MainCamera";

        public Camera()
        {
            Name = NAME;
            AttachedEntityName = "Player";
            Position = new Vector3(0.0f, 0.0f, 20.0f);
            Type = ProjectionTypes.Perspective;
            ZNear = 0.1f;
            ZFar = 1000.0f;
            FieldOfViewY = UnitConversions.ToRadians(45.0f);
            Behavior = MapBehavior.Load(FilePathHelper.CAMERA_BEHAVIOR_PATH);
        }
    }
}
