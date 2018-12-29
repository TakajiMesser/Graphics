using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics
{
    public interface ITranslationProvider
    {
        IEnumerable<EntityTranslation> EntityTranslations { get; }
    }
}
