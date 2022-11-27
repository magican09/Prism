using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldWorkToCollectionViewModel :
        AddElementToCollectionDialogViewModel<ObservableCollection<bldWork>, bldWork>
    {
        public AddbldWorkToCollectionViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
