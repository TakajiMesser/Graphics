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
using Graphics.Physics.Collision;

namespace Graphics.GameObjects
{
    public class Player : GameObject
    {
        public const float WALK_SPEED = 0.1f;
        public const float RUN_SPEED = 0.15f;
        public const float CREEP_SPEED = 0.04f;
        public const float EVADE_SPEED = 0.175f;
        public const int EVADE_TICK_COUNT = 20;

        private int _nEvadeTicks = 0;

        public InputMapping InputMapping { get; set; } = new InputMapping();
        public PlayerBehaviorTree BehaviorTree { get; set; } = new PlayerBehaviorTree();

        public Player() : base("Player") { }

        public override void OnHandleInput(InputState inputState, Camera camera, IEnumerable<ICollider> colliders)
        {
            HandleMovement(inputState, colliders);
            HandleTurning(inputState, camera);
        }

        private Vector3 _rollDirection;

        private void HandleMovement(InputState inputState, IEnumerable<ICollider> colliders)
        {
            if (_nEvadeTicks == 0 && inputState.IsPressed(InputMapping.Evade))
            {
                var evadeTranslation = GetTranslation(inputState, EVADE_SPEED);

                if (evadeTranslation != Vector3.Zero)
                {
                    _nEvadeTicks++;
                    Scale = new Vector3(1.0f, 0.5f, 1.0f);
                    _rollDirection = evadeTranslation;
                }
            }
            else if (_nEvadeTicks > EVADE_TICK_COUNT)
            {
                _nEvadeTicks = 0;
                Scale = Vector3.One;
                _rollDirection = Vector3.Zero;
            }
            else if (_nEvadeTicks > 0)
            {
                _nEvadeTicks++;
            }

            float speed = inputState.IsHeld(InputMapping.Run)
                ? RUN_SPEED
                : inputState.IsHeld(InputMapping.Crawl)
                    ? CREEP_SPEED
                    : WALK_SPEED;

            var translation = (_nEvadeTicks > 0) ? _rollDirection : GetTranslation(inputState, speed);
            TranslateUnlessCollision(translation, colliders);
        }

        private Vector3 GetTranslation(InputState inputState, float speed)
        {
            var translation = new Vector3();

            if (inputState.IsHeld(InputMapping.Forward))
            {
                translation.Y += speed;
            }

            if (inputState.IsHeld(InputMapping.Left))
            {
                translation.X -= speed;
            }

            if (inputState.IsHeld(InputMapping.Backward))
            {
                translation.Y -= speed;
            }

            if (inputState.IsHeld(InputMapping.Right))
            {
                translation.X += speed;
            }

            if (inputState.IsHeld(InputMapping.In))
            {
                translation.Z += speed;
            }

            if (inputState.IsHeld(InputMapping.Out))
            {
                translation.Z -= speed;
            }

            return translation;
        }

        private void HandleTurning(InputState inputState, Camera camera)
        {
            // Compare current position to location of mouse, and set rotation to face the mouse
            if (_nEvadeTicks == 0 && inputState.IsMouseInWindow)
            {
                var clipSpacePosition = camera.ViewProjectionMatrix * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//Position, 1.0f);
                var screenCoordinates = new Vector2()
                {
                    X = ((clipSpacePosition.X + 1.0f) / 2.0f) * inputState.WindowWidth,
                    Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * inputState.WindowHeight,
                };

                var vectorBetween = inputState.MouseCoordinates - screenCoordinates;
                float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                Rotation = new Quaternion(new Vector3(turnAngle, 0.0f, 0.0f));
            }
        }
    }
}
