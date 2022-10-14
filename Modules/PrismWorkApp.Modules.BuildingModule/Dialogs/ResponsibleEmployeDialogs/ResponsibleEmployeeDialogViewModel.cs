using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class ResponsibleEmployeeDialogViewModel : ResponsibleEmployeeViewModel, IDialogAware
    {
            public ResponsibleEmployeeDialogViewModel(IDialogService dialogService )
            :base(dialogService)
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
       override  public  void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldResponsibleEmployee>(SelectedResposibleEmployee, 
                    "Сохранить", "отвествественного работника", 
                    "Сохранить",
                     "Не сохранять",
                    "Отмена", (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                   //     CoreFunctions.CopyObjectReflectionNewInstances(SelectedResposibleEmployee, ResivedResposibleEmployee);
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
        
        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)parameters.GetValue<object>("selected_element_conveyance_object");
            if (navigane_message != null)
            {
                ResivedResposibleEmployee = (bldResponsibleEmployee)navigane_message.Object;
                EditMode = navigane_message.EditMode;
              //  Objects = (bldObjectsGroup)parameters.GetValue<object>("current_collection");
               // AllObjects = (bldObjectsGroup)parameters.GetValue<object>("common_collection");
                if (SelectedResposibleEmployee != null) SelectedResposibleEmployee.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedResposibleEmployee = new SimpleEditableResposibleEmployee();
                SelectedResposibleEmployee.ErrorsChanged += RaiseCanExecuteChanged;
            //    CoreFunctions.CopyObjectReflectionNewInstances(ResivedResposibleEmployee, SelectedResposibleEmployee);
             
            }

        }
    }
}
