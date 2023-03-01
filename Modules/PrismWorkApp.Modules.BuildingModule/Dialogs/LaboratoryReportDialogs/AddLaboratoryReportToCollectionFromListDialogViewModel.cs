using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddLaboratoryReportToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldLaboratoryReportsGroup, bldLaboratoryReport>
    {
        public AddLaboratoryReportToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
