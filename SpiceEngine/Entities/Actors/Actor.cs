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
using System;

namespace SpiceEngine.Entities.Actors
{
    public class Actor : IEntity, IRotate, IScale
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

        public Matrix4 ModelMatrix => _modelMatrix.Matrix;

        private ModelMatrix _modelMatrix = new ModelMatrix();

        private Dictionary<int, Material> _materialByMeshIndex = new Dictionary<int, Material>();
        private Dictionary<int, TextureMapping> _textureMappingByMeshIndex = new Dictionary<int, TextureMapping>();

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
            Rotation = actor.Rotation;
            Scale = actor.Scale;
            Orientation = actor.Orientation;

            foreach (var kvp in actor._materialByMeshIndex)
            {
                _materialByMeshIndex.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in actor._textureMappingByMeshIndex)
            {
                var textureMapping = new TextureMapping()
                {
                    DiffuseMapID = kvp.Value.DiffuseMapID,
                    NormalMapID = kvp.Value.NormalMapID,
                    ParallaxMapID = kvp.Value.ParallaxMapID,
                    SpecularMapID = kvp.Value.SpecularMapID
                };

                _textureMappingByMeshIndex.Add(kvp.Key, textureMapping);
            }
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

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
