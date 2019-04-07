using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class TransformPanelViewModel : ViewModel
    {
        private Vector3 _transform;

        private NumericUpDownViewModel _xViewModel;
        private NumericUpDownViewModel _yViewModel;
        private NumericUpDownViewModel _zViewModel;

        public Vector3 Transform
        {
            get => _transform;
            set => SetProperty(ref _transform, value);
        }

        public NumericUpDownViewModel XViewModel
        {
            get => _xViewModel;
            set
            {
                _xViewModel = value;
                AddChild(_xViewModel, OnChildViewModelUpdated);
            }
        }

        public NumericUpDownViewModel YViewModel
        {
            get => _yViewModel;
            set
            {
                _yViewModel = value;
                AddChild(_yViewModel, OnChildViewModelUpdated);
            }
        }

        public NumericUpDownViewModel ZViewModel
        {
            get => _zViewModel;
            set
            {
                _zViewModel = value;
                AddChild(_zViewModel, OnChildViewModelUpdated);
            }
        }

        public void UpdateTransform(Vector3 transform)
        {
            _transform = transform;

            XViewModel.Value = transform.X;
            YViewModel.Value = transform.Y;
            ZViewModel.Value = transform.Z;
        }

        private void OnChildViewModelUpdated(object sender, PropertyChangedEventArgs args) =>
            Transform = new Vector3(XViewModel.Value, YViewModel.Value, ZViewModel.Value);
    }
}