using SauceEditorCore.Models.Entities;
using SpiceEngine.Entities;
using System.Collections.Generic;

namespace SauceEditor.ViewModels
{
    public interface ISelectEntities
    {
        void EnableLayer(string layerName, IEnumerable<IModelEntity> entities);
        void DisableLayer(string layerName);

        void SetSelectableEntities(string layerName, IEnumerable<IModelEntity> entities);
        void SelectEntities(IEnumerable<IEntity> entities);
    }
}
