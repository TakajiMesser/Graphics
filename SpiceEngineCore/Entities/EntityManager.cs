using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpiceEngineCore.Entities
{
    public class EntityManager : IEntityProvider
    {
        private List<IEntity> _entities = new List<IEntity>();

        private Dictionary<string, INamedEntity> _entitiesByName = new Dictionary<string, INamedEntity>();
        //private Dictionary<string, Archetype> _archetypeByName = new Dictionary<string, Archetype>();

        private ConcurrentDictionary<int, IEntityBuilder> _buildersByID = new ConcurrentDictionary<int, IEntityBuilder>();
        private ConcurrentQueue<Tuple<int, IEntityBuilder>> _builderIDQueue = new ConcurrentQueue<Tuple<int, IEntityBuilder>>();

        private ConcurrentQueue<INamedEntity> _namedEntityQueue = new ConcurrentQueue<INamedEntity>();
        private ConcurrentQueue<Tuple<string, ICamera>> _attachedCameraQueue = new ConcurrentQueue<Tuple<string, ICamera>>();

        private ConcurrentQueue<int> _removedIDs = new ConcurrentQueue<int>();
        private int _nextAvailableID = 1;

        private object _availableIDLock = new object();

        public List<ICamera> Cameras { get; } = new List<ICamera>();
        public List<IActor> Actors { get; } = new List<IActor>();
        public List<IBrush> Brushes { get; } = new List<IBrush>();
        public List<IVolume> Volumes { get; } = new List<IVolume>();
        public List<ILight> Lights { get; } = new List<ILight>();
        public List<IUIControl> Controls { get; } = new List<IUIControl>();

        public ILayerProvider LayerProvider { get; } = new LayerManager();

        public event EventHandler<EntityBuilderEventArgs> EntitiesAdded;
        public event EventHandler<IDEventArgs> EntitiesRemoved;

        public void ClearEntities()
        {
            _entities.Clear();
            _entitiesByName.Clear();
            _removedIDs.Clear();

            lock (_availableIDLock)
            {
                _nextAvailableID = 1;
            }

            Cameras.Clear();
            Actors.Clear();
            Brushes.Clear();
            Volumes.Clear();
            Lights.Clear();
            Controls.Clear();
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

        public INamedEntity GetEntity(string name)
        {
            if (_entitiesByName.TryGetValue(name, out INamedEntity value))
            {
                return value;
            }

            throw new KeyNotFoundException("No entity found for name " + name);
        }

        public IEntity GetEntityOrDefault(int id) => id <= _entities.Count ? _entities[id - 1] : null;

        public IEnumerable<IEntity> GetEntities(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                yield return GetEntity(id);
            }
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

        public void ClearLayer(string layerName)
        {
            // TODO - We need to ensure that these entity ID's are marked for deletion but maybe not yet deleted,
            //        because the RenderManager could be in the middle of drawing a batch
            foreach (var id in LayerProvider.GetLayerEntityIDs(layerName))
            {
                RemoveEntityByID(id);
                LayerProvider.RemoveFromLayer(LayerManager.ROOT_LAYER_NAME, id);
            }

            LayerProvider.ClearLayer(layerName);
        }

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
                            availableID = _nextAvailableID;
                            _nextAvailableID += entityBuilders.Count() - index;

                            _entities.PadTo(null, _nextAvailableID);
                        }
                    }
                }

                // Recheck if we set a valid available ID
                if (availableID > 0)
                {
                    id = availableID;
                    availableID++;
                }

                LayerProvider.AddToLayer(LayerManager.ROOT_LAYER_NAME, id);
                _buildersByID.TryAdd(id, entityBuilder);
                //_builderIDQueue.Enqueue(Tuple.Create(id, entityBuilder));
                yield return id;
            }
        }

        public void LoadEntity(int id)
        {
            if (_buildersByID.TryGetValue(id, out IEntityBuilder builder))
            {
                var entity = builder.ToEntity();
                entity.ID = id;

                //LayerProvider.AddToLayer(LayerManager.ROOT_LAYER_NAME, id);
                _entities[id - 1] = entity;

                if (entity is INamedEntity namedEntity)
                {
                    if (string.IsNullOrEmpty(namedEntity.Name)) throw new ArgumentException("Named entities must have a name defined");
                    if (_entitiesByName.ContainsKey(namedEntity.Name)) throw new ArgumentException("Named entities must have a unique name");

                    _namedEntityQueue.Enqueue(namedEntity);
                }

                if (entity is ICamera camera && builder is IMapCamera mapCamera && !string.IsNullOrEmpty(mapCamera.AttachedEntityName))
                {
                    _attachedCameraQueue.Enqueue(Tuple.Create(mapCamera.AttachedEntityName, camera));
                }

                AddToList(entity);
            }
        }

        public void Load()
        {
            if (_namedEntityQueue.TryDequeue(out INamedEntity namedEntity))
            {
                _entitiesByName.Add(namedEntity.Name, namedEntity);
            }

            if (_attachedCameraQueue.TryDequeue(out Tuple<string, ICamera> nameAndCamera))
            {
                var attachedEntity = GetEntity(nameAndCamera.Item1);
                nameAndCamera.Item2.AttachToEntity(attachedEntity, true, false);
            }

            /*while (_builderIDQueue.TryDequeue(out Tuple<int, IEntityBuilder> builderID))
            {
                var id = builderID.Item1;
                var entity = builderID.Item2.ToEntity();

                LayerProvider.AddToLayer(LayerManager.ROOT_LAYER_NAME, id);

                if (entity is INamedEntity namedEntity)
                {
                    if (string.IsNullOrEmpty(namedEntity.Name)) throw new ArgumentException("Named entities must have a name defined");
                    if (_entitiesByName.ContainsKey(namedEntity.Name)) throw new ArgumentException("Named entities must have a unique name");

                    _entitiesByName[namedEntity.Name] = namedEntity;
                }

                if ()

                AddToList(entity);
            }*/
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

            LayerProvider.AddToLayer(LayerManager.ROOT_LAYER_NAME, entity.ID);

            if (entity is INamedEntity namedEntity)
            {
                if (string.IsNullOrEmpty(namedEntity.Name)) throw new ArgumentException("Named entities must have a name defined");
                if (_entitiesByName.ContainsKey(namedEntity.Name)) throw new ArgumentException("Named entities must have a unique name");

                _namedEntityQueue.Enqueue(namedEntity);
            }

            AddToList(entity);

            return entity.ID;
        }

        private void AddToList(IEntity entity)
        {
            switch (entity)
            {
                case ICamera camera:
                    Cameras.Add(camera);
                    break;
                case IActor actor:
                    Actors.Add(actor);
                    break;
                case IBrush brush:
                    Brushes.Add(brush);
                    break;
                case IVolume volume:
                    Volumes.Add(volume);
                    break;
                case ILight light:
                    Lights.Add(light);
                    break;
                case IUIControl control:
                    Controls.Add(control);
                    break;
            }
        }

        public IEntity DuplicateEntity(IEntity entity)
        {
            var duplicateEntity = EntityFactory.Duplicate(entity);

            if (entity is INamedEntity namedEntity && duplicateEntity is INamedEntity duplicateNamedEntity)
            {
                var name = GetUniqueName(namedEntity.Name);
                duplicateNamedEntity.Name = name;
            }

            AddEntity(duplicateEntity);
            return duplicateEntity;
        }

        public void RemoveEntityByID(int id)
        {
            var entity = GetEntity(id);
            _entities[id - 1] = null;

            lock (_availableIDLock)
            {
                _removedIDs.Enqueue(id);
            }

            switch (entity)
            {
                case ICamera camera:
                    Cameras.Remove(camera);
                    break;
                case IActor actor:
                    Actors.Remove(actor);
                    break;
                case IBrush brush:
                    Brushes.Remove(brush);
                    break;
                case IVolume volume:
                    Volumes.Remove(volume);
                    break;
                case ILight light:
                    Lights.Remove(light);
                    break;
                case IUIControl control:
                    Controls.Remove(control);
                    break;
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
            if (_entitiesByName.ContainsKey(name))
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
