using System;
using System.Collections.Generic;
using System.Text;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldWorkToCollectionViewModel :
        AddElementToCollectionDialogViewModel<bldWorksGroup,bldWork>
    {
        public AddbldWorkToCollectionViewModel(IDialogService  dialogService, IPropertiesChangeJornal propertiesChangeJornal)
            :base(dialogService, propertiesChangeJornal)
        {
                
        }
    }
}
