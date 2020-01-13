using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Entities.UserInterfaces
{
    public class UIElement : Entity, IUIElement
    {
        public string Name { get; set; }
        public ICollection<int> ChildIDs { get; } = new List<int>();

        public event EventHandler<IDEventArgs> ChildrenAdded;
        public event EventHandler<IDEventArgs> ChildrenRemoved;

        public void AddChild(int id)
        {
            ChildIDs.Add(id);
            ChildrenAdded?.Invoke(this, new IDEventArgs(id));
        }

        public void RemoveChild(int id)
        {
            ChildIDs.Remove(id);
            ChildrenRemoved?.Invoke(this, new IDEventArgs(id));
        }

        public void ClearChildren()
        {
            var ids = ChildIDs.ToList();
            ChildIDs.Clear();

            ChildrenRemoved?.Invoke(this, new IDEventArgs(ids));
        }
    }
}
