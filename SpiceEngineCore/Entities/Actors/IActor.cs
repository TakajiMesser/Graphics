using OpenTK;

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
