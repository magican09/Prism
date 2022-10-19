using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

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

        public ParticipantDialogViewModel(IDialogService dialogService)
            :base(dialogService)
        {

        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
          
        }
       public override  void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldParticipant>(SelectedParticipant,
                    "Сохранить", "участника строительства",
                    "Сохрать", "Не сохранять","Отмена", (result) => {
                    if (result.Result == ButtonResult.Yes)
                    {
                    //    CoreFunctions.CopyObjectReflectionNewInstances(SelectedParticipant, ResivedParticipant);
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
                    }
                    else
                    {

                        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                    }

                }, _dialogService);

            }
            else
            {
                // bldObject new_bldObject = new bldObject();
                //  CoreFunctions.CopyObjectReflectionNewInstances(SelectedObject, new_bldObject);


            }


        }
        override public void OnClose(object obj)
        {
            if (EditMode) SelectedParticipant.UnDo(Id);
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigate_message =(ConveyanceObject) parameters.GetValue<object>("selected_element_conveyance_object");
            CurrentContextId = (Guid)parameters.GetValue<object>("current_context_id");
            if (navigate_message!=null)
            {
                ResivedParticipant =(bldParticipant) navigate_message.Object;
                EditMode = navigate_message.EditMode;
                if (!EditMode)
                    Id = CurrentContextId;

                if (SelectedParticipant != null) SelectedParticipant.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedParticipant = ResivedParticipant;
                SelectedParticipant.ErrorsChanged += RaiseCanExecuteChanged;
            }
        }
    }
}
