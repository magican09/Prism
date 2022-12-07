﻿using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddLaboratoryReportToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<ObservableCollection<bldLaboratoryReport>, bldLaboratoryReport>
    {
        public AddLaboratoryReportToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
