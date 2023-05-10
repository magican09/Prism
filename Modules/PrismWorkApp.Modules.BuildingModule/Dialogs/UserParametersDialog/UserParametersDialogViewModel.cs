using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class UserParametersDialogViewModel : BaseViewModel<AppSettingsSystem>, IDialogAware
    {
        private string _title = "Настройки";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public AppSettingsSystem _selectedAppSettingsSystem;
        public AppSettingsSystem SelectedAppSettingsSystem
        {
            get { return _selectedAppSettingsSystem; }
            set { SetProperty(ref _selectedAppSettingsSystem, value); }

        }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public UserParametersDialogViewModel(IDialogService dialogService, IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            UnDoReDo = new UnDoReDoSystem();
            _dialogService = dialogService;

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedAppSettingsSystem);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

        }


        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
          //  if (SelectedAppSettingsSystem != null)
             //   return !SelectedAppSettingsSystem.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
          //  else
                return false;
        }
        public virtual void OnSave()
        {
          //  base.OnSave<AppSettingsSystem>(SelectedAppSettingsSystem);
        }
        public virtual void OnClose(object obj)
        {
         //   base.OnClose<AppSettingsSystem>(obj, SelectedAppSettingsSystem);

        }
        //override public void OnSave()
        //{
        //    if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
        //    {
        //        CoreFunctions.ConfirmActionOnElementDialog<AppSettingsSystem>(SelectedAppSettingsSystem,
        //            "Сохранить", "насройки",
        //            "Сохранить",
        //             "Не сохранять",
        //            "Отмена", (result) =>
        //            {
        //                if (result.Result == ButtonResult.Yes)
        //                {
        //                    DialogParameters param = new DialogParameters();
        //                    param.Add("undo_redo", UnDoReDo);
        //                    RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
        //                }
        //                else
        //                {
        //                    RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        //                }
        //            }, _dialogService);
        //    }
        //    else
        //    {
        //    }
        //}
        //override public void OnClose(object obj)
        //{
        //    UnDoReDo.UnDoAll();
        //    RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

        //}

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)parameters.GetValue<object>("selected_app_settings_system");
            if (navigane_message != null)
            {
                SelectedAppSettingsSystem = (AppSettingsSystem)navigane_message.Object;
                EditMode = navigane_message.EditMode;
              //  if (SelectedAppSettingsSystem != null) SelectedAppSettingsSystem.ErrorsChanged -= RaiseCanExecuteChanged;
              //  SelectedAppSettingsSystem.ErrorsChanged += RaiseCanExecuteChanged;
               // UnDoReDo.Register(SelectedAppSettingsSystem);
             //   UnDoReDo.Register(SelectedAppSettingsSystem.AppSettings);
            }
        }
    }
}
