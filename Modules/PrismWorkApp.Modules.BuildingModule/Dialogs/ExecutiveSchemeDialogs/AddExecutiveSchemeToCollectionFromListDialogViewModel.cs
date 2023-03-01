using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddExecutiveSchemeToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldExecutiveSchemesGroup, bldExecutiveScheme>
    {
        public AddExecutiveSchemeToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
