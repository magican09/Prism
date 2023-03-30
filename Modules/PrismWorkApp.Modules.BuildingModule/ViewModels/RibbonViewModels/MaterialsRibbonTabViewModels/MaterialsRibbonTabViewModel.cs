using Prism;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Dialogs;
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
    public class MaterialsRibbonTabViewModel : LocalBindableBase, IActiveAware
    {

        public NotifyCommand LoadMaterialCertificatesFromAccessCommand { get; private set; }
        public NotifyCommand LoadMaterialCertificatesFromDBCommand { get; private set; }
        public NotifyCommand SaveDataToDBCommand { get; private set; }
        private string _title = "Материалы";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        bldAggregationDocument TempCatalog { get; set; } = new bldAggregationDocument();

        private IDialogService _dialogService;
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        public MaterialsRibbonTabViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands)

        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
            _eventAggregator = eventAggregator;
            IsActiveChanged += OnActiveChanged;
            LoadMaterialCertificatesFromAccessCommand = new NotifyCommand(OnLoadMaterialsFromAccess);
            LoadMaterialCertificatesFromDBCommand = new NotifyCommand(OnLoadMaterialCertificatesFromDB);
            SaveDataToDBCommand = new NotifyCommand(OnSaveDataToDB);
            // ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialsFromAccessCommand);

        }

        private void OnLoadMaterialCertificatesFromDB()
        {
            bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(_buildingUnitsRepository.AggregationDocumentsRepository.GetAllAsync().ToList());

            CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                      (All_AggregationDocuments, _dialogService, (result) =>
                      {
                          if (result.Result == ButtonResult.Yes)
                          {
                              bldAggregationDocument selected_catalog = result.Parameters.GetValue<bldAggregationDocument>("element");

                              var navParam = new NavigationParameters();
                              navParam.Add("bld_aggregation_document", selected_catalog);
                              _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, typeof(DocumentationExplorerView).Name, navParam);


                          }

                      }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                      "Выберете каталог для сохранения",
                         "Форма для выбора каталога для загзузки из базы данных."
                        , "Перечень каталогов");

        }

        private void OnSaveDataToDB()
        {
            CoreFunctions.ConfirmActionDialog(
                "Cохранить в БД", "документация", "Сохранить", "Отмена", "Сохраниение в БД завершено!",
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         bldAggregationDocumentsGroup All_AggregationDocuments = new bldAggregationDocumentsGroup(_buildingUnitsRepository.AggregationDocumentsRepository.GetAll().ToList());
                         var current_catalog = All_AggregationDocuments.Where(ct => ct.Id == TempCatalog.Id).FirstOrDefault();
                         if (current_catalog == null)
                         {
                             CoreFunctions.SelectElementFromCollectionWhithDialog<bldAggregationDocumentsGroup, bldAggregationDocument>
                              (All_AggregationDocuments, _dialogService, (result) =>
                                {
                                    if (result.Result == ButtonResult.Yes)
                                    {
                                        current_catalog = result.Parameters.GetValue<bldAggregationDocument>("element");
                                        var dialog_par = new DialogParameters();
                                        dialog_par.Add("massege", current_catalog.Name);

                                        _dialogService.ShowDialog(typeof(InputTextValueDialog).Name, dialog_par,
                                            (diag_result) =>
                                            {
                                                if (diag_result.Result == ButtonResult.Yes)
                                                {
                                                    current_catalog.Name = diag_result.Parameters.GetValue<string>("input_text");

                                                }
                                                if (diag_result.Result == ButtonResult.Cancel)
                                                {
                                                    current_catalog.Name = "Новый каталог";
                                                }
                                            });


                                    }

                                }, typeof(SelectAggregationDocumentFromCollectionDialogView).Name,
                              "Выберете каталог для сохранения",
                                 "Форма для выбора каталога для загзузки из базы данных."
                                , "Перечень каталогов");
                         }


                         foreach (bldMaterialCertificate certificate in TempCatalog.AttachedDocuments)
                             if (!current_catalog.AttachedDocuments.Where(mc => (mc.Id == certificate.Id && mc.Id != Guid.Empty) ||
                             (mc.Name == certificate.Name && mc.RegId == certificate.RegId && mc.Date == certificate.Date)).Any())
                                 current_catalog.AttachedDocuments.Add(certificate);
                         if (!_buildingUnitsRepository.AggregationDocumentsRepository.Find(ad => ad.Id == current_catalog.Id).Any())
                         {
                             foreach(bldMaterialCertificate certificate in current_catalog.AttachedDocuments)
                             {
                                 if(!_buildingUnitsRepository.MaterialCertificates.Find(mc=>mc.Id==certificate.Id).Any())
                                 {
                                     _buildingUnitsRepository.MaterialCertificates.Add(certificate);
                                     _buildingUnitsRepository.Complete();
                                 }
                             }
                             _buildingUnitsRepository.AggregationDocumentsRepository.Add(current_catalog);
                         }
                         SaveDataToDBCommand.RaiseCanExecuteChanged();
                         _buildingUnitsRepository.Complete();

                     }
                 }, _dialogService);
        }

        private void OnLoadMaterialsFromAccess()
        {
            bldMaterialCertificatesGroup certificates = new bldMaterialCertificatesGroup();
            Functions.OnLoadMaterialCertificatesFromAccess(certificates);

            ObservableCollection<bldUnitOfMeasurement> units = new ObservableCollection<bldUnitOfMeasurement>();
            EntityCategory category = new EntityCategory();
            foreach (bldMaterialCertificate certificate in certificates)
            {
                bldUnitOfMeasurement measurement =
                    _buildingUnitsRepository.UnitOfMeasurementRepository.GetAllAsync().Where(el => el.Name == certificate.UnitOfMeasurement.Name).FirstOrDefault();
                if (measurement == null)
                {
                    _buildingUnitsRepository.UnitOfMeasurementRepository.Add(certificate.UnitOfMeasurement);
                }
                else
                    certificate.UnitOfMeasurement = measurement;
                //   Catalog.AttachedDocuments.Add(certificate);
            }
            _buildingUnitsRepository.Complete();
            TempCatalog.AttachedDocuments = (bldDocumentsGroup)certificates;
            TempCatalog.Name = "Загруженные документы";
            var navParam = new NavigationParameters();
            navParam.Add("bld_aggregation_document", TempCatalog);
            _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, typeof(DocumentationExplorerView).Name, navParam);
            //  navParam.Add("bld_documents", new ConveyanceObject(certificates, ConveyanceObjectModes.EditMode.FOR_EDIT));
            //_regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialCertificatesGroupView).Name, navParam);
            //EventMessage event_massage = new EventMessage();
            //event_massage.Recipient = "DocumentationExplorer";
            //event_massage.Value = Catalog;
            //_eventAggregator.GetEvent<MessageConveyEvent>().Publish(event_massage);

        }


        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialCertificatesFromAccessCommand);
            }
            else
            {
                ApplicationCommands.LoadMaterialsFromAccessCommand.UnregisterCommand(LoadMaterialCertificatesFromAccessCommand);
            }
        }
    }
}
