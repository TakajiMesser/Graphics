using OpenTK;
using SpiceEngine.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<IEntity> _selectedEntities = new List<IEntity>();

        public ReadOnlyCollection<IEntity> Entities => _selectedEntities.AsReadOnly();
        public SelectionTypes SelectionType { get; set; }

        public int Count => _selectedEntities.Count;
        public Vector3 Position
        {
            get
            {
                var positions = _selectedEntities.Select(e => e.Position);

                return new Vector3()
                {
                    X = positions.Average(p => p.X),
                    Y = positions.Average(p => p.Y),
                    Z = positions.Average(p => p.Z)
                };
            }
        }

        public void Set(IEnumerable<IEntity> entities) => _selectedEntities = entities.ToList();

        public void SelectEntity(IEntity entity)
        {
            if (entity != null)
            {
                _selectedEntities.Add(entity);
            }
            else
            {
                _selectedEntities.Clear();
            }
        }

        public void SelectEntities(IEnumerable<IEntity> entities)
        {
            _selectedEntities.Clear();
            _selectedEntities.AddRange(entities);
        }

        public bool Contains(int id) => _selectedEntities.Select(e => e.ID).Contains(id);

        public void Add(IEntity entity) => _selectedEntities.Add(entity);

        public void Remove(IEntity entity) => _selectedEntities.Remove(entity);

        public void Remove(int id)
        {
            var entity = _selectedEntities.First(e => e.ID == id);
            _selectedEntities.Remove(entity);
        }

        public void Clear() => _selectedEntities.Clear();
    }
}
