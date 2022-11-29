using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldResponsibleEmployeeToCollectionDialogViewModel :
      AddElementToCollectionDialogViewModel<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>
    {
        public AddbldResponsibleEmployeeToCollectionDialogViewModel(IDialogService dialogService)
           : base(dialogService)
        {

        }
    }
}
