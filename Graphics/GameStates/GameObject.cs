using Graphics.Inputs;
using Graphics.Matrices;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    public class GameObject
    {
        internal ShaderProgram _program;
        private ModelMatrix _modelMatrix = new ModelMatrix();
        private ICollider _collider;

        public string Name { get; private set; }
        public Mesh Mesh { get; set; }
        public BehaviorTree Behaviors
        {
            get;
            set;
        }
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public ICollider Collider
        {
            get => _collider;
            set
            {
                _collider = value;
                _collider.Properties = Properties;
            }
        }
        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set
            {
                _modelMatrix.Translation = value;

                if (Collider != null)
                {
                    Collider.Center = value;
                }
            }
        }
        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }
        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public GameObject(string name)
        {
            Name = name;
        }

        public virtual void OnHandleInput(InputState inputState, Camera camera, IEnumerable<ICollider> colliders)
        {

        }

        public virtual void OnUpdateFrame(IEnumerable<ICollider> colliders)
        {
            if (Behaviors != null)
            {
                Behaviors.VariablesByName["Name"] = Name;
                Behaviors.VariablesByName["Position"] = Position;
                Behaviors.VariablesByName["Rotation"] = Rotation;
                Behaviors.VariablesByName["Scale"] = Scale;

                Behaviors.Tick();

                if (Behaviors.VariablesByName.ContainsKey("Translation"))
                {
                    HandleCollisions((Vector3)Behaviors.VariablesByName["Translation"], colliders);
                    Behaviors.VariablesByName["Translation"] = Vector3.Zero;
                }
            }
        }

        public virtual void HandleCollisions(Vector3 translation, IEnumerable<ICollider> colliders)
        {
            if (translation != Vector3.Zero && Collider != null)
            {
                Collider.Center = Position + translation;

                foreach (var collider in colliders)
                {
                    if (collider.GetType() == typeof(BoundingSphere))
                    {
                        if (Collider.CollidesWith((BoundingSphere)collider))
                        {
                            // Correct the X translation
                            Collider.Center = new Vector3(Position.X + translation.X, Position.Y, Position.Z);
                            if (Collider.CollidesWith((BoundingSphere)collider))
                            {
                                translation.X = 0;
                            }

                            // Correct the Y translation
                            Collider.Center = new Vector3(Position.X, Position.Y + translation.Y, Position.Z);
                            if (Collider.CollidesWith((BoundingSphere)collider))
                            {
                                translation.Y = 0;
                            }
                        }
                    }
                    else if (collider.GetType() == typeof(BoundingBox))
                    {
                        if (Collider.CollidesWith((BoundingBox)collider))
                        {
                            // Correct the X translation
                            Collider.Center = new Vector3(Position.X + translation.X, Position.Y, Position.Z);
                            if (Collider.CollidesWith((BoundingBox)collider))
                            {
                                translation.X = 0;
                            }

                            // Correct the Y translation
                            Collider.Center = new Vector3(Position.X, Position.Y + translation.Y, Position.Z);
                            if (Collider.CollidesWith((BoundingBox)collider))
                            {
                                translation.Y = 0;
                            }
                        }
                    }
                }
            }

            Position += translation;
        }

        public virtual void OnRenderFrame()
        {
            _modelMatrix.Set(_program);
            Mesh?.Draw();
        }

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
