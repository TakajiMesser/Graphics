using SpiceEngineCore.Game.Loading.Builders;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Game.Loading
{

    public class EntityBuilderEventArgs : EventArgs
    {
        public IEnumerable<Tuple<int, IEntityBuilder>> Builders { get; private set; }

        public EntityBuilderEventArgs(IEnumerable<Tuple<int, IEntityBuilder>> builders) => Builders = builders;
    }
}
