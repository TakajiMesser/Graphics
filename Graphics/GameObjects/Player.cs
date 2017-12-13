using Graphics.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Graphics.GameObjects
{
    public class Player : GameObject
    {
        public const float WALK_SPEED = 0.01f;
        public const float RUN_SPEED = 0.015f;
        public const float CREEP_SPEED = 0.005f;

        public Player() : base("Player") { }

        public override void OnHandleInput(KeyboardState keyState, MouseState mouseState, KeyboardState previousKeyState, MouseState previousMouseState)
        {
            Vector3 translation = new Vector3();

            float speed = keyState.IsKeyDown(Key.ShiftLeft)
                ? RUN_SPEED
                : keyState.IsKeyDown(Key.ControlLeft)
                    ? CREEP_SPEED
                    : WALK_SPEED;

            if (keyState.IsKeyDown(Key.W))
            {
                translation.Y += speed;
            }

            if (keyState.IsKeyDown(Key.A))
            {
                translation.X -= speed;
            }

            if (keyState.IsKeyDown(Key.S))
            {
                translation.Y -= speed;
            }

            if (keyState.IsKeyDown(Key.D))
            {
                translation.X += speed;
            }

            if (keyState.IsKeyDown(Key.Q))
            {
                translation.Z += speed;
            }

            if (keyState.IsKeyDown(Key.E))
            {
                translation.Z -= speed;
            }

            Transform = Transform.FromTranslation(translation);
        }
    }
}
