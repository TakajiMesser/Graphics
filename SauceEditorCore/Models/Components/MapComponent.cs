using SauceEditorCore.Models.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngineCore.Entities;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Components
{
    public class MapComponent : Component
    {
        public MapComponent() {}
        public MapComponent(string filePath) : base(filePath) {}

        private enum MapEntityType
        {
            Actor,
            Brush,
            Volume,
            Light
        }

        private Dictionary<int, MapEntityType> _entityTypeByEntityID = new Dictionary<int, MapEntityType>();

        private Dictionary<int, int> _mapActorIndexByEntityID = new Dictionary<int, int>();
        private Dictionary<int, int> _mapBrushIndexByEntityID = new Dictionary<int, int>();
        private Dictionary<int, int> _mapVolumeIndexByEntityID = new Dictionary<int, int>();
        private Dictionary<int, int> _mapLightIndexByEntityID = new Dictionary<int, int>();

        //private Dictionary<int, IMapEntity3D> _entityIDByMapEntity = new Dictionary<int, IMapEntity3D>();

        public Map Map { get; set; }

        public void SetEntityMapping(IList<int> actorIDs, IList<int> brushIDs, IList<int> volumeIDs, IList<int> lightIDs)
        {
            for (var i = 0; i < actorIDs.Count; i++)
            {
                _mapActorIndexByEntityID.Add(actorIDs[i], i);
            }

            for (var i = 0; i < brushIDs.Count; i++)
            {
                _mapBrushIndexByEntityID.Add(brushIDs[i], i);
            }

            for (var i = 0; i < volumeIDs.Count; i++)
            {
                _mapVolumeIndexByEntityID.Add(volumeIDs[i], i);
            }

            for (var i = 0; i < lightIDs.Count; i++)
            {
                _mapLightIndexByEntityID.Add(lightIDs[i], i);
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
            var index = _mapActorIndexByEntityID[actor.ID];
            var mapActor = Map.Actors[index];

            mapActor.UpdateFrom(actor);
        }

        private void UpdateMapBrush(Brush brush)
        {
            var index = _mapBrushIndexByEntityID[brush.ID];
            var mapBrush = Map.Brushes[index];

            mapBrush.UpdateFrom(brush);
        }

        private void UpdateMapVolume(Volume volume)
        {
            var index = _mapVolumeIndexByEntityID[volume.ID];
            var mapVolume = Map.Volumes[index];

            mapVolume.UpdateFrom(volume);
        }

        private void UpdateMapLight(ILight light)
        {
            var index = _mapLightIndexByEntityID[light.ID];
            var mapLight = Map.Lights[index];

            mapLight.UpdateFrom(light);
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
            var index = _mapActorIndexByEntityID[entityID];
            return Map.Actors[index];
        }

        public MapBrush GetMapBrush(int entityID)
        {
            var index = _mapBrushIndexByEntityID[entityID];
            return Map.Brushes[index];
        }

        public MapVolume GetMapVolume(int entityID)
        {
            var index = _mapVolumeIndexByEntityID[entityID];
            return Map.Volumes[index];
        }

        public MapLight GetMapLight(int entityID)
        {
            var index = _mapLightIndexByEntityID[entityID];
            return Map.Lights[index];
        }

        public override void Save() => Map.Save(Path);
        public override void Load() => Map = Map.Load(Path);

        public static bool IsValidExtension(string extension) => extension == ".map";
    }
}
