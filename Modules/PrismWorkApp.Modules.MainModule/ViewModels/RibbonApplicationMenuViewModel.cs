using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismWorkApp.Core.Console;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.MainModule.ViewModels
{
    public class RibbonApplicationMenuViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator _evetnAggregator;
        private readonly IModulesContext _modulesContext;

        private ObservableCollection<ModuleInfoData> _modulesInfoData;
        public ObservableCollection<ModuleInfoData> ModulesInfoData
        {
            get { return _modulesInfoData; }
            set { SetProperty(ref _modulesInfoData, value); }
        }

        public RibbonApplicationMenuViewModel(IEventAggregator evetnAggregator, IModulesContext modulesContext)
        {
            _evetnAggregator = evetnAggregator;
            _modulesContext = modulesContext;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}
