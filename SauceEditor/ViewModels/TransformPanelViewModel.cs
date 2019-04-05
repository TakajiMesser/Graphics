using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class TransformPanelViewModel : ViewModel
    {
        private Vector3 _transform;
        private ObservableCollection<NumericUpDownViewModel> _children;

        public Vector3 Transform
        {
            get => _transform;
            set => SetProperty(ref _transform, value);
        }
        
        public ObservableCollection<NumericUpDownViewModel> Children
        {
            get => _children;
            set
            {
                if (_children != value)
                {
                    _children = value;

                    foreach (var child in _children)
                    {
                        child.PropertyChanged += OnChildPropertyChanged;
                    }

                    SetProperty(ref _children, value);
                }
            }
        }

        public TransformPanelViewModel()
        {
            AddChild(X_UpDown.ViewModel, OnChildPropertyChanged);
            AddChild(Y_UpDown.ViewModel, OnChildPropertyChanged);
            AddChild(Z_UpDown.ViewModel, OnChildPropertyChanged);

            Children = new ObservableCollection<NumericUpDownViewModel>()
            {
                X_UpDown.ViewModel,
                Y_UpDown.ViewModel,
                Z_UpDown.ViewModel
            };
        }

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Transform = new Vector3()
            {
                X = X_UpDown.ViewModel.Value,
                Y = Y_UpDown.ViewModel.Value,
                Z = Z_UpDown.ViewModel.Value
            };
        }
    }
}