using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddUnitOfMeasurementToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>
    {
        public AddUnitOfMeasurementToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
