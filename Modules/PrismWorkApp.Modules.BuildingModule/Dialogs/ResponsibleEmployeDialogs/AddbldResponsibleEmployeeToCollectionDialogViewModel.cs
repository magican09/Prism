using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldResponsibleEmployeeToCollectionDialogViewModel:
      AddElementToCollectionDialogViewModel<bldResponsibleEmployeesGroup, bldResponsibleEmployee>
    {
        public AddbldResponsibleEmployeeToCollectionDialogViewModel(IDialogService dialogService, IPropertiesChangeJornal propertiesChangeJornal)
           : base(dialogService,  propertiesChangeJornal)
        {

        }
    }
}
