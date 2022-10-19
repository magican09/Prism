﻿using System;
using System.Collections.Generic;
using System.Text;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldConstructionToCollectionViewModel :
        AddElementToCollectionDialogViewModel<bldConstructionsGroup,bldConstruction>
    {
        public AddbldConstructionToCollectionViewModel(IDialogService  dialogService)
            :base(dialogService)
        {
                
        }
    }
}