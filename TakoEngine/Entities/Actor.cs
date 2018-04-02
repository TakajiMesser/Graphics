using OpenTK;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Entities.Models;
using TakoEngine.Game;
using TakoEngine.Inputs;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Scripting.Behaviors;

namespace TakoEngine.Entities
{
    public class Actor : IEntity, ICollidable, IRotate
    {
        public int ID { get; set; }
        public string Name { get; private set; }
        public Model Model { get; set; }

        public Vector3 Position
        {
            get => Model.Position;
            set => Model.Position = value;
        }

        private Vector3 _originalRotation;
        public Vector3 OriginalRotation
        {
            get => _originalRotation;
            set
            {
                _originalRotation = value;
                Model._modelMatrix.Rotation = Quaternion.FromEulerAngles(value);
            }
        }

        public Quaternion Rotation
        {
            get => Model.Rotation;
            set => Model.Rotation = value;
        }

        public Vector3 Scale
        {
            get => Model.Scale;
            set => Model.Scale = value;
        }

        public Behavior Behaviors { get; set; }
        public InputMapping InputMapping { get; set; } = new InputMapping();
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = true;

        public Actor(string name)
        {
            Name = name;
        }

        /*On Model Position Change -> if (Bounds != null)
        {
            Bounds.Center = value;
        }*/

        public void ClearLights() => Model.ClearLights();
        public void AddPointLights(IEnumerable<PointLight> lights) => Model.AddPointLights(lights);

        public virtual void OnInitialization()
        {
            if (Behaviors != null)
            {
                Behaviors.Context.Actor = this;

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
                //Behaviors.Context.Rotation = Rotation;
                Behaviors.Context.Scale = Model.Scale;
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

                if (Model is AnimatedModel animated)
                {
                    animated.Animator.CurrentAnimation = animated.Animator.Animations.First();
                    animated.Animator.Tick();
                }

                //Rotation = Quaternion.FromEulerAngles(Behaviors.Context.Rotation);
                Model.Rotation = Behaviors.Context.QRotation;
                Model.Scale = Behaviors.Context.Scale;
            }
        }

        public virtual void HandleCollisions(Vector3 translation, IEnumerable<Bounds> colliders)
        {
            if (HasCollision && Bounds != null && translation != Vector3.Zero)
            {
                Bounds.Center = Model.Position + translation;

                foreach (var collider in colliders)
                {
                    if (collider.AttachedEntity is Actor a)
                    {
                        if (!a.HasCollision)
                        {
                            continue;
                        }
                    }
                    else if (collider.AttachedEntity is Brush b)
                    {
                        if (!b.HasCollision)
                        {
                            continue;
                        }
                    }

                    if (collider.GetType() == typeof(BoundingCircle))
                    {
                        if (Bounds.CollidesWith((BoundingCircle)collider))
                        {
                            // Correct the X translation
                            Bounds.Center = new Vector3(Model.Position.X + translation.X, Model.Position.Y, Model.Position.Z);
                            if (Bounds.CollidesWith((BoundingCircle)collider))
                            {
                                translation.X = 0;
                            }

                            // Correct the Y translation
                            Bounds.Center = new Vector3(Model.Position.X, Model.Position.Y + translation.Y, Model.Position.Z);
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
                            Bounds.Center = new Vector3(Model.Position.X + translation.X, Model.Position.Y, Model.Position.Z);
                            if (Bounds.CollidesWith((BoundingBox)collider))
                            {
                                translation.X = 0;
                            }

                            // Correct the Y translation
                            Bounds.Center = new Vector3(Model.Position.X, Model.Position.Y + translation.Y, Model.Position.Z);
                            if (Bounds.CollidesWith((BoundingBox)collider))
                            {
                                translation.Y = 0;
                            }
                        }
                    }
                }
            }

            Model.Position += translation;
        }

        public virtual void Draw(ShaderProgram program, TextureManager textureManager = null) => Model.Draw(program, textureManager);

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
