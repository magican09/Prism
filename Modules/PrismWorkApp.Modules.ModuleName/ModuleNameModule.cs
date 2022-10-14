using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismWorkApp.Core;
using PrismWorkApp.Modules.ModuleName.Views;

namespace PrismWorkApp.Modules.ModuleName
{
    public class ModuleNameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleNameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "ProjectExplorerView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<ProjectExplorerView>();

        }
    }
}