using SpiceEngine.Maps;
using System;
using System.Collections;

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
                _mapActorIndexByEntityID.Add(i, entityMapping.ActorIDs[i]);
            }

            for (var i = 0; i < entityMapping.BrushIDs.Count; i++)
            {
                _mapBrushIndexByEntityID.Add(i, entityMapping.BrushIDs[i]);
            }

            for (var i = 0; i < entityMapping.VolumeIDs.Count; i++)
            {
                _mapVolumeIndexByEntityID.Add(i, entityMapping.VolumeIDs[i]);
            }

            for (var i = 0; i < entityMapping.LightIDs.Count; i++)
            {
                _mapLightIndexByEntityID.Add(i, entityMapping.LightIDs[i]);
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

        public void AddMapLight(Light light, int entityID)
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
                    case Light light:
                        UpdateMapLight(light);
                        break;
                }
            }
        }

        private void UpdateMapActor(Actor actor)
        {
            var index = _mapActorIndexByEntityID[actor.ID];
            var mapActor = Map.Actors[index];

            mapActor.Position = actor.Position;
            mapActor.Rotation = actor.Rotation;
            mapActor.Scale = actor.Scale;
        }

        private void UpdateMapBrush(Brush brush)
        {
            var index = _mapBrushIndexByEntityID[brush.ID];
            var mapBrush = Map.Brushes[index];

            mapBrush.Position = brush.Position;
            mapBrush.Rotation = brush.Rotation;
            mapBrush.Scale = brush.Scale;
        }

        private void UpdateMapVolume(Volume volume)
        {
            var index = _mapVolumeIndexByEntityID[volume.ID];
            var mapVolume = Map.Volumes[index];

            mapVolume.Position = volume.Position;
            mapVolume.Rotation = volume.Rotation;
            mapVolume.Scale = volume.Scale;
        }

        private void UpdateMapLight(Light light)
        {
            var index = _mapLightIndexByEntityID[light.ID];
            var mapLight = Map.Lights[index];

            mapLight.Position = light.Position;
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

        public Light GetMapLight(int entityID)
        {
            var index = _mapLightIndexByEntityID[entityID];
            return Map.Lights[index];
        }
    }
}
