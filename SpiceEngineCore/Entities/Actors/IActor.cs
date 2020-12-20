using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities.Actors
{
    public interface IActor : INamedEntity, IRotate, IScale
    {
        /// <summary>
        /// All models are assumed to have their "forward" direction in the positive X direction.
        /// If the model is oriented in a different direction, this quaternion should orient it from the assumed direction to the correct one.
        /// If the model is already oriented correctly, this should be the quaternion identity.
        /// </summary>
        Quaternion Orientation { get; set; }
    }
}
