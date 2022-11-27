using Prism.Mvvm;
using Prism.Regions;

namespace PrismWorkApp.Modules.MainModule.ViewModels
{
    public class ToolBarViewModel : BindableBase, INavigationAware
    {
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
