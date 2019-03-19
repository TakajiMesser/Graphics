using SpiceEngine.Game;

namespace SpiceEngine.Sounds
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
                AL.Listener(ALListener3f.Position, value);
            }
        }

        public Vector3 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                AL.Listener(ALListener3f.Velocity, value);
            }
        }

        public Vector3 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                AL.Listener(ALListenerfv.Orientation, value, _up);
            }
        }

        public Vector3 Up
        {
            get => _up;
            set
            {
                _up = value;
                AL.Listener(ALListenerfv.Orientation, _direction, value);
            }
        }
    }
}
