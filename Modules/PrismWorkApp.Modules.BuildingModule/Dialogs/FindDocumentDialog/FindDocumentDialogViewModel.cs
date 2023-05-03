using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.ViewModels;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class FindDocumentDialogViewModel : FindDocumentViewModel, IDialogAware
    {
        public FindDocumentDialogViewModel(IDialogService dialogService,
         IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)
             : base( dialogService,regionManager,  buildingUnitsRepository,  applicationCommands,  appObjectsModel)
        {

        }
      
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
           
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
           
        }
    }
}
