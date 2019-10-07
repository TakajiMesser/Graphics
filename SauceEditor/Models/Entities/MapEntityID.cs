using SpiceEngine.Maps;
using System;

namespace SauceEditor.Models.Entities
{
    public class MapEntityID
    {
        private int _id;

        public MapEntityID(IMapEntity3D mapEntity)
        {
            MapEntity = mapEntity;
            Name = GetInitialName();
        }

        public IMapEntity3D MapEntity { get; }
        public string Name { get; private set; }

        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                Name += " (" + value + ")";
            }
        }

        private string GetInitialName()
        {
            if (MapEntity is MapActor mapActor)
            {
                return mapActor.Name;
            }
            else if (MapEntity is MapLight)
            {
                return "Light";
            }
            else if (MapEntity is MapBrush)
            {
                return "Brush";
            }
            else if (MapEntity is MapVolume)
            {
                return "Volume";
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
