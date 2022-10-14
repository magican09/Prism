using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class WorkDialogViewModel : WorkViewModel,IDialogAware
    {
        private Guid _currentContextId;

        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }

        public WorkDialogViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
            :base(dialogService, regionManager, buildingUnitsRepository)
        {

        }
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            //throw new NotImplementedException();
        }
         override  public void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldWork>(SelectedWork, 
                    "Сохранить", "работа",
                    "Сохранить",
                     "Не сохранять",
                    "Отмена",  (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        if (EditMode) SelectedWork.SaveAll(Id);
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
                    }
                    else
                    {
                        if (EditMode) SelectedWork.SaveAll(Id);
                        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                    }
                }, _dialogService);

            }
            else
            {
               // bldObject new_bldObject = new bldObject();
              //  CoreFunctions.CopyObjectReflectionNewInstances(SelectedBuildingObject, new_bldObject);
             

            }


        }
        
        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)parameters.GetValue<object>("selected_element_conveyance_object");
            if (navigane_message != null)
            {
                ResivedWork = (bldWork)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                if (!EditMode)
                    Id = CurrentContextId;
                if (SelectedWork != null) SelectedWork.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedWork = ResivedWork;
                SelectedWork.ErrorsChanged += RaiseCanExecuteChanged;
            //    CoreFunctions.CopyObjectReflectionNewInstances(ResivedWork, SelectedWork);
             
            }

        }
    }
}
