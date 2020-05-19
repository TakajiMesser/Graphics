using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;

namespace SweetGraphicsCore.Rendering.Batches
{
    public interface IBatchAction
    {
        ICamera Camera { get; set; }

        IBatchAction SetShader(ShaderProgram shader);

        IBatchAction SetCamera(ICamera camera);
        IBatchAction SetCamera(ICamera camera, ILight light);

        IBatchAction SetUniform<T>(string name, T value) where T : struct;
        IBatchAction SetPerIDAction(Action<int> action);
        IBatchAction SetPerBatchAction(Action<IBatch> action);
        IBatchAction SetRenderType(RenderTypes renderType);
        IBatchAction ClearRenderType();

        IBatchAction SetEntityIDSet(IEnumerable<int> ids);
        IBatchAction SetEntityIDOrder(IEnumerable<int> ids);
        IBatchAction ClearEntityIDs();

        IBatchAction PerformAction(Action action);

        IBatchAction SetTexture(ITexture texture, string name, int index);

        IBatchAction Render();

        void Execute();
    }
}
