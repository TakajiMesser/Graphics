using SauceEditor.ViewModels.Docks;

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

    public class ModelToolPanelViewModel : DockViewModel
    {
        public ModelToolPanelViewModel() : base(DockTypes.Tool) { }

        public ModelToolTypes ModelToolType { get; set; }

        public void OnModelToolTypeChanged()
        {

        }
    }
}