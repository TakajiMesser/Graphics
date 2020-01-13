using System.Collections.Generic;

namespace SpiceEngineCore.Maps
{
    public class EntityMapping
    {
        private enum IDTypes
        {
            Camera,
            Brush,
            Actor,
            Light,
            Volume,
            UIElement
        }

        private int _indexCount = 0;
        private int _idCount = 0;
        private Dictionary<int, IDTypes> _idTypeByIndex = new Dictionary<int, IDTypes>();

        private List<int> _cameraIDs = new List<int>();
        private List<int> _brushIDs = new List<int>();
        private List<int> _actorIDs = new List<int>();
        private List<int> _lightIDs = new List<int>();
        private List<int> _volumeIDs = new List<int>();
        private List<int> _uiElementIDs = new List<int>();

        public IEnumerable<int> CameraIDs => _cameraIDs;
        public IEnumerable<int> BrushIDs => _brushIDs;
        public IEnumerable<int> ActorIDs => _actorIDs;
        public IEnumerable<int> LightIDs => _lightIDs;
        public IEnumerable<int> VolumeIDs => _volumeIDs;
        public IEnumerable<int> UIElementIDs => _uiElementIDs;

        public void AddCameras(int nCameras)
        {
            var startIndex = _indexCount;
            _indexCount += nCameras;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.Camera);
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

        public void AddLights(int nLights)
        {
            var startIndex = _indexCount;
            _indexCount += nLights;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.Light);
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

        public void AddUIElements(int nUIElements)
        {
            var startIndex = _indexCount;
            _indexCount += nUIElements;

            for (var i = startIndex; i < _indexCount; i++)
            {
                _idTypeByIndex.Add(i, IDTypes.UIElement);
            }
        }

        public void AddID(int id)
        {
            switch (_idTypeByIndex[_idCount])
            {
                case IDTypes.Camera:
                    _cameraIDs.Add(id);
                    break;
                case IDTypes.Brush:
                    _brushIDs.Add(id);
                    break;
                case IDTypes.Actor:
                    _actorIDs.Add(id);
                    break;
                case IDTypes.Light:
                    _lightIDs.Add(id);
                    break;
                case IDTypes.Volume:
                    _volumeIDs.Add(id);
                    break;
                case IDTypes.UIElement:
                    _uiElementIDs.Add(id);
                    break;
            }

            _idCount++;
        }
    }
}
