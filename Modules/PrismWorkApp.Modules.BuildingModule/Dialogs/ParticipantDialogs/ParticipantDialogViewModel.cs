using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class ParticipantDialogViewModel : ParticipantViewModel, IDialogAware
    {
        public event Action<IDialogResult> RequestClose;
        private Guid _currentContextId;

        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }

        public ParticipantDialogViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
           : base(dialogService, regionManager, buildingUnitsRepository)
        {

        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }
        public override void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldParticipant>(SelectedParticipant,
                    "Сохранить", "участника строительства",
                    "Сохрать", "Не сохранять", "Отмена", (result) =>
                    {
                        if (result.Result == ButtonResult.Yes)
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("undo_redo", UnDoReDo);
                            RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
                        }
                        else
                        {

                            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                        }

                    }, _dialogService);

            }
            else
            {
            
            }


        }
        override public void OnClose(object obj)
        {
            UnDoReDo.UnDoAll();
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigate_message = (ConveyanceObject)parameters.GetValue<object>("selected_element_conveyance_object");
           if (navigate_message != null)
            {
                ResivedParticipant = (bldParticipant)navigate_message.Object;
                EditMode = navigate_message.EditMode;
                Id = CurrentContextId;

                if (SelectedParticipant != null) SelectedParticipant.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedParticipant = ResivedParticipant;
                SelectedParticipant.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo = new UnDoReDoSystem();
                UnDoReDo.Register(SelectedParticipant);
            }
        }
    }
}
