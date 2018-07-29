using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Maps;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Entities
{
    public class EntityManager
    {
        public List<Actor> Actors { get; } = new List<Actor>();
        public List<Brush> Brushes { get; } = new List<Brush>();
        public List<Volume> Volumes { get; } = new List<Volume>();
        public List<Light> Lights { get; } = new List<Light>();

        private int _nextAvailableID = 1;

        public EntityManager() { }

        public void ClearEntities()
        {
            Actors.Clear();
            Brushes.Clear();
            Volumes.Clear();
            Lights.Clear();
        }

        public IEntity GetEntityByID(int id)
        {
            var actor = Actors.FirstOrDefault(g => g.ID == id);
            if (actor != null)
            {
                return actor;
            }
            else
            {
                var brush = Brushes.FirstOrDefault(b => b.ID == id);
                if (brush != null)
                {
                    return brush;
                }
                else
                {
                    var volume = Volumes.FirstOrDefault(v => v.ID == id);
                    if (volume != null)
                    {
                        return volume;
                    }
                    else
                    {
                        var light = Lights.FirstOrDefault(l => l.ID == id);
                        if (light != null)
                        {
                            return light;
                        }
                        else
                        {
                            throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
                        }
                    }
                }
            }
        }

        public void AddEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                AddEntity(entity);
            }
        }

        public int AddEntity(IEntity entity)
        {
            // Assign a unique ID
            if (entity.ID == 0)
            {
                entity.ID = _nextAvailableID;
                _nextAvailableID++;

                if (_nextAvailableID == SelectionRenderer.RED_ID || _nextAvailableID == SelectionRenderer.GREEN_ID || _nextAvailableID == SelectionRenderer.BLUE_ID
                    || _nextAvailableID == SelectionRenderer.CYAN_ID || _nextAvailableID == SelectionRenderer.MAGENTA_ID || _nextAvailableID == SelectionRenderer.YELLOW_ID)
                {
                    _nextAvailableID++;
                }
            }

            switch (entity)
            {
                case Actor actor:
                    if (string.IsNullOrEmpty(actor.Name)) throw new ArgumentException("Actor must have a name defined");
                    if (Actors.Any(g => g.Name == actor.Name)) throw new ArgumentException("Actor must have a unique name");
                    Actors.Add(actor);
                    break;
                case Brush brush:
                    Brushes.Add(brush);
                    break;
                case Volume volume:
                    Volumes.Add(volume);
                    break;
                case Light light:
                    Lights.Add(light);
                    break;
            }

            return entity.ID;
        }

        public void RemoveEntityByID(int id)
        {
            var entity = GetEntityByID(id);

            switch (entity)
            {
                case Actor actor:
                    Actors.Remove(actor);
                    break;
                case Brush brush:
                    Brushes.Remove(brush);
                    break;
                case Volume volume:
                    Volumes.Remove(volume);
                    break;
                case Light light:
                    Lights.Remove(light);
                    break;
            }
        }

        public void LoadEntities()
        {
            foreach (var actor in Actors)
            {
                actor.Load();
            }

            foreach (var brush in Brushes)
            {
                brush.Load();
            }

            foreach (var volume in Volumes)
            {
                volume.Load();
            }
        }

        public void LoadFromMap(Map map)
        {
            AddEntities(map.Lights);
            AddEntities(map.Brushes.Select(b => b.ToBrush()));
            AddEntities(map.Volumes.Select(v => v.ToVolume()));
            AddEntities(map.Actors.Select(a => a.ToActor()));
        }
    }
}
