namespace SauceEditor.ViewModels.Tools
{
    public enum ToolTypes
    {
        Select,
        Volume,
        Brush,
        Mesh,
        Texture
    }

    public class ToolsPanelViewModel : ViewModel
    {
        public ToolTypes ToolType { get; set; }
    }
}