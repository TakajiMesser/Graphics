using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Inputs;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Outputs;

namespace TakoEngine.GameObjects
{
    public class OrthographicCamera : Camera
    {
        public const float ZNEAR = -10.0f;
        public const float ZFAR = 10.0f;

        public OrthographicCamera(string name, Resolution resolution, float startingWidth) : base(name, resolution)
        {
            _projectionMatrix.Type = ProjectionTypes.Orthographic;
            _projectionMatrix.Width = startingWidth;
            _projectionMatrix.ZNear = ZNEAR;
            _projectionMatrix.ZFar = ZFAR;
        }

        public override void OnHandleInput(InputState inputState)
        {
            float amount = inputState.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _projectionMatrix.Width += amount;
            }
        }
    }
}
