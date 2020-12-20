using OpenTK;
using OpenTK.Audio.OpenAL;

/*using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;*/

namespace SmokyAudioCore.Sounds
{
    public class Listener
    {
        private Vector3 _position;
        private Vector3 _velocity;
        private Vector3 _direction;
        private Vector3 _up;

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                AL.Listener(ALListener3f.Position, ref _position);
            }
        }

        public Vector3 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                AL.Listener(ALListener3f.Velocity, ref _velocity);
            }
        }

        public Vector3 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                AL.Listener(ALListenerfv.Orientation, ref _direction, ref _up);
            }
        }

        public Vector3 Up
        {
            get => _up;
            set
            {
                _up = value;
                AL.Listener(ALListenerfv.Orientation, ref _direction, ref _up);
            }
        }
    }
}
