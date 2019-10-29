using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Maps
{
    public class EntityMap
    {
        private enum IDTypes
        {
            Light,
            Brush,
            Actor,
            Volume
        }

        /*private int _lightStartIndex;
        private int _brushStartIndex;
        private int _actorStartIndex;
        private int _volumeStartIndex;

        private int _lightCount;
        private int _brushCount;
        private int _actorCount;
        private int _volumeCount;

        private List<int> _ids = new List<int>();

        public IEnumerable<int> LightIDs => _ids.Skip(_lightStartIndex).Take(_lightCount);
        public IEnumerable<int> BrushIDs => _ids.Skip(_brushStartIndex).Take(_brushCount);
        public IEnumerable<int> ActorIDs => _ids.Skip(_actorStartIndex).Take(_actorCount);
        public IEnumerable<int> VolumeIDs => _ids.Skip(_volumeStartIndex).Take(_volumeCount);*/

        private int _indexCount = 0;
        private int _idCount = 0;
        private Dictionary<int, IDTypes> _idTypeByIndex = new Dictionary<int, IDTypes>();

        private List<int> _lightIDs = new List<int>();
        private List<int> _brushIDs = new List<int>();
        private List<int> _actorIDs = new List<int>();
        private List<int> _volumeIDs = new List<int>();

        public IEnumerable<int> LightIDs => _lightIDs;
        public IEnumerable<int> BrushIDs => _brushIDs;
        public IEnumerable<int> ActorIDs => _actorIDs;
        public IEnumerable<int> VolumeIDs => _volumeIDs;

        public void AddLights(int nLights)
        {
            var startIndex = _indexCount;
            _indexCount += nLights;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.Light);
            }
        }

        public void AddBrushes(int nBrushes)
        {
            var startIndex = _indexCount;
            _indexCount += nBrushes;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.Brush);
            }
        }

        public void AddActors(int nActors)
        {
            var startIndex = _indexCount;
            _indexCount += nActors;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.Actor);
            }
        }

        public void AddVolumes(int nVolumes)
        {
            var startIndex = _indexCount;
            _indexCount += nVolumes;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.Volume);
            }
        }

        public void AddID(int id)
        {
            switch (_idTypeByIndex[_idCount])
            {
                case IDTypes.Light:
                    _lightIDs.Add(id);
                    break;
                case IDTypes.Brush:
                    _brushIDs.Add(id);
                    break;
                case IDTypes.Actor:
                    _actorIDs.Add(id);
                    break;
                case IDTypes.Volume:
                    _volumeIDs.Add(id);
                    break;
            }

            _idCount++;
        }

        /*public void AddID(int id) => _ids.Add(id);

        public void SetLightCount(int count)
        {
            _lightStartIndex = _indexCount;
            _indexCount += count;
            _lightCount = count;
        }

        public void SetBrushCount(int count)
        {
            _brushStartIndex = _indexCount;
            _indexCount += count;
            _brushCount = count;
        }

        public void SetActorCount(int count)
        {
            _actorStartIndex = _indexCount;
            _indexCount += count;
            _actorCount = count;
        }

        public void SetVolumeCount(int count)
        {
            _volumeStartIndex = _indexCount;
            _indexCount += count;
            _volumeCount = count;
        }

        public void Clear()
        {
            _indexCount = 0;

            _lightStartIndex = 0;
            _brushStartIndex = 0;
            _actorStartIndex = 0;
            _volumeStartIndex = 0;

            _lightCount = 0;
            _brushCount = 0;
            _actorCount = 0;
            _volumeCount = 0;

            _ids.Clear();
        }*/
    }
}
