using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities.Lights;

namespace TakoEngine.Rendering.Buffers
{
    public class MatrixStack
    {
        public const string NAME = "PointLightBlock";
        public const int BINDING = 1;

        private MatrixBuffer _modelMatrixBuffer;
        private MatrixBuffer _viewMatrixBuffer;
        private MatrixBuffer _projectionMatrixBuffer;

        public MatrixStack()
        {

        }

        /*public void AddPointLight(PointLight light) => PointLights.Add(light.ToStruct());
        public void AddPointLights(IEnumerable<PointLight> lights) => PointLights.AddRange(lights.Select(l => l.ToStruct()));

        public void Clear() => PointLights.Clear();

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * PointLights.Count, PointLights.ToArray(), BufferUsageHint.DynamicDraw);
        }*/
    }
}
