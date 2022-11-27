using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldWorkToCollectionViewModel :
        AddElementToCollectionDialogViewModel<bldWorksGroup, bldWork>
    {
        public AddbldWorkToCollectionViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
