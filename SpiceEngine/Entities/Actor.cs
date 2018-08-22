using OpenTK;
using System.Collections.Generic;
using System.Linq;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Models;
using SpiceEngine.Game;
using SpiceEngine.Inputs;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.StimResponse;

namespace SpiceEngine.Entities
{
    public class Actor : IEntity, IStimulate, ICollidable, IRotate, IScale
    {
        public int ID { get; set; }
        public string Name { get; private set; }
        public IModel3D Model { get; set; }

        public Vector3 Position
        {
            get => Model.Position;
            set => Model.Position = value;
        }

        public Quaternion Rotation
        {
            get => Model.Rotation;
            set => Model.Rotation = value;
        }

        public Vector3 OriginalRotation
        {
            get => Model.OriginalRotation;
            set => Model.OriginalRotation = value;
        }

        public Vector3 Scale
        {
            get => Model.Scale;
            set => Model.Scale = value;
        }

        public Behavior Behaviors { get; set; }
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();
        public InputMapping InputMapping { get; set; } = new InputMapping();
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();

        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = true;

        public Actor(string name)
        {
            Name = name;
        }

        public void Load() => Model.Load();
        public void Draw() => Model.Draw();

        /*On Model Position Change -> if (Bounds != null)
        {
            Bounds.Center = value;
        }*/

        //public void ClearLights() => Model.ClearLights();
        //public void AddPointLights(IEnumerable<PointLight> lights) => Model.AddPointLights(lights);

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

                if (Model is AnimatedModel3D animated)
                {
                    animated.Animator.CurrentAnimation = animated.Animator.Animations.First();
                    animated.Animator.Tick();
                }

                //Rotation = Quaternion.FromEulerAngles(Behaviors.Context.Rotation);
                //Model.Rotation = Behaviors.Context.QRotation;
            }
        }

        public virtual void HandleCollisions(Vector3 translation, IEnumerable<Bounds> colliders)
        {
            if (HasCollision && Bounds != null && translation != Vector3.Zero)
            {
                Bounds.Center = Model.Position + translation;

                foreach (var collider in colliders)
                {
                    if (collider.AttachedEntity is ICollidable collidable && collidable.HasCollision)
                    {
                        switch (collider)
                        {
                            case BoundingCircle circle:
                                if (Bounds.CollidesWith(circle))
                                {
                                    // Correct the X translation
                                    Bounds.Center = new Vector3(Model.Position.X + translation.X, Model.Position.Y, Model.Position.Z);
                                    if (Bounds.CollidesWith(circle))
                                    {
                                        translation.X = 0;
                                    }

                                    // Correct the Y translation
                                    Bounds.Center = new Vector3(Model.Position.X, Model.Position.Y + translation.Y, Model.Position.Z);
                                    if (Bounds.CollidesWith(circle))
                                    {
                                        translation.Y = 0;
                                    }
                                }
                                break;

                            case BoundingBox box:
                                if (Bounds.CollidesWith(box))
                                {
                                    // Correct the X translation
                                    Bounds.Center = new Vector3(Model.Position.X + translation.X, Model.Position.Y, Model.Position.Z);
                                    if (Bounds.CollidesWith(box))
                                    {
                                        translation.X = 0;
                                    }

                                    // Correct the Y translation
                                    Bounds.Center = new Vector3(Model.Position.X, Model.Position.Y + translation.Y, Model.Position.Z);
                                    if (Bounds.CollidesWith(box))
                                    {
                                        translation.Y = 0;
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            Model.Position += translation;
        }

        public virtual void SetUniforms(ShaderProgram program, TextureManager textureManager = null) => Model.SetUniforms(program, textureManager);
        public virtual void SetUniformsAndDraw(ShaderProgram program, TextureManager textureManager = null) => Model.SetUniformsAndDraw(program, textureManager);

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
