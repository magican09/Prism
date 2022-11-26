using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldObjectToCollectionViewModel :
        AddElementToCollectionDialogViewModel<ObservableCollection<bldObject>, bldObject>
    {
        public AddbldObjectToCollectionViewModel(IDialogService  dialogService)
            :base(dialogService)
        {
                
        }
    }
}
