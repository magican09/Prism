using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class AppObjectsModel : BindableBase, IAppObjectsModel//,IActiveAware
    {
        private bldAggregationDocument _documentation = new bldAggregationDocument();
        public bldAggregationDocument Documentation
        {
            get { return _documentation; }
            set { SetProperty(ref _documentation, value); }
        }

        //private bldDocumentsGroup _documentation = new bldDocumentsGroup();
        //public bldDocumentsGroup Documentation
        //{
        //    get { return _documentation; }
        //    set { SetProperty(ref _documentation, value); }
        //}

        #region Selected Object Properties
        //private bldDocument _selectedDocument;
        //public bldDocument SelectedDocument
        //{
        //    get { return _selectedDocument; }
        //    set { _selectedDocument = value; }
        //}

        //private bldDocumentsGroup _selectedDocumentsGroup;
        //public bldDocumentsGroup SelectedDocumentsGroup
        //{
        //    get { return _selectedDocumentsGroup; }
        //    set { _selectedDocumentsGroup = value; }
        //}
        #endregion
        #region Commands 
        //        public NotifyMenuCommands DocumentsGroupCommands { get; set; } = new NotifyMenuCommands();
        #endregion
        #region Contructors
        private IApplicationCommands _applicationCommands;
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;
        public NotifyCommand<object> LoadAggregationDocumentFromDBCommand { get; set; }

        public AppObjectsModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDoSystem)
        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
            //    UnDoReDo = unDoReDoSystem;
            Documentation.AttachedDocuments.Name = "Документация";

            LoadAggregationDocumentFromDBCommand = new NotifyCommand<object>(OnLoadAggregationDocumentFromDB);
            LoadAggregationDocumentFromDBCommand.Name = "Загрузить ведомость документов из БД";
            // CreateNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument);
            //CreateNewAggregationDocumentCommand.Name = "Добавить новую ведомость документов";
            //RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument);
            //RemoveAggregationDocumentCommand.Name = "Удалить ведомость документов";

            //CreateNewMaterialCertificateCommand = new NotifyCommand<object>(OnCreateNewMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate || SelectedDocument is bldMaterialCertificate).ObservesProperty(()=>SelectedDocument);
            //CreateBasedOnMaterialCertificateCommand = new NotifyCommand<object>(OnCreateBasedOnMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate).ObservesProperty(() => SelectedDocument);
            //RemoveMaterialCertificateCommand = new NotifyCommand<object>(OnRemoveMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate).ObservesProperty(() => SelectedDocument);


        }

        //private void OnCreateNewMaterialCertificate(object new_cetificate)
        //{
        //    bldMaterialCertificate certificate = new bldMaterialCertificate();
        //    SelectedDocument.AttachedDocuments.Add(certificate);
        //    new_cetificate = certificate;
        //}
        //private void OnCreateBasedOnMaterialCertificate(object obj)
        //{

        //}
        //private void OnRemoveMaterialCertificate(object obj)
        //{

        //}


        #region bldDocumentaation  services



        //public NotifyCommand<object> CreateNewAggregationDocumentCommand { get; set; }
        //public NotifyCommand<object> RemoveAggregationDocumentCommand { get; private set; }

        //public NotifyCommand<object> CreateNewMaterialCertificateCommand { get; set; }
        //public NotifyCommand<object> CreateBasedOnMaterialCertificateCommand { get; set; }
        //public NotifyCommand<object> RemoveMaterialCertificateCommand { get; private set; }

        //private void OnRemoveAggregationDocument(object selected_object)
        //{

        //   if(((IHierarchical)selected_object).Parents.Count>1) CoreFunctions.GetElementFromCollectionWhithDialog(((IHierarchical)selected_object).Parents, (IEntityObject)selected_object,
        //        _dialogService,(result)=>{ },nameof(GetObjectFromCollectionDialogVeiw),
        //        "Выберите ведомость из которой удалить документ",
        //        "Сообщиение!!");
        //    else
        //    CoreFunctions.RemoveFromParentObject(((IHierarchical)selected_object).Parents[0], selected_object,
        //        "ведомость", ((INameable)selected_object).Name, _dialogService);

        //}
        //private void OnCreateNewAggregationDocument(object selected_object)
        //{
        //    if (selected_object is IbldDocumentsGroup)
        //    {
        //        bldAggregationDocument new_agr_doc = new bldAggregationDocument();
        //        new_agr_doc.Name = "Новый каталог";
        //        new_agr_doc.Id = Guid.NewGuid();
        //        (selected_object as IbldDocumentsGroup).Add(new_agr_doc as bldDocument);
        //        UnDoReDo.Register(new_agr_doc);
        //    }
        //}
        private void OnLoadAggregationDocumentFromDB(object loaded_object)
        {


        }

        #endregion

        #endregion
        #region Model data 
        #region Documentation
        /// <summary>
        /// Коллекция для хранения документации
        /// </summary>
        #endregion

        #endregion

        #region Save 
        public void OnSaveDocumentationToDB()
        {
            CoreFunctions.ConfirmActionDialog("Сохранить все изменения в документации БД?", "Документация",
                "Сохранить", "Отмена", (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          foreach (bldDocument document in Documentation.AttachedDocuments)
                          {
                              if (_buildingUnitsRepository.DocumentsRepository.Get(document.Id) == null)
                                  _buildingUnitsRepository.DocumentsRepository.Add(document);
                          }
                          _buildingUnitsRepository.Complete();
                      }
                  }, _dialogService);

        }

        #endregion
    }
}
