namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IAppSettingsSystem
    {
         AppSettings AppSettings { get; set; }
         void Read();
         void Save();
    }
}