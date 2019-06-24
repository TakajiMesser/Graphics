using OpenTK;
using SpiceEngine.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Game
{
    public enum SelectionTypes
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    public class SelectionManager
    {
        private IEntityProvider _entityProvider;
        private ConcurrentDictionary<int, bool> _selectedByID = new ConcurrentDictionary<int, bool>();

        public SelectionManager(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public IEnumerable<int> IDs => _selectedByID.Keys;
        public IEnumerable<int> SelectedIDs => _selectedByID.Where(kvp => kvp.Value).Select(kvp => kvp.Key);

        public IEnumerable<IEntity> Entities => _selectedByID.Keys.Select(i => _entityProvider.GetEntity(i));
        public IEnumerable<IEntity> SelectedEntities => _selectedByID.Where(kvp => kvp.Value).Select(kvp => _entityProvider.GetEntity(kvp.Key));

        public SelectionTypes SelectionType { get; set; }

        public int Count => _selectedByID.Count;
        public int SelectionCount => _selectedByID.Count(kvp => kvp.Value);

        public Vector3 Position
        {
            get
            {
                var positions = _selectedByID.Where(kvp => kvp.Value).Select(kvp => _entityProvider.GetEntity(kvp.Key).Position);

                return new Vector3()
                {
                    X = positions.Average(p => p.X),
                    Y = positions.Average(p => p.Y),
                    Z = positions.Average(p => p.Z)
                };
            }
        }

        public void SetSelectable(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                _selectedByID.AddOrUpdate(id, false, (i, b) => b);
            }
        }

        public void Select(int id) => _selectedByID.AddOrUpdate(id, true, (i, b) => true);

        public void Select(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                _selectedByID.AddOrUpdate(id, true, (i, b) => true);
            }
        }

        public void Deselect(int id) => _selectedByID.TryUpdate(id, false, true);

        public bool IsSelectable(int id) => _selectedByID.ContainsKey(id);

        public bool IsSelected(int id) => _selectedByID.TryGetValue(id, out bool value) && value;

        public void Remove(int id) => _selectedByID.TryRemove(id, out bool value);

        public void SelectAll()
        {
            foreach (var id in _selectedByID.Keys)
            {
                _selectedByID.TryUpdate(id, true, false);
            }
        }

        public void Clear() => _selectedByID.Clear();

        public void ClearSelection()
        {
            foreach (var id in _selectedByID.Keys)
            {
                _selectedByID.TryUpdate(id, false, true);
            }
        }
    }
}
