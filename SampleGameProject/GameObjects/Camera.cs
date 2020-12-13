using SampleGameProject.Helpers;
using SpiceEngine.Maps;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
