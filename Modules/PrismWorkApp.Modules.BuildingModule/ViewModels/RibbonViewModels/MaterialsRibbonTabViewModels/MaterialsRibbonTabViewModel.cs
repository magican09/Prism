using Prism;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class MaterialsRibbonTabViewModel:LocalBindableBase, INotifyPropertyChanged, IActiveAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public NotifyCommand LoadMaterialsFromAccessCommand { get; private set; }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string _title = "Импорт";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public IbldMaterialsUnitsRepository _bldMaterialsUnitsRepository;
      
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        private readonly IRegionManager _regionManager;
        public MaterialsRibbonTabViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                            IbldMaterialsUnitsRepository  bldMaterialsUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _bldMaterialsUnitsRepository = bldMaterialsUnitsRepository;
            _applicationCommands = applicationCommands;
            IsActiveChanged += OnActiveChanged;
            LoadMaterialsFromAccessCommand = new NotifyCommand(OnLoadMaterialsFromAccess);
           // ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialsFromAccessCommand);

        }

        private void OnLoadMaterialsFromAccess()
        {
         bldMaterialsGroup materials = new bldMaterialsGroup();
            Functions.OnLoadMaterialsFromAccess(materials);
            NavigationParameters navParam = new NavigationParameters();
            navParam.Add("bld_materials", new ConveyanceObject(materials, ConveyanceObjectModes.EditMode.FOR_EDIT));
            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialsGroupView).Name, navParam);

        }


        private void OnActiveChanged(object sender, EventArgs e)
        {
            if(IsActive)
            {
                ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialsFromAccessCommand);
            }
            else
            {
                ApplicationCommands.LoadMaterialsFromAccessCommand.UnregisterCommand(LoadMaterialsFromAccessCommand);
            }
        }
    }
}
