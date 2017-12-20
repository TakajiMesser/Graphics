using Graphics.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using Graphics.Inputs;
using Graphics.Utilities;
using System.Diagnostics;

namespace Graphics.GameObjects
{
    public class Player : GameObject
    {
        public const float WALK_SPEED = 0.1f;
        public const float RUN_SPEED = 0.15f;
        public const float CREEP_SPEED = 0.05f;

        public InputMapping InputMapping { get; set; } = new InputMapping();
        public PlayerBehaviorTree BehaviorTree { get; set; } = new PlayerBehaviorTree();

        private float _currentAngle;

        public Player() : base("Player") { }

        public /*override*/ void OnHandleInput(KeyboardState keyState, MouseState mouseState, MouseDevice mouse, int width, int height, Camera camera)
        {
            if (keyState != null)
            {
                HandleMovement(keyState);
            }
            
            if (mouseState != null && mouse != null)
            {
                HandleTurning(mouseState, mouse, width, height, camera);
            }
        }

        private void HandleMovement(KeyboardState keyState)
        {
            var scale = new Vector3();

            if (keyState.IsKeyDown(InputMapping.Evade))
            {

            }

            var translation = new Vector3();

            float speed = keyState.IsKeyDown(InputMapping.Run)
                ? RUN_SPEED
                : keyState.IsKeyDown(InputMapping.Crawl)
                    ? CREEP_SPEED
                    : WALK_SPEED;

            if (keyState.IsKeyDown(InputMapping.Forward))
            {
                translation.Y += speed;
            }

            if (keyState.IsKeyDown(InputMapping.Left))
            {
                translation.X -= speed;
            }

            if (keyState.IsKeyDown(InputMapping.Backward))
            {
                translation.Y -= speed;
            }

            if (keyState.IsKeyDown(InputMapping.Right))
            {
                translation.X += speed;
            }

            if (keyState.IsKeyDown(InputMapping.In))
            {
                translation.Z += speed;
            }

            if (keyState.IsKeyDown(InputMapping.Out))
            {
                translation.Z -= speed;
            }

            //Transform.Translation = Transform.Translation + translation;
            Transform = Transform.FromTranslation(translation);
        }

        private void HandleTurning(MouseState mouseState, MouseDevice mouse, int width, int height, Camera camera)
        {
            Quaternion rotation = Quaternion.Identity;

            // Compare current position to location of mouse, and set rotation to face the mouse
            var mouseCoordinates = new Vector2(mouse.X, mouse.Y);

            if (mouseCoordinates.X.IsBetween(0.0f, width) && mouseCoordinates.Y.IsBetween(0.0f, height))
            {
                Matrix4 viewProjectionMatrix = camera._projectionMatrix.Matrix * camera._viewMatrix.Matrix;
                var clipSpacePosition = viewProjectionMatrix * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//Position, 1.0f);
                var screenCoordinates = new Vector2()
                {
                    X = ((clipSpacePosition.X + 1.0f) / 2.0f) * width,
                    Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * height,
                };

                var vectorBetween = mouseCoordinates - screenCoordinates;
                float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);

                // Extract yaw from existing transformation
                //double siny = 2.0 * (Transform.Rotation.W * Transform.Rotation.Z + Transform.Rotation.X * Transform.Rotation.Y);
                //double cosy = 1.0 - 2.0 * (Transform.Rotation.Y * Transform.Rotation.Y + Transform.Rotation.Z * Transform.Rotation.Z);
                //float previousTurnAngle = (float)Math.Atan2(siny, cosy);

                if (_currentAngle != turnAngle)
                {
                    rotation = new Quaternion(new Vector3(turnAngle - _currentAngle, 0.0f, 0.0f));
                    _currentAngle = turnAngle;

                    //rotation = new Quaternion(new Vector3(turnAngle - previousTurnAngle, 0.0f, 0.0f));

                    /*Console.WriteLine("<" + rotation.W + ", " + rotation.X + ", " + rotation.Y + ", " + rotation.Z
                        + "> (" + turnAngle + "," + previousTurnAngle + ")");*/
                }
            }

            Transform.Rotation = rotation;
        }
    }
}
