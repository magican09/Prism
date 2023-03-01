using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddMaterialToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldMaterialsGroup, bldMaterial>
    {
        public AddMaterialToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
