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
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Materials;

namespace SpiceEngine.Entities
{
    public class Actor : IEntity, IStimulate, IRotate, IScale
    {
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
            get => Orientation * _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = Orientation * value;
        }

        private Vector3 _originalRotation;
        public Vector3 OriginalRotation
        {
            get => _originalRotation;
            set
            {
                _originalRotation = value;
                _modelMatrix.Rotation = Quaternion.FromEulerAngles(value);
            }
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public Behavior Behaviors { get; set; }
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();
        public InputBinding InputMapping { get; set; } = new InputBinding();
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();

        public Matrix4 ModelMatrix => _modelMatrix.Matrix;

        private ModelMatrix _modelMatrix = new ModelMatrix();

        private Dictionary<int, Material> _materialByMeshIndex = new Dictionary<int, Material>();
        private Dictionary<int, TextureMapping> _textureMappingByMeshIndex = new Dictionary<int, TextureMapping>();

        public Actor(string name)
        {
            Name = name;
        }

        public void AddMaterial(int meshIndex, Material material) => _materialByMeshIndex.Add(meshIndex, material);

        public void AddTextureMapping(int meshIndex, TextureMapping textureMapping) => _textureMappingByMeshIndex.Add(meshIndex, textureMapping);

        public virtual void SetUniforms(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);
        }

        public virtual void SetUniforms(ShaderProgram program, TextureManager textureManager, int meshIndex)
        {
            _materialByMeshIndex[meshIndex].SetUniforms(program);
            if (textureManager != null && _textureMappingByMeshIndex.ContainsKey(meshIndex))
            {
                program.BindTextures(textureManager, _textureMappingByMeshIndex[meshIndex]);
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

        public virtual void OnHandleInput(InputManager inputManager, Camera camera)
        {
            if (Behaviors != null)
            {
                Behaviors.Context.InputManager = inputManager;
                Behaviors.Context.InputMapping = InputMapping;
                Behaviors.Context.Camera = camera;
            }
        }

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
