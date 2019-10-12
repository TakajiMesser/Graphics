using SauceEditorCore.Models.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Helpers;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Components
{
    public class MapComponent : Component
    {
        public MapComponent() {}
        public MapComponent(string filePath) : base(filePath) {}

        /*private enum MapEntityType
        {
            Actor,
            Brush,
            Volume,
            Light
        }

        private Dictionary<int, MapEntityType> _entityTypeByEntityID = new Dictionary<int, MapEntityType>();*/

        private int _mapActorIndexCount;
        private int _mapBrushIndexCount;
        private int _mapVolumeIndexCount;
        private int _mapLightIndexCount;

        private BidirectionalDictionary<int, int> _mapActorIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapBrushIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapVolumeIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapLightIndexByEntityID = new BidirectionalDictionary<int, int>();

        public Map Map { get; set; }

        public void SetEntityMap(EntityMap entityMap)
        {
            foreach (var id in entityMap.ActorIDs)
            {
                _mapActorIndexByEntityID.Add(id, _mapActorIndexCount);
                _mapActorIndexCount++;
            }

            foreach (var id in entityMap.BrushIDs)
            {
                _mapBrushIndexByEntityID.Add(id, _mapBrushIndexCount);
                _mapBrushIndexCount++;
            }

            foreach (var id in entityMap.VolumeIDs)
            {
                _mapVolumeIndexByEntityID.Add(id, _mapVolumeIndexCount);
                _mapVolumeIndexCount++;
            }

            foreach (var id in entityMap.LightIDs)
            {
                _mapLightIndexByEntityID.Add(id, _mapLightIndexCount);
                _mapLightIndexCount++;
            }
        }

        public void AddMapActor(MapActor mapActor, int entityID)
        {
            Map.Actors.Add(mapActor);
            var index = Map.Actors.Count - 1;

            _mapActorIndexByEntityID.Add(entityID, index);
        }

        public void AddMapBrush(MapBrush mapBrush, int entityID)
        {
            Map.Brushes.Add(mapBrush);
            var index = Map.Brushes.Count - 1;

            _mapBrushIndexByEntityID.Add(entityID, index);
        }

        public void AddMapVolume(MapVolume mapVolume, int entityID)
        {
            Map.Volumes.Add(mapVolume);
            var index = Map.Volumes.Count - 1;

            _mapVolumeIndexByEntityID.Add(entityID, index);
        }

        public void AddMapLight(MapLight light, int entityID)
        {
            Map.Lights.Add(light);
            var index = Map.Lights.Count - 1;

            _mapLightIndexByEntityID.Add(entityID, index);
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case Actor actor:
                        UpdateMapActor(actor);
                        break;
                    case Brush brush:
                        UpdateMapBrush(brush);
                        break;
                    case Volume volume:
                        UpdateMapVolume(volume);
                        break;
                    case ILight light:
                        UpdateMapLight(light);
                        break;
                }
            }
        }

        private void UpdateMapActor(Actor actor)
        {
            var index = _mapActorIndexByEntityID.GetValue(actor.ID);
            var mapActor = Map.Actors[index];

            mapActor.UpdateFrom(actor);
        }

        private void UpdateMapBrush(Brush brush)
        {
            var index = _mapBrushIndexByEntityID.GetValue(brush.ID);
            var mapBrush = Map.Brushes[index];

            mapBrush.UpdateFrom(brush);
        }

        private void UpdateMapVolume(Volume volume)
        {
            var index = _mapVolumeIndexByEntityID.GetValue(volume.ID);
            var mapVolume = Map.Volumes[index];

            mapVolume.UpdateFrom(volume);
        }

        private void UpdateMapLight(ILight light)
        {
            var index = _mapLightIndexByEntityID.GetValue(light.ID);
            var mapLight = Map.Lights[index];

            mapLight.UpdateFrom(light);
        }

        public IEnumerable<MapEntityID> GetMapActorEntityIDs()
        {
            for (var i = 0; i < Map.Actors.Count; i++)
            {
                var entityID = _mapActorIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.Actors[i])
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapBrushEntityIDs()
        {
            for (var i = 0; i < Map.Brushes.Count; i++)
            {
                var entityID = _mapBrushIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.Brushes[i])
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapVolumeEntityIDs()
        {
            for (var i = 0; i < Map.Volumes.Count; i++)
            {
                var entityID = _mapVolumeIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.Volumes[i])
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapLightEntityIDs()
        {
            for (var i = 0; i < Map.Lights.Count; i++)
            {
                var entityID = _mapLightIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.Lights[i])
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<EditorEntity> GetEditorEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case Actor actor:
                        var mapActor = GetMapActor(actor.ID);
                        yield return new EditorEntity(actor, mapActor);
                        break;
                    case Brush brush:
                        var mapBrush = GetMapBrush(brush.ID);
                        yield return new EditorEntity(brush, mapBrush);
                        break;
                    case Volume volume:
                        var mapVolume = GetMapVolume(volume.ID);
                        yield return new EditorEntity(volume, mapVolume);
                        break;
                    case ILight light:
                        var mapLight = GetMapLight(light.ID);
                        yield return new EditorEntity(light, mapLight);
                        break;
                }
            }
        }

        public MapCamera GetMapCamera() => Map.Camera;

        public MapActor GetMapActor(int entityID)
        {
            var index = _mapActorIndexByEntityID.GetValue(entityID);
            return Map.Actors[index];
        }

        public MapBrush GetMapBrush(int entityID)
        {
            var index = _mapBrushIndexByEntityID.GetValue(entityID);
            return Map.Brushes[index];
        }

        public MapVolume GetMapVolume(int entityID)
        {
            var index = _mapVolumeIndexByEntityID.GetValue(entityID);
            return Map.Volumes[index];
        }

        public MapLight GetMapLight(int entityID)
        {
            var index = _mapLightIndexByEntityID.GetValue(entityID);
            return Map.Lights[index];
        }

        public override void Save() => Map.Save(Path);
        public override void Load() => Map = Map.Load(Path);

        public static bool IsValidExtension(string extension) => extension == ".map";
    }
}
