using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Builders;
using SpiceEngine.Entities.Layers;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngine.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpiceEngine.Entities
{
    public class EntityManager : IEntityProvider
    {
        private LayerManager _layerManager = new LayerManager();

        private List<IEntity> _entities = new List<IEntity>();
        private List<EntityTypes?> _entityTypes = new List<EntityTypes?>();
        //private Dictionary<int, IEntity> _entitiesByID = new Dictionary<int, IEntity>();
        //private Dictionary<int, EntityTypes> _entityTypeByID = new Dictionary<int, EntityTypes>();
        private Dictionary<string, Archetype> _archetypeByName = new Dictionary<string, Archetype>();

        private ConcurrentDictionary<int, IEntityBuilder> _buildersByID = new ConcurrentDictionary<int, IEntityBuilder>();
        private ConcurrentQueue<Tuple<int, IEntityBuilder>> _builderIDQueue = new ConcurrentQueue<Tuple<int, IEntityBuilder>>();
        private ConcurrentQueue<int> _removedIDs = new ConcurrentQueue<int>();
        private int _nextAvailableID = 1;

        private object _availableIDLock = new object();

        public List<Actor> Actors { get; } = new List<Actor>();
        public List<Brush> Brushes { get; } = new List<Brush>();
        public List<Volume> Volumes { get; } = new List<Volume>();
        public List<ILight> Lights { get; } = new List<ILight>();

        public IEnumerable<int> EntityRenderIDs => _layerManager.EntityRenderIDs;
        public IEnumerable<int> EntityScriptIDs => _layerManager.EntityScriptIDs;
        public IEnumerable<int> EntityPhysicsIDs => _layerManager.EntityPhysicsIDs;
        public IEnumerable<int> EntitySelectIDs => _layerManager.EntitySelectIDs;

        public event EventHandler<EntityBuilderEventArgs> EntitiesAdded;
        public event EventHandler<IDEventArgs> EntitiesRemoved;

        public void ClearEntities()
        {
            _entities.Clear();
            _entityTypes.Clear();
            _removedIDs.Clear();

            lock (_availableIDLock)
            {
                _nextAvailableID = 1;
            }

            Actors.Clear();
            Brushes.Clear();
            Volumes.Clear();
            Lights.Clear();
        }

        public IEntity GetEntity(int id)
        {
            if (id <= _entities.Count)
            {
                var entity = _entities[id - 1];
                if (entity != null)
                {
                    return entity;
                }
            }

            throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
        }

        public IEntity GetEntityOrDefault(int id) => id <= _entities.Count ? _entities[id - 1] : null;

        public IEnumerable<IEntity> GetEntities(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                yield return GetEntity(id);
            }
        }

        public EntityTypes GetEntityType(int id)
        {
            if (id <= _entityTypes.Count)
            {
                var entityType = _entityTypes[id - 1];
                if (entityType.HasValue)
                {
                    return entityType.Value;
                }
            }

            throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
        }

        public Actor GetActor(string name)
        {
            var actor = Actors.FirstOrDefault(a => a.Name == name);
            if (actor == null) throw new KeyNotFoundException("No actor found for name " + name);

            return actor;
        }

        public void AddEntities(IEnumerable<IEntityBuilder> entityBuilders)
        {
            var entities = new List<Tuple<int, IEntityBuilder>>();

            foreach (var entityBuilder in entityBuilders)
            {
                var id = AddEntity(entityBuilder.ToEntity());
                entities.Add(Tuple.Create(id, entityBuilder));
            }

            EntitiesAdded?.Invoke(this, new EntityBuilderEventArgs(entities));
        }

        public void AddEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                AddEntity(entity);
            }

            //EntitiesAdded?.Invoke(this, new IDEventArgs(entities.Select(e => e.ID)));
        }

        public void AddLayer(string layerName) => _layerManager.AddLayer(layerName);

        public bool ContainsLayer(string layerName) => _layerManager.ContainsLayer(layerName);

        public void AddEntitiesToLayer(string layerName, IEnumerable<int> entityIDs)
        {
            foreach (var id in entityIDs)
            {
                _layerManager.AddToLayer(layerName, id);
            }
        }

        public IEnumerable<int> GetLayerEntityIDs(string layerName) => _layerManager.GetLayerEntityIDs(layerName);

        public void SetLayerState(string name, LayerStates state)
        {
            _layerManager.SetPhysicsLayerState(name, state);
            _layerManager.SetRenderLayerState(name, state);
            _layerManager.SetScriptLayerState(name, state);
            _layerManager.SetSelectLayerState(name, state);
        }

        public void ClearLayer(string layerName)
        {
            // TODO - We need to ensure that these entity ID's are marked for deletion but maybe not yet deleted,
            //        because the RenderManager could be in the middle of drawing a batch
            foreach (var id in _layerManager.GetLayerEntityIDs(layerName))
            {
                RemoveEntityByID(id);
                _layerManager.RootLayer.Remove(id);
            }

            _layerManager.ClearLayer(layerName);
        }

        public void SetRenderLayerState(string name, LayerStates state) => _layerManager.SetRenderLayerState(name, state);

        public void SetSelectLayerState(string name, LayerStates state) => _layerManager.SetSelectLayerState(name, state);

        public int AddEntity(IEntityBuilder entityBuilder) => AddEntity(entityBuilder.ToEntity());

        public IEnumerable<int> AssignEntityIDs(IEnumerable<IEntityBuilder> entityBuilders)
        {
            var availableID = 0;
            var index = 0;

            foreach (var entityBuilder in entityBuilders)
            {
                var id = 0;

                if (availableID == 0)
                {
                    if (_removedIDs.TryDequeue(out int result))
                    {
                        id = result;
                        index++;
                    }
                    else
                    {
                        // The moment we fail to dequeue, we should increment nextAvailableID PAST where we need it for the remainder of this loop
                        lock (_availableIDLock)
                        {
                            var nReservedEntities = entityBuilders.Count() - index;
                            availableID = _nextAvailableID;
                            _nextAvailableID += nReservedEntities;

                            _entities.PadTo(null, nReservedEntities);
                            _entityTypes.PadTo(null, nReservedEntities);
                        }
                    }
                }

                // Recheck if we set a valid available ID
                if (availableID > 0)
                {
                    id = availableID;
                    availableID++;
                }

                _buildersByID.TryAdd(id, entityBuilder);
                //_builderIDQueue.Enqueue(Tuple.Create(id, entityBuilder));
                yield return id;
            }
        }

        public void LoadEntity(int id)
        {
            var a = 3;
            if (id == 13)
            {
                a = 4;
            }

            if (_buildersByID.TryGetValue(id, out IEntityBuilder builder))
            {
                var entity = builder.ToEntity();
                entity.ID = id;

                _layerManager.RootLayer.Add(id);
                _entities[id - 1] = entity;

                switch (entity)
                {
                    case Actor actor:
                        if (string.IsNullOrEmpty(actor.Name)) throw new ArgumentException("Actor must have a name defined");
                        if (Actors.Any(g => g.Name == actor.Name)) throw new ArgumentException("Actor must have a unique name");
                        Actors.Add(actor);
                        _entityTypes[entity.ID - 1] = actor is AnimatedActor ? EntityTypes.Joint : EntityTypes.Actor;
                        break;
                    case Brush brush:
                        Brushes.Add(brush);
                        _entityTypes[entity.ID - 1] = EntityTypes.Brush;
                        break;
                    case Volume volume:
                        Volumes.Add(volume);
                        _entityTypes[entity.ID - 1] = EntityTypes.Volume;
                        break;
                    case ILight light:
                        Lights.Add(light);
                        _entityTypes[entity.ID - 1] = EntityTypes.Light;
                        break;
                }
            }
        }

        public void Load()
        {
            while (_builderIDQueue.TryDequeue(out Tuple<int, IEntityBuilder> builderID))
            {
                var id = builderID.Item1;
                var entity = builderID.Item2.ToEntity();

                _layerManager.RootLayer.Add(id);

                switch (entity)
                {
                    case Actor actor:
                        if (string.IsNullOrEmpty(actor.Name)) throw new ArgumentException("Actor must have a name defined");
                        if (Actors.Any(g => g.Name == actor.Name)) throw new ArgumentException("Actor must have a unique name");
                        Actors.Add(actor);
                        _entityTypes[entity.ID - 1] = actor is AnimatedActor ? EntityTypes.Joint : EntityTypes.Actor;
                        break;
                    case Brush brush:
                        Brushes.Add(brush);
                        _entityTypes[entity.ID - 1] = EntityTypes.Brush;
                        break;
                    case Volume volume:
                        Volumes.Add(volume);
                        _entityTypes[entity.ID - 1] = EntityTypes.Volume;
                        break;
                    case ILight light:
                        Lights.Add(light);
                        _entityTypes[entity.ID - 1] = EntityTypes.Light;
                        break;
                }
            }
        }

        public int AddEntity(IEntity entity)
        {
            // Assign a unique ID
            if (entity.ID == 0)
            {
                int id = GetUniqueID();
                _entities[id - 1] = entity;
                entity.ID = id;
            }

            _layerManager.RootLayer.Add(entity.ID);

            switch (entity)
            {
                case Actor actor:
                    if (string.IsNullOrEmpty(actor.Name)) throw new ArgumentException("Actor must have a name defined");
                    if (Actors.Any(g => g.Name == actor.Name)) throw new ArgumentException("Actor must have a unique name");
                    Actors.Add(actor);
                    _entityTypes[entity.ID - 1] = actor is AnimatedActor ? EntityTypes.Joint : EntityTypes.Actor;
                    break;
                case Brush brush:
                    Brushes.Add(brush);
                    _entityTypes[entity.ID - 1] = EntityTypes.Brush;
                    break;
                case Volume volume:
                    Volumes.Add(volume);
                    _entityTypes[entity.ID - 1] = EntityTypes.Volume;
                    break;
                case ILight light:
                    Lights.Add(light);
                    _entityTypes[entity.ID - 1] = EntityTypes.Light;
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
            _entities[id - 1] = null;
            _entityTypes[id - 1] = null;
            //_entitiesByID.Remove(id);
            //_entityTypeByID.Remove(id);

            lock (_availableIDLock)
            {
                _removedIDs.Enqueue(id);
            }

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
            if (_removedIDs.TryDequeue(out int result))
            {
                return result;
            }
            else
            {
                lock (_availableIDLock)
                {
                    var id = _nextAvailableID;
                    _nextAvailableID++;
                    return id;
                }
            }
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
