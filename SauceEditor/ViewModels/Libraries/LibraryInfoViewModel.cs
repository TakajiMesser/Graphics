using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Libraries;
using SpiceEngineCore.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;

namespace SauceEditor.ViewModels.Libraries
{
    public class LibraryInfoViewModel : PathInfoViewModel
    {
        public LibraryInfoViewModel(LibraryInfo libraryInfo, ILibraryTracker libraryTracker)
        {
            PathInfo = libraryInfo;
            PreviewIcon = new BitmapImage();//GetPreviewIcon<T>();
            OpenCommand = new RelayCommand(p => libraryTracker.OpenLibrary(libraryInfo.Name, Children));

            Children = new ReadOnlyCollection<PathInfoViewModel>(CreateChildren(libraryInfo, libraryTracker).ToList());
        }

        public ReadOnlyCollection<PathInfoViewModel> Children { get; set; }

        // TODO - Initialize as default preview icon based on component type. Should load same default icon statically and share across all components to save memory
        public override void LoadPreviewIcon()
        {
            throw new NotImplementedException();

            /*var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelWidth = 100;
            bitmapImage.UriSource = new Uri(Path);
            bitmapImage.EndInit();

            var croppedImage = new CroppedBitmap(bitmapImage, new Int32Rect(0, 0, 100, 100));

            var scaleWidth = 100;
            var scaleHeight = 100;
            var scaledImage = new TransformedBitmap(croppedImage, new ScaleTransform(scaleWidth, scaleHeight));

            PreviewIcon = scaledImage;*/
        }

        public static IEnumerable<PathInfoViewModel> CreateChildren(LibraryInfo libraryInfo, ILibraryTracker libraryTracker)
        {
            foreach (var item in libraryInfo.Items)
            {
                if (item is LibraryInfo libraryInfoItem)
                {
                    yield return new LibraryInfoViewModel(libraryInfoItem, libraryTracker);
                }
                else if (item is ComponentInfo componentInfoItem)
                {
                    yield return new ComponentInfoViewModel(componentInfoItem, libraryTracker);
                }
            }
        }

        private static byte[] GetPreviewIcon<T>() where T : IComponent
        {
            return new TypeSwitch<byte[]>()
                /*.Case<MapComponent>(() => "Maps")
                .Case<ModelComponent>(() => "Models")
                .Case<BehaviorComponent>(() => "Behaviors")
                .Case<TextureComponent>(() => "Textures")
                .Case<SoundComponent>(() => "Sounds")
                .Case<MaterialComponent>(() => "Materials")
                .Case<ArchetypeComponent>(() => "Archetypes")
                .Case<ScriptComponent>(() => "Scripts")*/
                .Default(() => new byte[0])//throw new NotImplementedException())
                .Match<T>();
        }
    }
}