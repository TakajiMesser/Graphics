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
        public List<IEntity> Entities { get; } = new List<IEntity>();

        public List<Actor> Actors { get; } = new List<Actor>();
        public List<Brush> Brushes { get; } = new List<Brush>();
        public List<Volume> Volumes { get; } = new List<Volume>();
        public List<Light> Lights { get; } = new List<Light>();

        public EntityManager() { }

        public void ClearEntities()
        {
            Entities.Clear();

            Actors.Clear();
            Brushes.Clear();
            Volumes.Clear();
            Lights.Clear();
        }

        public IEntity GetEntityByID(int id)
        {
            if (id > Entities.Count) throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
            return Entities[id - 1];
        }

        public Actor GetActorByName(string name)
        {
            var actor = Actors.FirstOrDefault(a => a.Name == name);
            if (actor == null) throw new KeyNotFoundException("No actor found for name " + name);

            return actor;
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
                Entities.Add(entity);
                entity.ID = Entities.Count;
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

        // Questionable method...
        public void Initialize()
        {
            foreach (var actor in Actors)
            {
                actor.OnInitialization();
            }
        }
    }
}
