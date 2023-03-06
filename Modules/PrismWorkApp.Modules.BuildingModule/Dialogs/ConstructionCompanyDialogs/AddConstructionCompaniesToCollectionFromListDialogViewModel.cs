using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

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
