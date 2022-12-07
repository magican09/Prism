using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddMaterialToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<ObservableCollection<bldMaterial>, bldMaterial>
    {
        public AddMaterialToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
