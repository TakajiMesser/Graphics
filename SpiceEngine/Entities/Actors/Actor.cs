using OpenTK;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Actors
{
    public class Actor : IEntity, IRotate, IScale
    {
        private ModelMatrix _modelMatrix = new ModelMatrix();

        public int ID { get; set; }
        public string Name { get; private set; }

        /// <summary>
        /// All models are assumed to have their "forward" direction in the positive X direction.
        /// If the model is oriented in a different direction, this quaternion should orient it from the assumed direction to the correct one.
        /// If the model is already oriented correctly, this should be the quaternion identity.
        /// </summary>
        public Quaternion Orientation { get; set; } = Quaternion.Identity;

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public Quaternion Rotation
        {
            // TODO - Determine if quaternion multiplication order matters here
            get => _modelMatrix.Rotation * Orientation.Inverted();
            set => _modelMatrix.Rotation = Orientation * value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public Matrix4 ModelMatrix => _modelMatrix.Matrix;

        public List<Material> Materials { get; set; } = new List<Material>();
        public List<TextureMapping?> TextureMappings { get; set; } = new List<TextureMapping?>();

        public Actor(string name)
        {
            Name = name;
        }

        public virtual Actor Duplicate(string name)
        {
            var actor = new Actor(name);
            actor.FromActor(this);
            return actor;
        }

        protected void FromActor(Actor actor)
        {
            Position = actor.Position;
            Orientation = actor.Orientation;
            Rotation = actor.Rotation;
            Scale = actor.Scale;

            Materials.AddRange(actor.Materials);
            TextureMappings.AddRange(actor.TextureMappings);
        }

        public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);

        public virtual void SetUniforms(ShaderProgram program, ITextureProvider textureProvider, int meshIndex)
        {
            Materials[meshIndex].SetUniforms(program);
            if (textureProvider != null && TextureMappings[meshIndex].HasValue)
            {
                program.BindTextures(textureProvider, TextureMappings[meshIndex].Value);
            }
            else
            {
                program.UnbindTextures();
            }
        }

        /*On Model Position Change -> if (Bounds != null)
        {
            Bounds.Center = value;
        }*/

        //public void ClearLights() => Model.ClearLights();
        //public void AddPointLights(IEnumerable<PointLight> lights) => Model.AddPointLights(lights);

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
