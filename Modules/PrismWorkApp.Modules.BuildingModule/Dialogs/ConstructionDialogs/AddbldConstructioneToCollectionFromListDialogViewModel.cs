﻿using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldConstructioneToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldConstructionsGroup, bldConstruction>
    {
        public AddbldConstructioneToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
