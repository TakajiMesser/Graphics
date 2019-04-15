using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpiceEngine.Entities
{
    public enum EntityTypes
    {
        Actor,
        Brush,
        Volume,
        Joint,
        Light
    }

    public class EntityManager : IEntityProvider
    {
        private LayerManager _layerManager = new LayerManager();
        
        private Dictionary<int, IEntity> _entitiesByID = new Dictionary<int, IEntity>();
        private Dictionary<int, EntityTypes> _entityTypeByID = new Dictionary<int, EntityTypes>();
        private Dictionary<string, Archetype> _archetypeByName = new Dictionary<string, Archetype>();
        private int _nextAvailableID = 1;

        public List<Actor> Actors { get; } = new List<Actor>();
        public List<Brush> Brushes { get; } = new List<Brush>();
        public List<Volume> Volumes { get; } = new List<Volume>();
        public List<ILight> Lights { get; } = new List<ILight>();

        public IEnumerable<int> EntityRenderIDs => _layerManager.EntityRenderIDs;
        public IEnumerable<int> EntityScriptIDs => _layerManager.EntityScriptIDs;
        public IEnumerable<int> EntityPhysicsIDs => _layerManager.EntityPhysicsIDs;

        public EntityManager() { }

        public void ClearEntities()
        {
            _entitiesByID.Clear();
            _entityTypeByID.Clear();
            _nextAvailableID = 1;

            Actors.Clear();
            Brushes.Clear();
            Volumes.Clear();
            Lights.Clear();
        }

        public IEntity GetEntity(int id)
        {
            if (!_entitiesByID.ContainsKey(id)) throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
            return _entitiesByID[id];
        }

        public EntityTypes GetEntityType(int id)
        {
            if (!_entityTypeByID.ContainsKey(id)) throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
            return _entityTypeByID[id];
        }

        public Actor GetActor(string name)
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
                int id = GetUniqueID();
                _entitiesByID.Add(id, entity);
                entity.ID = id;
            }

            _layerManager.RootLayer.Add(entity.ID);

            switch (entity)
            {
                case Actor actor:
                    if (string.IsNullOrEmpty(actor.Name)) throw new ArgumentException("Actor must have a name defined");
                    if (Actors.Any(g => g.Name == actor.Name)) throw new ArgumentException("Actor must have a unique name");
                    Actors.Add(actor);
                    _entityTypeByID.Add(entity.ID, actor is AnimatedActor ? EntityTypes.Joint : EntityTypes.Actor);
                    break;
                case Brush brush:
                    Brushes.Add(brush);
                    _entityTypeByID.Add(entity.ID, EntityTypes.Brush);
                    break;
                case Volume volume:
                    Volumes.Add(volume);
                    _entityTypeByID.Add(entity.ID, EntityTypes.Volume);
                    break;
                case ILight light:
                    Lights.Add(light);
                    _entityTypeByID.Add(entity.ID, EntityTypes.Light);
                    break;
            }

            return entity.ID;
        }

        public IEntity DuplicateEntity(IEntity entity)
        {
            IEntity duplicateEntity = null;

            switch (entity)
            {
                case Actor actor:
                    var name = GetUniqueName(actor.Name);
                    duplicateEntity = actor.Duplicate(name);
                    break;
                case Brush brush:
                    duplicateEntity = brush.Duplicate();
                    break;
                case Volume volume:
                    duplicateEntity = volume.Duplicate();
                    break;
                case ILight light:
                    /*duplicateEntity = new Light()
                    {
                        Position = light.Position,
                        Rotation = light.Rotation,
                        Scale = light.Scale,
                    };*/
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
                case ILight light:
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

        private int GetUniqueID()
        {
            int id = _nextAvailableID;
            _nextAvailableID++;

            return id;
        }

        private string GetUniqueName(string name)
        {
            // Check if this name is already taken
            if (Actors.Any(a => a.Name == name))
            {
                var regex = new Regex("(?<Name>.+)_(?<Number>[0-9]+$)");
                var match = regex.Match(name);

                if (match.Success)
                {
                    // Increment "_n" and try again
                    var number = int.Parse(match.Groups["Number"].Value);
                    return GetUniqueName(match.Groups["Name"].Value + "_" + (number + 1));
                }
                else
                {
                    // Append "_2" and try again
                    return GetUniqueName(name + "_2");
                }
            }
            else
            {
                return name;
            }
        }
    }
}
