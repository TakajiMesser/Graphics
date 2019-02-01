using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Entities
{
    public class EntityManager : IEntityProvider
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

        public IEntity GetEntity(int id)
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

        private int GetUniqueID()
        {

        }

        private string GetUniqueName(string name)
        {
             
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

        public IEntity DuplicateEntity(IEntity entity)
        {
            IEntity duplicateEntity;

            switch (entity)
            {
                case Actor actor:
                    var name = GetUniqueName(entity.Name);
                    duplicateEntity = new Actor(name)
                    {
                        Position = entity.Position,
                        Rotation = entity.Rotation,
                        Scale = entity.Scale,
                        Orientation = entity.Orientation
                    };
                    break;
                case Brush brush:
                    duplicateEntity = new Brush()
                    {
                        Position = entity.Position,
                        Rotation = entity.Rotation,
                        Scale = entity.Scale,
                    };
                    break;
                case Volume volume:
                    duplicateEntity = new Volume()
                    {
                        Position = entity.Position,
                        Rotation = entity.Rotation,
                        Scale = entity.Scale,
                    };
                    break;
                case Light light:
                    duplicateEntity = new Light()
                    {
                        Position = entity.Position,
                        Rotation = entity.Rotation,
                        Scale = entity.Scale,
                    };
                    break;
            }

            AddEntity(duplicateEntity);
            return duplicateEntity;
        }

        public void RemoveEntityByID(int id)
        {
            var entity = GetEntity(id);

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
                //actor.Load();
            }

            foreach (var brush in Brushes)
            {
                //brush.Load();
            }

            foreach (var volume in Volumes)
            {
                //volume.Load();
            }
        }
    }
}
