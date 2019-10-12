using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Maps
{
    public class EntityMap
    {
        private int _indexCount;

        private int _lightStartIndex;
        private int _brushStartIndex;
        private int _actorStartIndex;
        private int _volumeStartIndex;

        private List<int> _ids = new List<int>();

        public int LightCount { get; private set; }
        public int BrushCount { get; private set; }
        public int ActorCount { get; private set; }
        public int VolumeCount { get; private set; }

        public IEnumerable<int> LightIDs => _ids.Skip(_lightStartIndex).Take(LightCount);
        public IEnumerable<int> BrushIDs => _ids.Skip(_brushStartIndex).Take(BrushCount);
        public IEnumerable<int> ActorIDs => _ids.Skip(_actorStartIndex).Take(ActorCount);
        public IEnumerable<int> VolumeIDs => _ids.Skip(_volumeStartIndex).Take(VolumeCount);

        public void AddID(int id) => _ids.Add(id);

        public void SetLightCount(int count)
        {
            _lightStartIndex = _indexCount;
            _indexCount += count;
            LightCount = count;
        }

        public void SetBrushCount(int count)
        {
            _brushStartIndex = _indexCount;
            _indexCount += count;
            BrushCount = count;
        }

        public void SetActorCount(int count)
        {
            _actorStartIndex = _indexCount;
            _indexCount += count;
            ActorCount = count;
        }

        public void SetVolumeCount(int count)
        {
            _volumeStartIndex = _indexCount;
            _indexCount += count;
            VolumeCount = count;
        }

        public void Clear()
        {
            _indexCount = 0;

            _lightStartIndex = 0;
            _brushStartIndex = 0;
            _actorStartIndex = 0;
            _volumeStartIndex = 0;

            LightCount = 0;
            BrushCount = 0;
            ActorCount = 0;
            VolumeCount = 0;

            _ids.Clear();
        }
    }
}
