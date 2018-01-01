using Graphics.Inputs;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Matrices;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Graphics.GameObjects
{
    public class GameObject
    {
        internal ShaderProgram _program;
        private ModelMatrix _modelMatrix = new ModelMatrix();
        private Collider _collider;

        //public int ID { get; private set; }
        public string Name { get; private set; }
        public Mesh Mesh { get; set; }
        public BehaviorTree Behaviors { get; set; }
        public InputMapping InputMapping { get; set; } = new InputMapping();
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public Collider Collider
        {
            get => _collider;
            set
            {
                _collider = value;
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

        public virtual void OnInitialization()
        {
            if (Behaviors != null)
            {
                foreach (var property in Properties)
                {
                    if (property.Value.IsConstant)
                    {
                        Behaviors.VariablesByName.Add(property.Key, property.Value.Value);
                    }
                }
            }
        }

        public virtual void OnHandleInput(InputState inputState, Camera camera, IEnumerable<Collider> colliders)
        {
            if (Behaviors != null)
            {
                Behaviors.VariablesByName["InputState"] = inputState;
                Behaviors.VariablesByName["InputMapping"] = InputMapping;
                Behaviors.VariablesByName["Camera"] = camera;
            }
        }

        public virtual void OnUpdateFrame(IEnumerable<Collider> colliders)
        {
            if (Behaviors != null)
            {
                Behaviors.VariablesByName["Name"] = Name;
                Behaviors.VariablesByName["Position"] = Position;
                Behaviors.VariablesByName["Rotation"] = Rotation;
                Behaviors.VariablesByName["Scale"] = Scale;

                foreach (var property in Properties.Where(p => !p.Value.IsConstant))
                {
                    Behaviors.VariablesByName[property.Key] = property.Value;
                }

                Behaviors.Tick();

                if (Behaviors.VariablesByName.ContainsKey("Translation"))
                {
                    HandleCollisions((Vector3)Behaviors.VariablesByName["Translation"], colliders);
                    Behaviors.VariablesByName["Translation"] = Vector3.Zero;
                }

                Rotation = (Quaternion)Behaviors.VariablesByName["Rotation"];
                Scale = (Vector3)Behaviors.VariablesByName["Scale"];
            }
        }

        public virtual void HandleCollisions(Vector3 translation, IEnumerable<Collider> colliders)
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
