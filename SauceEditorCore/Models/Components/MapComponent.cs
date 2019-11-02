using SauceEditorCore.Models.Entities;
using SpiceEngine.Maps;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Components
{
    public class MapComponent : Component
    {
        public MapComponent() {}
        public MapComponent(string filePath) : base(filePath) {}

        private int _mapCameraIndexCount;
        private int _mapBrushIndexCount;
        private int _mapActorIndexCount;
        private int _mapLightIndexCount;
        private int _mapVolumeIndexCount;

        private BidirectionalDictionary<int, int> _mapCameraIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapBrushIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapActorIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapLightIndexByEntityID = new BidirectionalDictionary<int, int>();
        private BidirectionalDictionary<int, int> _mapVolumeIndexByEntityID = new BidirectionalDictionary<int, int>();

        public IMap Map { get; set; }

        public void ClearEntityMapping()
        {
            _mapCameraIndexCount = 0;
            _mapBrushIndexCount = 0;
            _mapActorIndexCount = 0;
            _mapLightIndexCount = 0;
            _mapVolumeIndexCount = 0;

            _mapCameraIndexByEntityID.Clear();
            _mapBrushIndexByEntityID.Clear();
            _mapActorIndexByEntityID.Clear();
            _mapLightIndexByEntityID.Clear();
            _mapVolumeIndexByEntityID.Clear();
        }

        public void SetEntityMapping(EntityMapping entityMapping)
        {
            foreach (var id in entityMapping.CameraIDs)
            {
                _mapCameraIndexByEntityID.Add(id, _mapCameraIndexCount);
                _mapCameraIndexCount++;
            }

            foreach (var id in entityMapping.BrushIDs)
            {
                _mapBrushIndexByEntityID.Add(id, _mapBrushIndexCount);
                _mapBrushIndexCount++;
            }

            foreach (var id in entityMapping.ActorIDs)
            {
                _mapActorIndexByEntityID.Add(id, _mapActorIndexCount);
                _mapActorIndexCount++;
            }

            foreach (var id in entityMapping.LightIDs)
            {
                _mapLightIndexByEntityID.Add(id, _mapLightIndexCount);
                _mapLightIndexCount++;
            }

            foreach (var id in entityMapping.VolumeIDs)
            {
                _mapVolumeIndexByEntityID.Add(id, _mapVolumeIndexCount);
                _mapVolumeIndexCount++;
            }
        }

        public void AddMapCamera(int entityID, IMapCamera mapCamera)
        {
            var index = Map.AddCamera(mapCamera);
            _mapCameraIndexByEntityID.Add(entityID, index);
        }

        public void AddMapBrush(int entityID, IMapBrush mapBrush)
        {
            var index = Map.AddBrush(mapBrush);
            _mapBrushIndexByEntityID.Add(entityID, index);
        }

        public void AddMapActor(int entityID, IMapActor mapActor)
        {
            var index = Map.AddActor(mapActor);
            _mapActorIndexByEntityID.Add(entityID, index);
        }

        public void AddMapLight(int entityID, IMapLight mapLight)
        {
            var index = Map.AddLight(mapLight);
            _mapLightIndexByEntityID.Add(entityID, index);
        }

        public void AddMapVolume(int entityID, IMapVolume mapVolume)
        {
            var index = Map.AddVolume(mapVolume);
            _mapVolumeIndexByEntityID.Add(entityID, index);
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case ICamera camera:
                        UpdateMapCamera(camera);
                        break;
                    case IBrush brush:
                        UpdateMapBrush(brush);
                        break;
                    case IActor actor:
                        UpdateMapActor(actor);
                        break;
                    case ILight light:
                        UpdateMapLight(light);
                        break;
                    case IVolume volume:
                        UpdateMapVolume(volume);
                        break;
                }
            }
        }

        private void UpdateMapCamera(ICamera camera)
        {
            var index = _mapCameraIndexByEntityID.GetValue(camera.ID);
            var mapCamera = Map.GetCameraAt(index);

            mapCamera.UpdateFrom(camera);
        }

        private void UpdateMapBrush(IBrush brush)
        {
            var index = _mapBrushIndexByEntityID.GetValue(brush.ID);
            var mapBrush = Map.GetBrushAt(index);

            mapBrush.UpdateFrom(brush);
        }

        private void UpdateMapActor(IActor actor)
        {
            var index = _mapActorIndexByEntityID.GetValue(actor.ID);
            var mapActor = Map.GetActorAt(index);

            mapActor.UpdateFrom(actor);
        }

        private void UpdateMapLight(ILight light)
        {
            var index = _mapLightIndexByEntityID.GetValue(light.ID);
            var mapLight = Map.GetLightAt(index);

            mapLight.UpdateFrom(light);
        }

        private void UpdateMapVolume(IVolume volume)
        {
            var index = _mapVolumeIndexByEntityID.GetValue(volume.ID);
            var mapVolume = Map.GetVolumeAt(index);

            mapVolume.UpdateFrom(volume);
        }

        public IEnumerable<MapEntityID> GetMapCameraEntityIDs()
        {
            for (var i = 0; i < Map.CameraCount; i++)
            {
                var entityID = _mapCameraIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.GetCameraAt(i))
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapBrushEntityIDs()
        {
            for (var i = 0; i < Map.BrushCount; i++)
            {
                var entityID = _mapBrushIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.GetBrushAt(i))
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapActorEntityIDs()
        {
            for (var i = 0; i < Map.ActorCount; i++)
            {
                var entityID = _mapActorIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.GetActorAt(i))
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapLightEntityIDs()
        {
            for (var i = 0; i < Map.LightCount; i++)
            {
                var entityID = _mapLightIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.GetLightAt(i))
                {
                    ID = entityID
                };
            }
        }

        public IEnumerable<MapEntityID> GetMapVolumeEntityIDs()
        {
            for (var i = 0; i < Map.VolumeCount; i++)
            {
                var entityID = _mapVolumeIndexByEntityID.GetKey(i);

                yield return new MapEntityID(Map.GetVolumeAt(i))
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
                    case ICamera camera:
                        var mapCamera = GetMapCamera(camera.ID);
                        yield return new EditorEntity(camera, mapCamera);
                        break;
                    case IBrush brush:
                        var mapBrush = GetMapBrush(brush.ID);
                        yield return new EditorEntity(brush, mapBrush);
                        break;
                    case IActor actor:
                        var mapActor = GetMapActor(actor.ID);
                        yield return new EditorEntity(actor, mapActor);
                        break;
                    case ILight light:
                        var mapLight = GetMapLight(light.ID);
                        yield return new EditorEntity(light, mapLight);
                        break;
                    case IVolume volume:
                        var mapVolume = GetMapVolume(volume.ID);
                        yield return new EditorEntity(volume, mapVolume);
                        break;
                }
            }
        }

        public IMapCamera GetMapCamera(int entityID)
        {
            var index = _mapCameraIndexByEntityID.GetValue(entityID);
            return Map.GetCameraAt(index);
        }

        public IMapActor GetMapActor(int entityID)
        {
            var index = _mapActorIndexByEntityID.GetValue(entityID);
            return Map.GetActorAt(index);
        }

        public IMapBrush GetMapBrush(int entityID)
        {
            var index = _mapBrushIndexByEntityID.GetValue(entityID);
            return Map.GetBrushAt(index);
        }

        public IMapVolume GetMapVolume(int entityID)
        {
            var index = _mapVolumeIndexByEntityID.GetValue(entityID);
            return Map.GetVolumeAt(index);
        }

        public IMapLight GetMapLight(int entityID)
        {
            var index = _mapLightIndexByEntityID.GetValue(entityID);
            return Map.GetLightAt(index);
        }

        // TODO - Handle this more gracefully than with a type check
        public override void Save()
        {
            if (Map is Map map)
            {
                map.Save(Path);
            }
        }

        public override void Load()
        {
            if (Map is Map)
            {
                Map = SpiceEngine.Maps.Map.Load(Path);
            }
        }

        public static bool IsValidExtension(string extension) => extension == ".map";
    }
}
