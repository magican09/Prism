using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldConstructioneToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldConstructionsGroup, bldConstruction>
    {
        public AddbldConstructioneToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
