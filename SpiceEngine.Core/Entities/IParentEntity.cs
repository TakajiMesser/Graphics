using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public interface IParentEntity : IEntity
    {
        ICollection<int> ChildIDs { get; }

        void AddChild(int entityID);
        void RemoveChild(int entityID);
        void ClearChildren();

        event EventHandler<IDEventArgs> ChildrenAdded;
        event EventHandler<IDEventArgs> ChildrenRemoved;
    }
}
