using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldConstructionToCollectionViewModel :
        AddElementToCollectionDialogViewModel<bldConstructionsGroup, bldConstruction>
    {
        public AddbldConstructionToCollectionViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
