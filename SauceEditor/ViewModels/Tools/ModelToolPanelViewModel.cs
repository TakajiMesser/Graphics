namespace SauceEditor.ViewModels.Tools
{
    public enum ModelToolTypes
    {
        Shape,
        Face,
        Triangle,
        Vertex,
        Texture
    }

    public class ModelToolPanelViewModel : ViewModel
    {
        public ModelToolTypes ModelToolType { get; set; }
    }
}