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
        public float Value { get; set; }

        /*private RelayCommand _litCommand;

        public RelayCommand WireframeCommand
        {
            get
            {
                if (_wireframeCommand == null)
                {
                    _wireframeCommand = new RelayCommand(
                        p =>
                        {
                            Panel.RenderMode = RenderModes.Wireframe;
                            CommandManager.InvalidateRequerySuggested();
                        },
                        p => Panel != null && Panel.RenderMode != RenderModes.Wireframe
                    );
                }

                return _wireframeCommand;
            }
        }*/
    }
}