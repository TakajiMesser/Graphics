using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Maps;

namespace SauceEditor.Models
{
    public class Script
    {
        public string FilePath { get; private set; }

        public Actor Actor { get; private set; }
        public MapActor MapActor { get; private set; }

        public void SetEntities(Actor actor, MapActor mapActor)
        {
            Actor = actor;
            MapActor = mapActor;
        }

        public void Load(string filePath)
        {
            FilePath = filePath;
        }
    }
}
