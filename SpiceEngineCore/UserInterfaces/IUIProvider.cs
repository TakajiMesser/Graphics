using System;
using System.Collections.Generic;

namespace SpiceEngineCore.UserInterfaces
{
    public interface IUIProvider
    {
        //void AddElement(int entityID, IElement element);
        IUIElement GetUIElement(int entityID);

        void Load();
        IEnumerable<int> GetDrawOrder();
        //void Draw();

        void Clear();

        event EventHandler<OrderEventArgs> OrderChanged;
    }
}
