using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Entities.Volumes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Volume : Entity, IRotate, IScale
    {
        public Quaternion Rotation => _modelMatrix.Rotation;
        public Vector3 Scale => _modelMatrix.Scale;

        public void SetRotation(Quaternion rotation) => _modelMatrix.SetRotation(rotation);
        public void SetScale(Vector3 scale) => _modelMatrix.SetScale(scale);

        public Volume Duplicate()
        {
            var volume = new Volume();
            volume.SetPosition(Position);
            volume.SetRotation(Rotation);
            volume.SetScale(Scale);

            return volume;
        }

        //public void Load() => Mesh.Load();
        //public void Draw() => Mesh.Draw();

        public override void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);

        public override bool CompareUniforms(IEntity entity) => false;
    }
}
