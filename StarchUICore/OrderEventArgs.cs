using System;
using System.Collections.Generic;

namespace StarchUICore
{
    public class OrderEventArgs : EventArgs
    {
        public OrderEventArgs(IList<int> ids) => IDs = ids;

        public IList<int> IDs { get; }
    }
}
