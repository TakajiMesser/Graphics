using SpiceEngineCore.Geometry.Quaternions;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngineCore.Entities.Actors
{
    public class Actor : Entity, IActor//TexturedEntity, IActor, IHaveModel
    {
        /*protected int _meshIndex = 0;

        private List<Material> _materials = new List<Material>();
        private List<TextureMapping?> _textureMappings = new List<TextureMapping?>();

        public override IEnumerable<Material> Materials => _materials;
        public override IEnumerable<TextureMapping?> TextureMappings => _textureMappings;

        public override Material CurrentMaterial => _materials[_meshIndex];
        public override TextureMapping? CurrentTextureMapping => _textureMappings[_meshIndex];

        public override void AddMaterial(Material material) => _materials.Add(material);
        public override void AddTextureMapping(TextureMapping? textureMapping) => _textureMappings.Add(textureMapping);*/

        public string Name { get; set; }

        /// <summary>
        /// All models are assumed to have their "forward" direction in the positive X direction.
        /// If the model is oriented in a different direction, this quaternion should orient it from the assumed direction to the correct one.
        /// If the model is already oriented correctly, this should be the quaternion identity.
        /// </summary>
        public Quaternion Orientation { get; set; } = Quaternion.Identity;

        // TODO - Determine if quaternion multiplication order matters here
        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation * Orientation.Inverted();
            set => _modelMatrix.Rotation = Orientation * value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public override void Transform(Transform transform)
        {
            base.Transform(new Transform()
            {
                Translation = transform.Translation,
                Rotation = Orientation * transform.Rotation,
                Scale = transform.Scale
            });
        }

        //public void SetMeshIndex(int meshIndex) => _meshIndex = meshIndex;

        /*public override void SetUniforms(ShaderProgram program)
        {
            // TODO - Make this less janky. For now, we only want to set the model matrix once for all meshes, so bind it for the first mesh only
            if (_meshIndex == 0)
            {
                _modelMatrix.Set(program);
            }

            base.SetUniforms(program);
        }*/

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
