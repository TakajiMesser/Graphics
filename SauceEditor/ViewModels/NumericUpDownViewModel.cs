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
    public class NumericUpDownViewModel : ViewModel
    {
        private float _value;

        public float Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}