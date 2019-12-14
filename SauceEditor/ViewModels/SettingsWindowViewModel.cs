using SauceEditor.Models;
using SauceEditor.Views.Factories;

namespace SauceEditor.ViewModels
{
    public class SettingsWindowViewModel : ViewModel
    {
        public EditorSettings Settings { get; set; }

        public IWindow Window { get; set; }
        public IMainView MainView { get; set; }

        private RelayCommand _okCommand;
        public RelayCommand OKCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new RelayCommand(
                    p =>
                    {
                        SaveSettings();
                        Window.Close();
                    },
                    p => true
                ));
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                    p => Window.Close(),
                    p => true
                ));
            }
        }

        public SettingsWindowViewModel() => LoadSettings();

        private void LoadSettings() => EditorSettings.Load(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);

        private void SaveSettings()
        {
            EditorSettings.Instance.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            EditorSettings.Reload();
        }
    }
}