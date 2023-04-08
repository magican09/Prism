
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace PrismWorkApp.Core
{
    public class AppSettingsSystem : BindableBase, INotifyPropertyChanged, IAppSettingsSystem
    {
        private readonly string appDataPath;
        private AppSettings _appSettings = new AppSettings();
        public string AppSaveDirName = "WorkApp";
        public string SettingsFaileName = "user_settings.json";
        public AppSettings AppSettings
        {
            get { return _appSettings; }
            set { SetProperty(ref _appSettings, value); }
        }
        public AppSettingsSystem()
        {
            appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSaveDirName);
            Save();
        }
        public void Save()
        {
            string settings_text = JsonSerializer.Serialize(_appSettings);
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);
            File.WriteAllText(Path.Combine(appDataPath, SettingsFaileName), settings_text);
        }
        public void Read()
        {
            string settings_tesx = File.ReadAllText(Path.Combine(appDataPath, SettingsFaileName));
            AppSettings settings = JsonSerializer.Deserialize(settings_tesx, typeof(AppSettings)) as AppSettings;

        }
    }
}
