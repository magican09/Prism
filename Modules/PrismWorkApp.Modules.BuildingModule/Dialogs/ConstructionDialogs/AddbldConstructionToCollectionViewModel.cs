using System;
using System.Collections.Generic;
using System.Text;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldConstructionToCollectionViewModel :
        AddElementToCollectionDialogViewModel<bldConstructionsGroup,bldConstruction>
    {
        public AddbldConstructionToCollectionViewModel(IDialogService  dialogService, IPropertiesChangeJornal propertiesChangeJornal)
            :base(dialogService,  propertiesChangeJornal)
        {
                
        }
    }
}
