using SauceEditorCore.Models.Libraries;
using System;
using System.IO;

namespace SauceEditorCore.Models.Components
{
    public class ComponentInfo : IPathInfo
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        public bool Exists { get; private set; }
        public long FileSize { get; private set; }
        
        public DateTime? CreationTime { get; private set; }
        public DateTime? LastWriteTime { get; private set; }
        public DateTime? LastAccessTime { get; private set; }

        // TODO - Initialize as default preview icon based on component type. Should load same default icon statically and share across all components to save memory
        public byte[] PreviewBitmap { get; private set; }

        private ComponentInfo() { }

        public void Refresh()
        {
            var fileInfo = new FileInfo(Path);

            if (fileInfo.Exists)
            {
                Exists = true;
                FileSize = fileInfo.Length;
                CreationTime = fileInfo.CreationTime;
                LastWriteTime = fileInfo.LastWriteTime;
                LastAccessTime = fileInfo.LastAccessTime;
            }
            else
            {
                Exists = false;
                FileSize = 0;
                CreationTime = null;
                LastWriteTime = null;
                LastAccessTime = null;
            }
        }

        // TODO - Should this be kicked off asynchronously from Refresh()?
        public void LoadPreviewIcon()
        {
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

        public static ComponentInfo Create(IComponent component)
        {
            return new ComponentInfo()
            {
                Name = component.Name,
                Path = component.Path
            };
        }
    }
}
