using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SauceEditor.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ViewModel> _childViewModels = new ObservableCollection<ViewModel>();

        public ObservableCollection<ViewModel> ChildViewModels
        {
            get => _childViewModels;
            set
            {
                if (_childViewModels != value)
                {
                    _childViewModels = value;

                    foreach (var childViewModel in _childViewModels)
                    {
                        childViewModel.PropertyChanged += ChildPropertyChanged;
                    }

                    SetProperty(ref _childViewModels, value);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler ChildPropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            
            return false;
        }

        public void AddChild(ViewModel childViewModel, PropertyChangedEventHandler changeHandler)
        {
            childViewModel.PropertyChanged += changeHandler;
            _childViewModels.Add(childViewModel);
        }
    }
}