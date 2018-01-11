using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Rendering.Shaders;
using Graphics.Inputs;
using Graphics.Rendering.Matrices;

namespace Graphics.GameObjects
{
    public class PerspectiveCamera : Camera
    {
        public const float ZNEAR = 0.1f;
        public const float ZFAR = 1000.0f;

        public PerspectiveCamera(string name, int width, int height, float fieldOfViewY) : base(name, width, height)
        {
            _projectionMatrix.FieldOfView = fieldOfViewY;
            _projectionMatrix.ZNear = ZNEAR;
            _projectionMatrix.ZFar = ZFAR;
        }

        public override void OnHandleInput(InputState inputState)
        {
            float amount = inputState.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                Position = new Vector3(Position.X, Position.Y, Position.Z + amount);
            }
        }
    }
}
