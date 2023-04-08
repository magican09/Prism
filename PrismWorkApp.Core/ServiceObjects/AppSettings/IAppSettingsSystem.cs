namespace PrismWorkApp.Core
{
    public interface IAppSettingsSystem
    {
        public AppSettings AppSettings { get; set; }
        public void Read();
        public void Save();
    }
}