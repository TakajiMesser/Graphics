using System;
using System.Collections.Generic;

namespace StarchUICore
{
    public interface IUIProvider
    {
        //void AddElement(int entityID, IElement element);
        IElement GetElement(int entityID);

        void Load();
        IEnumerable<int> GetDrawOrder();
        //void Draw();

        void Clear();

        event EventHandler<OrderEventArgs> OrderChanged;
    }
}
