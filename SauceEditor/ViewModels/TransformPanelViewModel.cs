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
        private float _x;
        private float _y;
        private float _z;

        public void SetValues(Vector3 transform)
        {
            X = transform.X;
            Y = transform.Y;
            Z = transform.Z;
        }

        public float X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        public float Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        public float Z
        {
            get => _z;
            set => SetProperty(ref _z, value);
        }
    }
}