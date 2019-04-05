﻿using SauceEditor.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SauceEditor.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public EditorSettings Settings
        {
            get => _settings;
            private set
            {
                _settings = value;
                SetDefaultView();
                SetDefaultTool();
            }
        }

        public event EventHandler<SettingsEventArgs> SettingsChanged;

        private EditorSettings _settings;

        public SettingsWindow(EditorSettings settings)
        {
            InitializeComponent();
            Settings = settings;
        }

        private void SetDefaultView()
        {
            switch (Settings.DefaultView)
            {
                case ViewTypes.All:
                    View_All.IsSelected = true;
                    break;
                case ViewTypes.Perspective:
                    View_Perspective.IsSelected = true;
                    break;
                case ViewTypes.X:
                    View_X.IsSelected = true;
                    break;
                case ViewTypes.Y:
                    View_Y.IsSelected = true;
                    break;
                case ViewTypes.Z:
                    View_Z.IsSelected = true;
                    break;
            }
        }

        private void SetDefaultTool()
        {
            switch (Settings.DefaultTool)
            {
                case SpiceEngine.Game.Tools.Brush:
                    Tool_Brush.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Mesh:
                    Tool_Mesh.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Selector:
                    Tool_Selector.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Texture:
                    Tool_Texture.IsSelected = true;
                    break;
                case SpiceEngine.Game.Tools.Volume:
                    Tool_Volume.IsSelected = true;
                    break;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsChanged?.Invoke(this, new SettingsEventArgs(Settings));
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ViewComboBox.SelectedItem as ComboBoxItem;

            switch (selectedItem.Content)
            {
                case "All":
                    Settings.DefaultView = ViewTypes.All;
                    break;
                case "Perspective":
                    Settings.DefaultView = ViewTypes.Perspective;
                    break;
                case "X":
                    Settings.DefaultView = ViewTypes.X;
                    break;
                case "Y":
                    Settings.DefaultView = ViewTypes.Y;
                    break;
                case "Z":
                    Settings.DefaultView = ViewTypes.Z;
                    break;
            }
        }

        private void ToolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ToolComboBox.SelectedItem as ComboBoxItem;

            switch (selectedItem.Content)
            {
                case "Brush":
                    Settings.DefaultTool = SpiceEngine.Game.Tools.Brush;
                    break;
                case "Mesh":
                    Settings.DefaultTool = SpiceEngine.Game.Tools.Mesh;
                    break;
                case "Selector":
                    Settings.DefaultTool = SpiceEngine.Game.Tools.Selector;
                    break;
                case "Texture":
                    Settings.DefaultTool = SpiceEngine.Game.Tools.Texture;
                    break;
                case "Volume":
                    Settings.DefaultTool = SpiceEngine.Game.Tools.Volume;
                    break;
            }
        }
    }
}