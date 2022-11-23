﻿using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class ResponsibleEmployeeDialogViewModel : ResponsibleEmployeeViewModel, IDialogAware
    {
            public ResponsibleEmployeeDialogViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository,
                 IApplicationCommands applicationCommands, IPropertiesChangeJornal propertiesChangeJornal)
              : base(dialogService, regionManager, buildingUnitsRepository, applicationCommands, propertiesChangeJornal)
        {

        }
        public event Action<IDialogResult> RequestClose;

        private Guid _currentContextId;
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }

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
                        base.OnSave<bldResponsibleEmployee>(SelectedResposibleEmployee);
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
                    }
                    else
                    {
                         RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                    }

                }, _dialogService);
            }
        }
        override public void OnClose(object obj)
        {
            //if (EditMode) SelectedResposibleEmployee.UnDoAll(Id);
            this.OnClose<bldResponsibleEmployee>(obj, SelectedResposibleEmployee);
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)parameters.GetValue<object>("selected_element_conveyance_object");
            CurrentContextId = (Guid)parameters.GetValue<object>("current_context_id");
            if (navigane_message != null)
            {
                ResivedResposibleEmployee = (bldResponsibleEmployee)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                if (CurrentContextId != Guid.Empty)
                    Id = CurrentContextId;
                else
                    Id = Guid.NewGuid();

                if (SelectedResposibleEmployee != null) SelectedResposibleEmployee.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedResposibleEmployee = ResivedResposibleEmployee;
                SelectedResposibleEmployee.ErrorsChanged += RaiseCanExecuteChanged;
        
             
            }

        }
    }
}
