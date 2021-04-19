using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.UserInterfaces;
using System.Collections.Generic;

namespace SpiceEngineCore.Scenes
{
    public interface IScene
    {
        List<ICamera> Cameras { get; }
        List<IActor> Actors { get; }
        List<IBrush> Brushes { get; }
        List<IVolume> Volumes { get; }
        List<ILight> Lights { get; }
        List<IUIItem> UIItems { get; }

        ICamera ActiveCamera { get; }

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
    }
}
