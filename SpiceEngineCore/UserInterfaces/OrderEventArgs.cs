using System;
using System.Collections.Generic;

namespace SpiceEngineCore.UserInterfaces
{
    public class OrderEventArgs : EventArgs
    {
        public OrderEventArgs(IList<int> ids) => IDs = ids;

        public IList<int> IDs { get; }
    }
}
