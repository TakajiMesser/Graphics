using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Entities
{
    public abstract class Entity : IEntity
    {
        protected ModelMatrix _modelMatrix = new ModelMatrix();

        public int ID { get; set; }

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);
    }
}
