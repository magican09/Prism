using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule;
using PrismWorkApp.Modules.MainModule;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services;
using PrismWorkApp.Services.Interfaces;
using PrismWorkApp.Services.Repositories;
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
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.RegisterSingleton<IModulesContext, ModulesContext>();
            
            containerRegistry.RegisterSingleton<IAppSettingsSystem, AppSettingsSystem>();
          
            containerRegistry.RegisterSingleton<IBuildingUnitsRepository, BuildingUnitsRepository>();
         //   containerRegistry.RegisterSingleton<IbldMaterialsUnitsRepository, bldMaterialsUnitsRepository>();
            containerRegistry.RegisterSingleton<IUnDoReDoSystem, UnDoReDoSystem>();
            
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();
            containerRegistry.RegisterDialog<ConfirmActionDialog, ConfirmActionDialogViewModel>();
            containerRegistry.RegisterDialog<InputTextValueDialog, InputTextValueDialogViewModel>();
            
            containerRegistry.RegisterDialog<ConfirmActionWhithoutCancelDialog, ConfirmActionWhithoutCancelDialogViewModel>();
            containerRegistry.RegisterDialogWindow<CommonDialogWindow>();

            ;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<BuildingModule>();
            moduleCatalog.AddModule<MainModule>();
        }
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName.Replace("Views", "ViewModels");
                //viewName = viewName.TrimEnd("View");
                var viewAssemblyName = viewType.Assembly.FullName;
                var viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
                viewModelName = viewModelName.Replace("ViewView", "View");
                return Type.GetType(viewModelName);

            });

        }

        private void PrismApplication_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Обнаружено не обработанное исключение: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void PrismApplication_Startup(object sender, StartupEventArgs e)
        {
            //Window splash_window = new SplashWindow();
            //splash_window.Title = "Заставка";
            //splash_window.Show();
            //System.Threading.Thread.Sleep(1000);
            //splash_window.Hide();


        }
    }
}
