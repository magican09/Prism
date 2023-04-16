using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class AppObjectsModel : BindableBase, IAppObjectsModel//,IActiveAware
    {
        private NameableObservableCollection<IEntityObject> _allModels = new NameableObservableCollection<IEntityObject>();

        public NameableObservableCollection<IEntityObject> AllModels
        {
            get { return _allModels; }
        }
        private bldDocumentsGroup _documentation = new bldDocumentsGroup();
        public bldDocumentsGroup Documentation
        {
            get { return _documentation; }
            set { SetProperty(ref _documentation, value); }
        }

        #region Commands 
        //        public NotifyMenuCommands DocumentsGroupCommands { get; set; } = new NotifyMenuCommands();
        #endregion
        #region Contructors
        private IApplicationCommands _applicationCommands;
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;
        public NotifyCommand SaveAllToDBCommand { get; set; }
        IUnDoReDoSystem UnDoReDo;
        public AppObjectsModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDoSystem)
        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
            UnDoReDo = unDoReDoSystem;
            UnDoReDo.Register(AllModels);
            
            //    UnDoReDo = unDoReDoSystem;
            Documentation.Name = "Документация";
            SaveAllToDBCommand = new NotifyCommand(OnSaveAllToDB);
            //LoadAggregationDocumentFromDBCommand = new NotifyCommand<object>(OnLoadAggregationDocumentFromDB);
            //LoadAggregationDocumentFromDBCommand.Name = "Загрузить ведомость документов из БД";
            // CreateNewAggregationDocumentCommand = new NotifyCommand<object>(OnCreateNewAggregationDocument);
            //CreateNewAggregationDocumentCommand.Name = "Добавить новую ведомость документов";
            //RemoveAggregationDocumentCommand = new NotifyCommand<object>(OnRemoveAggregationDocument);
            //RemoveAggregationDocumentCommand.Name = "Удалить ведомость документов";

            //CreateNewMaterialCertificateCommand = new NotifyCommand<object>(OnCreateNewMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate || SelectedDocument is bldMaterialCertificate).ObservesProperty(()=>SelectedDocument);
            //CreateBasedOnMaterialCertificateCommand = new NotifyCommand<object>(OnCreateBasedOnMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate).ObservesProperty(() => SelectedDocument);
            //RemoveMaterialCertificateCommand = new NotifyCommand<object>(OnRemoveMaterialCertificate, (ob) => SelectedDocument is bldMaterialCertificate).ObservesProperty(() => SelectedDocument);

            _applicationCommands.SaveAllToDBCommand.RegisterCommand(SaveAllToDBCommand);
        }

   

        #endregion
        #region Model data 
        #region Documentation
        /// <summary>
        /// Коллекция для хранения документации
        /// </summary>
        #endregion

        #endregion

        #region Save 
        public void OnSaveAllToDB()
        {
            CoreFunctions.ConfirmActionDialog("Сохранить все изменения в БД?", "",
                "Сохранить", "Отмена", (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          var all_changed_objects = UnDoReDo._RegistedModels.Keys.Where(ob => ob.IsDbBranch && ob.State != EntityState.Unchanged).ToList();
                          UnDoReDo.SaveAll();
                          _buildingUnitsRepository.Complete(UnDoReDo);
                      }
                  }, _dialogService);

        }

        #endregion
    }
}
