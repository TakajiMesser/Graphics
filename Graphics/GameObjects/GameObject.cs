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
using Graphics.Lighting;
using Graphics.Rendering.Textures;
using OpenTK.Graphics.OpenGL;

namespace Graphics.GameObjects
{
    public class GameObject
    {
        internal ModelMatrix _modelMatrix = new ModelMatrix();
        internal Matrix4 _previousModelMatrix;

        //public int ID { get; private set; }
        public string Name { get; private set; }
        public Mesh Mesh { get; set; }
        public TextureMapping TextureMapping { get; set; }
        public BehaviorTree Behaviors { get; set; }
        public InputMapping InputMapping { get; set; } = new InputMapping();
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();

        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = true;
        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set
            {
                _modelMatrix.Translation = value;

                if (Bounds != null)
                {
                    Bounds.Center = value;
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

        public void ClearLights() => Mesh.ClearLights();
        public void AddLights(IEnumerable<Light> lights) => Mesh.AddLights(lights);

        public virtual void OnInitialization()
        {
            if (Behaviors != null)
            {
                Behaviors.Context.GameObjectName = Name;
                Behaviors.Context.Bounds = Bounds;

                foreach (var property in Properties)
                {
                    if (property.Value.IsConstant)
                    {
                        Behaviors.Context.AddProperty(property.Key, property.Value.Value);
                    }
                }
            }
        }

        public virtual void OnHandleInput(InputState inputState, Camera camera)
        {
            if (Behaviors != null)
            {
                Behaviors.Context.InputState = inputState;
                Behaviors.Context.InputMapping = InputMapping;
                Behaviors.Context.Camera = camera;
            }
        }

        public virtual void OnUpdateFrame(IEnumerable<Bounds> colliders)
        {
            if (Behaviors != null)
            {
                Behaviors.Context.Position = Position;
                //Behaviors.Context.Rotation = Rotation;
                Behaviors.Context.Scale = Scale;
                Behaviors.Context.Colliders = colliders;

                foreach (var property in Properties.Where(p => !p.Value.IsConstant))
                {
                    Behaviors.Context.SetProperty(property.Key, property.Value);
                }

                Behaviors.Tick();

                if (Behaviors.Context.Translation != Vector3.Zero)
                {
                    HandleCollisions(Behaviors.Context.Translation, colliders);
                    Behaviors.Context.Translation = Vector3.Zero;
                }

                //Rotation = Quaternion.FromEulerAngles(Behaviors.Context.Rotation);
                Rotation = Behaviors.Context.QRotation;
                Scale = Behaviors.Context.Scale;
            }
        }

        public virtual void HandleCollisions(Vector3 translation, IEnumerable<Bounds> colliders)
        {
            if (HasCollision && Bounds != null && translation != Vector3.Zero)
            {
                Bounds.Center = Position + translation;

                foreach (var collider in colliders)
                {
                    if (collider.GetType() == typeof(BoundingCircle))
                    {
                        if (Bounds.CollidesWith((BoundingCircle)collider))
                        {
                            // Correct the X translation
                            Bounds.Center = new Vector3(Position.X + translation.X, Position.Y, Position.Z);
                            if (Bounds.CollidesWith((BoundingCircle)collider))
                            {
                                translation.X = 0;
                            }

                            // Correct the Y translation
                            Bounds.Center = new Vector3(Position.X, Position.Y + translation.Y, Position.Z);
                            if (Bounds.CollidesWith((BoundingCircle)collider))
                            {
                                translation.Y = 0;
                            }
                        }
                    }
                    else if (collider.GetType() == typeof(BoundingBox))
                    {
                        if (Bounds.CollidesWith((BoundingBox)collider))
                        {
                            // Correct the X translation
                            Bounds.Center = new Vector3(Position.X + translation.X, Position.Y, Position.Z);
                            if (Bounds.CollidesWith((BoundingBox)collider))
                            {
                                translation.X = 0;
                            }

                            // Correct the Y translation
                            Bounds.Center = new Vector3(Position.X, Position.Y + translation.Y, Position.Z);
                            if (Bounds.CollidesWith((BoundingBox)collider))
                            {
                                translation.Y = 0;
                            }
                        }
                    }
                }
            }

            Position += translation;
        }

        public void Draw(ShaderProgram program)
        {
            if (Mesh == null)
            {
                throw new InvalidOperationException("Cannot draw GameObject " + Name + " with null mesh");
            }

            _modelMatrix.Set(program);

            int location = program.GetUniformLocation("previousModelMatrix");
            GL.UniformMatrix4(location, false, ref _previousModelMatrix);

            _previousModelMatrix = _modelMatrix.Model;

            Mesh.Draw();
        }

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
