using Prism.Ioc;
using Prism.Modularity;
using PrismWorkApp.Modules.ModuleName;
using PrismWorkApp.Services;
using PrismWorkApp.Services.Interfaces;
using PrismWorkApp.Views;
using System;
using System.Windows;

namespace PrismWorkApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
      
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
         //   containerRegistry.RegisterSingleton<IProjectRepository, ProjectRepository>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
