using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SauceEditor.ViewModels.Trees
{
    public interface IComponentListViewModel
    {
        string Name { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
    }
}