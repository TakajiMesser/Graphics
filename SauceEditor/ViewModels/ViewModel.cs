using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SauceEditor.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ViewModel> ChildViewModels { get; set; } = new ObservableCollection<ViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Contracts", "CS0067", Justification = "Fody.PropertyChanged requires this event.")]
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler ChildPropertyChanged;

        public void OnChildViewModelsChanged()
        {
            foreach (var childViewModel in ChildViewModels)
            {
                childViewModel.PropertyChanged += ChildPropertyChanged;
            }
        }

        public void AddChild(ViewModel childViewModel, PropertyChangedEventHandler changeHandler)
        {
            childViewModel.PropertyChanged += changeHandler;
            ChildViewModels.Add(childViewModel);
        }
    }
}