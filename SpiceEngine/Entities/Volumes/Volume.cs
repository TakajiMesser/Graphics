using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngine.Entities.Volumes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Volume : Entity, IRotate, IScale
    {
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

        public Volume Duplicate() => new Volume()
        {
            Position = Position,
            Rotation = Rotation,
            Scale = Scale
        };

        //public void Load() => Mesh.Load();
        //public void Draw() => Mesh.Draw();

        public override void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);

        public override bool CompareUniforms(IEntity entity) => false;
    }
}
