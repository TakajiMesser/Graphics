using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.Views.UpDowns;
using SpiceEngineCore.Utilities;
using System;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePane.xaml
    /// </summary>
    public partial class GamePane : LayoutAnchorablePane, IAnchor, IDragPosition
    {
        public readonly static DependencyProperty WireframeThicknessProperty = DependencyProperty.Register("WireframeThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty SelectedWireframeThicknessProperty = DependencyProperty.Register("SelectedWireframeThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty SelectedLightWireframeThicknessProperty = DependencyProperty.Register("SelectedLightWireframeThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty GridThicknessProperty = DependencyProperty.Register("GridThickness", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty GridUnitProperty = DependencyProperty.Register("GridUnit", typeof(float), typeof(NumericUpDown));

        //public event EventHandler<CommandEventArgs> CommandExecuted;
        //public event EventHandler<SpiceEngine.Game.EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<CommandEventArgs> CommandExecuted;

        public LayoutAnchorable Anchor => Anchorable;

        public float WireframeThickness
        {
            get => (float)GetValue(WireframeThicknessProperty);
            set
            {
                SetValue(WireframeThicknessProperty, value);
                Viewport.SetWireframeThickness(value);
            }
        }

        public float SelectedWireframeThickness
        {
            get => (float)GetValue(SelectedWireframeThicknessProperty);
            set
            {
                SetValue(SelectedWireframeThicknessProperty, value);
                Viewport.SetSelectedWireframeThickness(value);
            }
        }

        public float SelectedLightWireframeThickness
        {
            get => (float)GetValue(SelectedLightWireframeThicknessProperty);
            set
            {
                SetValue(SelectedLightWireframeThicknessProperty, value);
                Viewport.SetSelectedLightWireframeThickness(value);
            }
        }

        public float GridThickness
        {
            get => (float)GetValue(GridThicknessProperty);
            set
            {
                SetValue(GridThicknessProperty, value);
                Viewport.SetGridLineThickness(value);
            }
        }

        public float GridUnit
        {
            get => (float)GetValue(GridUnitProperty);
            set
            {
                SetValue(GridUnitProperty, value);
                Viewport.SetGridUnit(value);
            }
        }

        public GamePane()
        {
            InitializeComponent();

            SettingsPopup.PlacementTarget = SettingsExpander;

            SettingsExpander.Expanded += (s, args) => SettingsPopup.IsOpen = true;
            SettingsExpander.Collapsed += (s, args) => SettingsPopup.IsOpen = false;

            GameDockPanel.Focusable = true;
            ViewModel.Viewport = Viewport;
            ViewModel.DragPositioner = this;

            //ViewModeButton.Value = ViewModel.ViewType;
            //ViewModel.OnViewTypeChanged;

            //Panel.EntitySelectionChanged += (s, args) => EntitySelectionChanged?.Invoke(this, args);
            //Panel.TransformModeChanged += GamePanel_TransformModeChanged;

            // Default to wireframe rendering
            //WireframeButton.IsEnabled = false;

            WireframeThicknessUpDown.ValueHoldChanged += (s, args) => Viewport.SetWireframeThickness(args.NewValue);
            WireframeColorPick.SelectedColorChanged += (s, args) => Viewport.SetWireframeColor(args.NewValue.Value.ToVector4().ToColor4());

            SelectedWireframeThicknessUpDown.ValueHoldChanged += (s, args) => Viewport.SetSelectedWireframeThickness(args.NewValue);
            SelectedWireframeColorPick.SelectedColorChanged += (s, args) => Viewport.SetSelectedWireframeColor(args.NewValue.Value.ToVector4().ToColor4());

            SelectedLightWireframeThicknessUpDown.ValueHoldChanged += (s, args) => Viewport.SetSelectedLightWireframeThickness(args.NewValue);
            SelectedLightWireframeColorPick.SelectedColorChanged += (s, args) => Viewport.SetSelectedLightWireframeColor(args.NewValue.Value.ToVector4().ToColor4());

            GridThicknessUpDown.ValueHoldChanged += (s, args) => Viewport.SetGridLineThickness(args.NewValue);
            GridUnitColorPick.SelectedColorChanged += (s, args) => Viewport.SetGridUnitColor(args.NewValue.Value.ToVector4().ToColor4());
            GridAxisColorPick.SelectedColorChanged += (s, args) => Viewport.SetGridAxisColor(args.NewValue.Value.ToVector4().ToColor4());
            Grid5ColorPick.SelectedColorChanged += (s, args) => Viewport.SetGrid5Color(args.NewValue.Value.ToVector4().ToColor4());
            Grid10ColorPick.SelectedColorChanged += (s, args) => Viewport.SetGrid10Color(args.NewValue.Value.ToVector4().ToColor4());
        }

        public Point Position(DragEventArgs args) => args.GetPosition(Viewport);

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
