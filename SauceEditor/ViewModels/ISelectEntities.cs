﻿using SpiceEngine.Entities;
using System.Collections.Generic;

namespace SauceEditor.ViewModels
{
    public interface ISelectEntities
    {
        void SetSelectableEntities(IEnumerable<IEntity> entities);
        void SelectEntities(IEnumerable<IEntity> entities);
    }
}
