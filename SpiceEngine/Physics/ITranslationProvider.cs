using System.Collections.Generic;

namespace SpiceEngine.Physics
{
    public interface ITranslationProvider
    {
        IEnumerable<EntityTranslation> EntityTranslations { get; }
    }
}
