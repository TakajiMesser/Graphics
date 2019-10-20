using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.Libraries;
using SauceEditor.Views.Properties;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Tools;
using SauceEditor.Views.Trees.Entities;
using SauceEditor.Views.Trees.Projects;
using System.Collections.Generic;

namespace SauceEditor.Views
{
    public interface IPanelFactory
    {
        void OpenModelToolPanel();
        void OpenBrushToolPanel();

        void OpenProjectTreePanel();
        void OpenLibraryPanel();
        void OpenPropertyPanel();
        void OpenEntityTreePanel();
    }
}
