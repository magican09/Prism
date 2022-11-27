using System.Collections.ObjectModel;

namespace PrismWorkApp.Core.Console
{
    public interface IModulesContext
    {
        ObservableCollection<ModuleInfoData> ModulesInfoData { get; set; }
    }
    public class ModulesContext : IModulesContext
    {
        public ObservableCollection<ModuleInfoData> ModulesInfoData { get; set; }
        public ModulesContext()
        {
            ModulesInfoData = new ObservableCollection<ModuleInfoData>();
        }
    }
}
