using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldObjectToCollectionViewModel :
        AddElementToCollectionDialogViewModel<ObservableCollection<bldObject>, bldObject>
    {
        public AddbldObjectToCollectionViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
