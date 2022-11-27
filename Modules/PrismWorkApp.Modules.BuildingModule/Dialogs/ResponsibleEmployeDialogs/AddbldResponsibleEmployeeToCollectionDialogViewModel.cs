using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldResponsibleEmployeeToCollectionDialogViewModel :
      AddElementToCollectionDialogViewModel<bldResponsibleEmployeesGroup, bldResponsibleEmployee>
    {
        public AddbldResponsibleEmployeeToCollectionDialogViewModel(IDialogService dialogService)
           : base(dialogService)
        {

        }
    }
}
