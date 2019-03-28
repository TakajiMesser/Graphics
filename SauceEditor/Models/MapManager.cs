using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using System.Collections.Generic;

namespace SauceEditor.Models
{
    public class MapManager
    {
        /*public MapCamera Camera { get; set; }
        public List<MapActor> Actors { get; set; } = new List<MapActor>();
        public List<MapBrush> Brushes { get; set; } = new List<MapBrush>();
        public List<MapVolume> Volumes { get; set; } = new List<MapVolume>();
        public List<Light> Lights { get; set; } = new List<Light>();
        public List<string> SkyboxTextureFilePaths { get; set; } = new List<string>();*/

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

        public Map Map { get; }

        public MapManager(string mapPath)
        {
            Map = Map.Load(mapPath);
        }

        public void SetEntityMapping(EntityMapping entityMapping)
        {
            for (var i = 0; i < entityMapping.ActorIDs.Count; i++)
            {
                _mapActorIndexByEntityID.Add(entityMapping.ActorIDs[i], i);
            }

            for (var i = 0; i < entityMapping.BrushIDs.Count; i++)
            {
                _mapBrushIndexByEntityID.Add(entityMapping.BrushIDs[i], i);
            }

            for (var i = 0; i < entityMapping.VolumeIDs.Count; i++)
            {
                _mapVolumeIndexByEntityID.Add(entityMapping.VolumeIDs[i], i);
            }

            for (var i = 0; i < entityMapping.LightIDs.Count; i++)
            {
                _mapLightIndexByEntityID.Add(entityMapping.LightIDs[i], i);
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
    }
}
