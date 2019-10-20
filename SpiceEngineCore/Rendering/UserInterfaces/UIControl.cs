using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using System;

namespace SpiceEngineCore.Rendering.UserInterfaces
{
    public abstract class UIControl : IUIControl
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Vector3 Position { get; set; }

        public event EventHandler<EntityTransformEventArgs> Transformed;

        public IEntity Duplicate() => throw new NotImplementedException();

        public virtual void Transform(Transform transform)
        {

        }

        public virtual void SetUniforms(ShaderProgram shader)
        {

        }

        public bool CompareUniforms(IEntity entity) => false;

        /*public Position Position { get; set; }
        public Size Size { get; set; }
        public Color Color { get; set; }*/
    }
}
