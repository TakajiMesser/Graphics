using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.ViewModels.Commands;
using SauceEditor.Views.UpDowns;
using SpiceEngine.Game;
using SpiceEngine.Rendering;
using SpiceEngine.Utilities;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePanelView.xaml
    /// </summary>
    public partial class GamePanelView : LayoutAnchorablePane, IAnchor
    {
        public readonly static DependencyProperty WireframeThicknessProperty = DependencyProperty.Register("WireframeThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty SelectedWireframeThicknessProperty = DependencyProperty.Register("SelectedWireframeThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty SelectedLightWireframeThicknessProperty = DependencyProperty.Register("SelectedLightWireframeThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty GridThicknessProperty = DependencyProperty.Register("GridThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty GridUnitProperty = DependencyProperty.Register("GridUnit", typeof(float), typeof(NumericUpDown));

        //public event EventHandler<CommandEventArgs> CommandExecuted;
        public event EventHandler<SpiceEngine.Game.EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<CommandEventArgs> CommandExecuted;

        public LayoutAnchorable Anchor => Anchorable;

        public float WireframeThickness
        {
            get => (float)GetValue(WireframeThicknessProperty);
            set
            {
                SetValue(WireframeThicknessProperty, value);
                Panel.SetWireframeThickness(value);
            }
        }

        public float SelectedWireframeThickness
        {
            get => (float)GetValue(SelectedWireframeThicknessProperty);
            set
            {
                SetValue(SelectedWireframeThicknessProperty, value);
                Panel.SetSelectedWireframeThickness(value);
            }
        }

        public float SelectedLightWireframeThickness
        {
            get => (float)GetValue(SelectedLightWireframeThicknessProperty);
            set
            {
                SetValue(SelectedLightWireframeThicknessProperty, value);
                Panel.SetSelectedLightWireframeThickness(value);
            }
        }

        public float GridThickness
        {
            get => (float)GetValue(GridThicknessProperty);
            set
            {
                SetValue(GridThicknessProperty, value);
                Panel.SetGridLineThickness(value);
            }
        }

        public float GridUnit
        {
            get => (float)GetValue(GridUnitProperty);
            set
            {
                SetValue(GridUnitProperty, value);
                Panel.SetGridUnit(value);
            }
        }

        public GamePanelView()
        {
            InitializeComponent();

            DockPanel.Focusable = true;
            ViewModel.Panel = Panel;

            Panel.EntitySelectionChanged += (s, args) => EntitySelectionChanged?.Invoke(this, args);
            //Panel.TransformModeChanged += GamePanel_TransformModeChanged;

            // Default to wireframe rendering
            //WireframeButton.IsEnabled = false;

            WireframeThicknessUpDown.ValueHoldChanged += (s, args) => Panel.SetWireframeThickness(args.NewValue);
            WireframeColorPick.SelectedColorChanged += (s, args) => Panel.SetWireframeColor(args.NewValue.Value.ToVector4().ToColor4());

            SelectedWireframeThicknessUpDown.ValueHoldChanged += (s, args) => Panel.SetSelectedWireframeThickness(args.NewValue);
            SelectedWireframeColorPick.SelectedColorChanged += (s, args) => Panel.SetSelectedWireframeColor(args.NewValue.Value.ToVector4().ToColor4());

            SelectedLightWireframeThicknessUpDown.ValueHoldChanged += (s, args) => Panel.SetSelectedLightWireframeThickness(args.NewValue);
            SelectedLightWireframeColorPick.SelectedColorChanged += (s, args) => Panel.SetSelectedLightWireframeColor(args.NewValue.Value.ToVector4().ToColor4());

            GridThicknessUpDown.ValueHoldChanged += (s, args) => Panel.SetGridLineThickness(args.NewValue);
            GridUnitColorPick.SelectedColorChanged += (s, args) => Panel.SetGridUnitColor(args.NewValue.Value.ToVector4().ToColor4());
            GridAxisColorPick.SelectedColorChanged += (s, args) => Panel.SetGridAxisColor(args.NewValue.Value.ToVector4().ToColor4());
            Grid5ColorPick.SelectedColorChanged += (s, args) => Panel.SetGrid5Color(args.NewValue.Value.ToVector4().ToColor4());
            Grid10ColorPick.SelectedColorChanged += (s, args) => Panel.SetGrid10Color(args.NewValue.Value.ToVector4().ToColor4());
        }

        /*private void GamePanel_TransformModeChanged(object sender, TransformModeEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (e.TransformMode)
                {
                    case TransformModes.Translate:
                        TranslateButton.IsEnabled = false;
                        RotateButton.IsEnabled = true;
                        ScaleButton.IsEnabled = true;
                        break;

                    case TransformModes.Rotate:
                        TranslateButton.IsEnabled = true;
                        RotateButton.IsEnabled = false;
                        ScaleButton.IsEnabled = true;
                        break;

                    case TransformModes.Scale:
                        TranslateButton.IsEnabled = true;
                        RotateButton.IsEnabled = true;
                        ScaleButton.IsEnabled = false;
                        break;
                }
            });
        }*/

        /*protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //DockManager.ParentWindow = this;
            //Grid.Children.Add(GameWindow);
        }*/

        private void OnLoaded(object sender, EventArgs e) { }

        /*protected override void OnClosing(CancelEventArgs e)
        {
            //GamePanel?.Close();
            base.OnClosing(e);
        }*/
    }
}
