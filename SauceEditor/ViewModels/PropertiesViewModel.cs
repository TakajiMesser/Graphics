using OpenTK;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SauceEditor.ViewModels
{
    public class PropertiesViewModel : ViewModel
    {
        private readonly DelegateCommand _openScriptCommand;

        public bool EntitySelected { get; set; }
        public string EntityType { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3? Rotation { get; set; }
        public Vector3? Scale { get; set; }
        public Color4? Color { get; set; }

        public ICommand OpenScriptCommand => _openScriptCommand;

        public PropertiesViewModel(EditorEntity editorEntity)
        {
            EntitySelected = editorEntity != null;
            EntityType = "No Properties to Show";

            if (editorEntity != null)
            {
                EntityType = editorEntity.Entity.GetType().Name;

                Position = editorEntity.MapEntity.Position;

                if (editorEntity.Entity is IRotate)
                {
                    Rotation = editorEntity.MapEntity.Rotation;
                }

                if (editorEntity.Entity is IScale)
                {
                    Scale = editorEntity.MapEntity.Scale;
                }

                if (editorEntity.MapEntity is MapActor mapActor)
                {
                    Name = mapActor.Name;
                }

                if (editorEntity.MapEntity is MapLight mapLight)
                {
                    Color = mapLight.Color.ToMediaColor();
                }
            }
            
            

            _openScriptCommand = new DelegateCommand(
                (p) =>
                {
                    _openScriptCommand.InvokeCanExecuteChanged();
                },
                (p) =>
                {
                    return true;
                }
            );
        }
    }
}