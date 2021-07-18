using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.UserInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Scenes
{
    public class Scene : IScene
    {
        public List<ICamera> Cameras { get; } = new List<ICamera>();
        public List<IActor> Actors { get; } = new List<IActor>();
        public List<IBrush> Brushes { get; } = new List<IBrush>();
        public List<IVolume> Volumes { get; } = new List<IVolume>();
        public List<ILight> Lights { get; } = new List<ILight>();
        public List<IUIItem> UIItems { get; } = new List<IUIItem>();

        public ICamera ActiveCamera
        {
            get
            {
                var camera = Cameras.FirstOrDefault(c => c.IsActive);
                if (camera == null)
                {
                    camera = Cameras.FirstOrDefault();
                }

                return camera;
            }
        }

        public void AddEntity(IEntity entity)
        {
            switch (entity)
            {
                case ICamera camera:
                    Cameras.Add(camera);
                    camera.BecameActive += Camera_BecameActive;
                    break;
                case IActor actor:
                    Actors.Add(actor);
                    break;
                case IBrush brush:
                    Brushes.Add(brush);
                    break;
                case IVolume volume:
                    Volumes.Add(volume);
                    break;
                case ILight light:
                    Lights.Add(light);
                    break;
                case IUIItem uiItem:
                    UIItems.Add(uiItem);
                    break;
            }
        }

        public void RemoveEntity(IEntity entity)
        {
            switch (entity)
            {
                case ICamera camera:
                    camera.BecameActive -= Camera_BecameActive;
                    Cameras.Remove(camera);
                    break;
                case IActor actor:
                    Actors.Remove(actor);
                    break;
                case IBrush brush:
                    Brushes.Remove(brush);
                    break;
                case IVolume volume:
                    Volumes.Remove(volume);
                    break;
                case ILight light:
                    Lights.Remove(light);
                    break;
                case IUIItem uiItem:
                    UIItems.Remove(uiItem);
                    break;
            }
        }

        private void Camera_BecameActive(object sender, EventArgs e)
        {
            foreach (var camera in Cameras.Where(c => c != sender))
            {
                //camera.IsActive = false;
            }
        }
    }
}
