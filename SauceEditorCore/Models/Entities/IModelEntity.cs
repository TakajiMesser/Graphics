using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Entities
{
    public interface IModelEntity : IEntity
    {
        IRenderable ToRenderable();
    }
}
