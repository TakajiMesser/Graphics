using StarchUICore.Views;
using System.Collections.Generic;

namespace StarchUICore
{
    public interface IUIProvider
    {
        void AddItem(int entityID, IUIItem item);
        IUIItem GetItem(int entityID);

        void Load();
        IEnumerable<int> GetDrawOrder();
        //void Draw();

        void Clear();
        
    }
}
