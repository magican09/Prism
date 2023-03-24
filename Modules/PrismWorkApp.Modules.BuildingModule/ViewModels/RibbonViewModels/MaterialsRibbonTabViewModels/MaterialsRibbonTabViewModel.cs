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
using System.Collections.Generic;
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

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        private readonly IRegionManager _regionManager;
        public MaterialsRibbonTabViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;

            _applicationCommands = applicationCommands;
            IsActiveChanged += OnActiveChanged;
            LoadMaterialsFromAccessCommand = new NotifyCommand(OnLoadMaterialsFromAccess);
           // ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialsFromAccessCommand);

        }

        private void OnLoadMaterialsFromAccess()
        {
         bldMaterialCertificatesGroup  certificates    = new bldMaterialCertificatesGroup();
            Functions.OnLoadMaterialCertificatesFromAccess(certificates);
            NavigationParameters navParam = new NavigationParameters();
            bldDocument chapter = new bldDocument("Загруженные");

            ObservableCollection<bldUnitOfMeasurement> units = new ObservableCollection<bldUnitOfMeasurement>();
            EntityCategory category = new EntityCategory();
            foreach (bldMaterialCertificate  certificate in certificates)
            {
                bldUnitOfMeasurement measurement = units.Where(el => el.Name == certificate.UnitOfMeasurement.Name).FirstOrDefault();
                if (measurement == null)
                {
                    units.Add(certificate.UnitOfMeasurement);
                }
                else
                    certificate.UnitOfMeasurement = measurement;

              if (!_buildingUnitsRepository.MaterialCertificates.Find(mc=>mc.RegId==certificate.RegId).Any())
                    _buildingUnitsRepository.MaterialCertificates.Add(certificate);
            }
                //  _buildingUnitsRepository.MaterialCertificates.Add(certificates);

                navParam.Add("bld_documents", new ConveyanceObject(certificates, ConveyanceObjectModes.EditMode.FOR_EDIT));
            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialCertificatesGroupView).Name, navParam);
            _buildingUnitsRepository.Complete();

            //foreach (bldMaterial matr in materials)
            //{
            //    category.Resources.Add(matr);
            //    matr.Category = category;
            //}
            //_bldMaterialsUnitsRepository.Complete();


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
