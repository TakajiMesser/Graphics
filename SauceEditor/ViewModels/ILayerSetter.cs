using SauceEditorCore.Models.Entities;
using System.Collections.Generic;

namespace SauceEditor.ViewModels
{
    public interface ILayerSetter
    {
        void AddToLayer(string layerName, IEnumerable<IModelEntity> entities);
        void EnableLayer(string layerName);
        void DisableLayer(string layerName);
        void NeutralizeLayer(string layerName);
        void ClearLayer(string layerName);

        //void SetSelectableEntities(string layerName, IEnumerable<IModelEntity> entities);
        //void SelectEntities(IEnumerable<IEntity> entities);
    }
}
