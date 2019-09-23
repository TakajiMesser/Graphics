using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Entities.Builders;
using SpiceEngine.Physics;
using SpiceEngine.Rendering;
using SpiceEngine.Scripting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using SpiceEngine.Utilities;
using SpiceEngine.Rendering.PostProcessing;

namespace SpiceEngine.Entities.Builders
{
    public interface IEntityLoader<T> where T : IBuilder
    {
        void AddEntity(int id, T builder);
        void Load();
    }
}
