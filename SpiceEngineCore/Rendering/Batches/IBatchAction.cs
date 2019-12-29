using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Batches
{
    public interface IBatchAction
    {
        ICamera Camera { get; set; }

        IBatchAction SetShader(ShaderProgram shader);

        IBatchAction SetCamera(ICamera camera);
        IBatchAction SetCamera(ICamera camera, ILight light);

        IBatchAction SetUniform<T>(string name, T value) where T : struct;

        IBatchAction SetEntityIDs(IEnumerable<int> ids);
        IBatchAction ClearEntityIDs();

        IBatchAction PerformAction(Action action);

        IBatchAction SetTexture(ITexture texture, string name, int index);

        IBatchAction RenderEntities();

        IBatchAction RenderOpaqueStatic();
        IBatchAction RenderOpaqueAnimated();
        IBatchAction RenderOpaqueBillboard();
        IBatchAction RenderOpaqueViews();
        IBatchAction RenderTransparentStatic();
        IBatchAction RenderTransparentAnimated();
        IBatchAction RenderTransparentBillboard();
        IBatchAction RenderTransparentViews();

        IBatchAction RenderOpaqueStaticWithAction(Action<int> action);
        IBatchAction RenderOpaqueAnimatedWithAction(Action<int> action);
        IBatchAction RenderTransparentStaticWithAction(Action<int> action);
        IBatchAction RenderTransparentAnimatedWithAction(Action<int> action);

        void Execute();
    }
}
