using Prism;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class DocumentsRibbonTabViewModel : BaseViewModel<IEntityObject>, IActiveAware
    {

        public NotifyCommand LoadMaterialCertificatesFromAccessCommand { get; private set; }
        public NotifyCommand LoadMaterialCertificatesFromDBCommand { get; private set; }
        public NotifyCommand FindDocumentCommand { get; private set; }

        private string _title = "Документация";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        bldAggregationDocument TempCatalog { get; set; } = new bldAggregationDocument();

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }

        private IApplicationCommands _applicationCommands;
        private readonly IEventAggregator _eventAggregator;
        private AppObjectsModel _appObjectsModel;
        public DocumentsRibbonTabViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel)

        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            ApplicationCommands = applicationCommands;
            _appObjectsModel = appObjectsModel as AppObjectsModel;
            _eventAggregator = eventAggregator;
              IsActiveChanged += OnActiveChanged;
            LoadMaterialCertificatesFromAccessCommand = new NotifyCommand(OnLoadMaterialsFromAccess);
            

       
           FindDocumentCommand = new NotifyCommand(OnFindDocument);
           ApplicationCommands.FindDocumentCommand.RegisterCommand(FindDocumentCommand);

        }

        private void OnFindDocument()
        {
        // DialogParameters parms  = new DialogParameters();

          _dialogService.ShowDialog(nameof(FindDocumentDialogView));

        }

        private void OnLoadMaterialsFromAccess()
        {
            bldMaterialCertificatesGroup certificates = new bldMaterialCertificatesGroup();
            Functions.OnLoadMaterialCertificatesFromAccess(certificates);

            ObservableCollection<bldUnitOfMeasurement> units = new ObservableCollection<bldUnitOfMeasurement>();
            bldAggregationDocument aggregationDocument = new bldAggregationDocument("Загруженные сертификаты");

            TypeOfFile typeOfFile = _buildingUnitsRepository.TypesOfFileRepository.GetAllAsync().Where(el => el.Name == "PDF").FirstOrDefault();
            if(typeOfFile==null)
            {
                typeOfFile = new TypeOfFile();
                typeOfFile.Name = "PDF";
                typeOfFile.Extention = "*.pdf";
                _buildingUnitsRepository.TypesOfFileRepository.Add(typeOfFile);
                _buildingUnitsRepository.Complete();

            }
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
                certificate.ImageFile.FileType = typeOfFile;
                aggregationDocument.AttachedDocuments.Add(certificate);
            }
            _buildingUnitsRepository.DocumentsRepository.AggregationDocuments.Add(aggregationDocument);
            aggregationDocument.IsDbBranch = true;
            _appObjectsModel.Documentation.Add(aggregationDocument);
            //_buildingUnitsRepository.Complete();
            //TempCatalog.AttachedDocuments = (bldDocumentsGroup)certificates;
            //TempCatalog.Name = "Загруженные документы";
            //var navParam = new NavigationParameters();
            //navParam.Add("bld_document", TempCatalog);
            //_regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, typeof(DocumentationExplorerView).Name, navParam);
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
             //   ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialCertificatesFromAccessCommand);
            }
            else
            {
             //   ApplicationCommands.LoadMaterialsFromAccessCommand.UnregisterCommand(LoadMaterialCertificatesFromAccessCommand);
            }
        }
    }
}
