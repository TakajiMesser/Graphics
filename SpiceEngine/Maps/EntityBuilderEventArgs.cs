using OpenTK;

namespace SpiceEngine.Maps
{
    public class EntityBuilderEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; private set; }
        public IEnumerable<IEntityBuilder> Builders { get; private set; }

        public EntityBuilderEventArgs(IEnumerable<int> ids, IEnumerable<IEntityBuilder> builders)
        {
            IDs = ids;
            Builders = builders;
        }
    }
}
