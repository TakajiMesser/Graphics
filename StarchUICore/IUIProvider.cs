using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;

namespace StarchUICore
{
    public interface IUIProvider : IGameSystem, IComponentProvider<IElement>
    {
        IEnumerable<int> GetDrawOrder();

        void RegisterSelection(int entityID);
        void RegisterDeselection(int entityID);

        void SetTextureProvider(ITextureProvider textureProvider);

        event EventHandler<OrderEventArgs> OrderChanged;
    }
}
