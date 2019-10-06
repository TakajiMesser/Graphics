using SauceEditorCore.Models.Components;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Libraries
{
    public class ComponentInfoViewModel : PathInfoViewModel
    {
        public ComponentInfoViewModel(ComponentInfo componentInfo, ILibraryTracker libraryTracker)
        {
            PathInfo = componentInfo;//new ComponentInfo(component);
            OpenCommand = new RelayCommand(p => libraryTracker.OpenComponent(componentInfo));
        }

        public override void LoadPreviewIcon()
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelWidth = 100;
            bitmapImage.UriSource = new Uri(PathInfo.Path);
            bitmapImage.EndInit();

            var croppedImage = new CroppedBitmap(bitmapImage, new Int32Rect(0, 0, 100, 100));

            var scaleWidth = 100;
            var scaleHeight = 100;
            var scaledImage = new TransformedBitmap(croppedImage, new ScaleTransform(scaleWidth, scaleHeight));

            PreviewIcon = bitmapImage;
        }

        /*private static RelayCommand GetOpenCommand<T>(T component, IComponentFactory componentFactory) where T : IComponent => new TypeSwitch<RelayCommand>()
            .Case<MapComponent>(() => new RelayCommand(p => componentFactory.OpenMap(component.Path)))
            .Case<ModelComponent>(() => new RelayCommand(p => componentFactory.OpenModel(component.Path)))
            .Case<BehaviorComponent>(() => new RelayCommand(p => componentFactory.OpenBehavior(component.Path)))
            .Case<TextureComponent>(() => new RelayCommand(p => componentFactory.OpenTexture(component.Path)))
            .Case<SoundComponent>(() => new RelayCommand(p => componentFactory.OpenSound(component.Path)))
            .Case<MaterialComponent>(() => new RelayCommand(p => componentFactory.OpenMaterial(component.Path)))
            .Case<ArchetypeComponent>(() => new RelayCommand(p => componentFactory.OpenArchetype(component.Path)))
            .Case<ScriptComponent>(() => new RelayCommand(p => componentFactory.OpenScript(component.Path)))
            .Default(() => throw new NotImplementedException())
            .Match<T>();*/
    }
}