using Prism;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ToolBarRibbonTabViewModel : LocalBindableBase, IActiveAware
    {
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        private IAppSettingsSystem _appSettings;
        private IDialogService _dialogService;
        public NotifyCommand OpenAppSetingsCommand { get; private set; }
        public ToolBarRibbonTabViewModel(IDialogService dialogService, IApplicationCommands applicationCommands, IAppSettingsSystem appSettings)
        {
            ApplicationCommands = applicationCommands;
            _appSettings = appSettings;
            _dialogService = dialogService;
            OpenAppSetingsCommand = new NotifyCommand(OnOpenAppSetings);
            ApplicationCommands.OpenAppSettingsDialogCommand.RegisterCommand(OpenAppSetingsCommand);

        }

        private void OnOpenAppSetings()
        {
            DialogParameters param = new DialogParameters();
            param.Add("selected_app_settings_system", new ConveyanceObject(_appSettings, false));
            _dialogService.ShowDialog(nameof(UserParametersDialogView), param,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {

                    }
                    if (result.Result == ButtonResult.No)
                    {

                    }

                });

        }

        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {

            }
            else
            {

            }
        }
    }
}
