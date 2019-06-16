using OpenTK;
using SpiceEngine.Entities;
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
        private class EntitySelection
        {
            public IEntity Entity { get; set; }
            public bool IsSelected { get; set; }

            public EntitySelection(IEntity entity) => Entity = entity;
        }

        private List<EntitySelection> _entities = new List<EntitySelection>();

        public IEnumerable<IEntity> Entities => _entities.Select(e => e.Entity);
        public IEnumerable<IEntity> SelectedEntities => _entities.Where(e => e.IsSelected).Select(e => e.Entity);
        public IEnumerable<int> IDs => _entities.Select(e => e.Entity.ID);
        public IEnumerable<int> SelectedIDs => _entities.Where(e => e.IsSelected).Select(e => e.Entity.ID);
        public SelectionTypes SelectionType { get; set; }

        public int Count => _entities.Count;
        public int SelectionCount => _entities.Count(e => e.IsSelected);

        public Vector3 Position
        {
            get
            {
                var positions = _entities.Select(e => e.Entity.Position);

                return new Vector3()
                {
                    X = positions.Average(p => p.X),
                    Y = positions.Average(p => p.Y),
                    Z = positions.Average(p => p.Z)
                };
            }
        }

        public void SetSelectableEntities(IEnumerable<IEntity> entities) => _entities = entities.Select(e => new EntitySelection(e)).ToList();

        public void SelectEntity(IEntity entity)
        {
            if (entity != null)
            {
                _entities.First(e => e.Entity.ID == entity.ID).IsSelected = true;
            }
            else
            {
                ClearSelection();
            }
        }

        public void SelectEntities(IEnumerable<IEntity> entities)
        {
            var ids = new HashSet<int>(entities.Select(e => e.ID));

            foreach (var entity in _entities)
            {
                entity.IsSelected = ids.Contains(entity.Entity.ID);
            }
        }

        public void DeselectEntity(IEntity entity) => _entities.First(e => e.Entity.ID == entity.ID).IsSelected = false;

        public void DeselectEntity(int id) => _entities.First(e => e.Entity.ID == id).IsSelected = false;

        public bool Contains(int id) => _entities.Select(e => e.Entity.ID).Contains(id);

        public bool IsSelected(int id) => _entities.First(e => e.Entity.ID == id).IsSelected;

        public void Add(IEntity entity) => _entities.Add(new EntitySelection(entity));

        public void Remove(IEntity entity) => _entities.Remove(_entities.FirstOrDefault(e => e.Entity == entity));

        public void Remove(int id) => _entities.Remove(_entities.FirstOrDefault(e => e.Entity.ID == id));

        public void SelectAll()
        {
            foreach (var entity in _entities)
            {
                entity.IsSelected = true;
            }
        }

        public void Clear() => _entities.Clear();

        public void ClearSelection()
        {
            foreach (var entity in _entities)
            {
                entity.IsSelected = false;
            }
        }
    }
}
