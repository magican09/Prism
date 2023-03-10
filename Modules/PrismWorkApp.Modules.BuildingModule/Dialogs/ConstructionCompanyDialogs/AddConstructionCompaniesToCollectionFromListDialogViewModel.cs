using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddConstructionCompaniesToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldConstructionCompanyGroup, bldConstructionCompany>
    {
        public AddConstructionCompaniesToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
