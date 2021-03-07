using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering;
using System.Collections.Generic;

namespace SweetGraphicsCore.Buffers
{
    public class MatrixStack
    {
        public const string MODEL_NAME = "ModelMatrixBlock";
        public const int MODEL_BINDING = 1;

        public const string VIEW_NAME = "ViewMatrixBlock";
        public const int VIEW_BINDING = 2;

        public const string PROJECTION_NAME = "ProjectionMatrixBlock";
        public const int PROJECTION_BINDING = 3;

        public const string MVP_NAME = "MVPMatrixBlock";
        public const int MVP_BINDING = 4;

        private MatrixBuffer _modelMatrixBuffer;
        private MatrixBuffer _viewMatrixBuffer;
        private MatrixBuffer _projectionMatrixBuffer;
        private MatrixBuffer _mvpMatrixBuffer;

        public MatrixStack(IRenderContextProvider contextProvider)
        {
            _modelMatrixBuffer = new MatrixBuffer(contextProvider, MODEL_NAME, MODEL_BINDING);
            _viewMatrixBuffer = new MatrixBuffer(contextProvider, VIEW_NAME, VIEW_BINDING);
            _projectionMatrixBuffer = new MatrixBuffer(contextProvider, PROJECTION_NAME, PROJECTION_BINDING);
            _mvpMatrixBuffer = new MatrixBuffer(contextProvider, MVP_NAME, MVP_BINDING);
        }

        public void AddEntities(ICamera camera, IEnumerable<Actor> actors, IEnumerable<Brush> brushes)
        {
            _viewMatrixBuffer.AddMatrix(camera.CurrentModelMatrix);
            _projectionMatrixBuffer.AddMatrix(camera.CurrentProjectionMatrix);

            /*foreach (var actor in actors)
            {
                _modelMatrixBuffer.AddMatrix(actor.ModelMatrix);

                // TODO - Confirm whether or not this multiplication order should be reversed
                _mvpMatrixBuffer.AddMatrix(actor.ModelMatrix * camera.ViewMatrix * camera.ProjectionMatrix);
            }

            foreach (var brush in brushes)
            {
                _modelMatrixBuffer.AddMatrix(brush.GetModelMatrix());

                // TODO - Confirm whether or not this multiplication order should be reversed
                _mvpMatrixBuffer.AddMatrix(brush.GetModelMatrix() * camera.ViewMatrix * camera.ProjectionMatrix);
            }*/
        }

        public void Bind()
        {
            _modelMatrixBuffer.Bind();
            _viewMatrixBuffer.Bind();
            _projectionMatrixBuffer.Bind();
            _mvpMatrixBuffer.Bind();
        }

        public void Clear()
        {
            _modelMatrixBuffer.Clear();
            _viewMatrixBuffer.Clear();
            _projectionMatrixBuffer.Clear();
            _mvpMatrixBuffer.Clear();
        }
    }
}
