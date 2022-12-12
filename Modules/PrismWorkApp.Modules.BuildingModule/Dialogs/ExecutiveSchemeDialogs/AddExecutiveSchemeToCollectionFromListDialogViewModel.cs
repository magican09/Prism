using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

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
