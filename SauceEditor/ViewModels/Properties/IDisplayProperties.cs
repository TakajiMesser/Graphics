using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditorCore.Models.Entities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Utilities;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SauceEditor.ViewModels.Properties
{
    public interface IDisplayProperties
    {
        void UpdateFromEntity(EditorEntity entity);
    }
}