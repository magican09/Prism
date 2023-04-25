using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System.Collections;
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
        public NotifyCommand SaveAllToDBCommand { get; set; }
        public NotifyCommand<object> AddNewMaterialCertificateCommand { get; private set; }
        public NotifyCommand<object> AddNewAggregationDocumentCommand { get; private set; }
        public NotifyCommand<object> AddNewLaboratoryReportCommand { get; private set; }
        public NotifyCommand<object> AddNewExecutiveSchemeCommand { get; private set; }
        #endregion
        #region Contructors
        private IApplicationCommands _applicationCommands;
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;
    
        IUnDoReDoSystem UnDoReDo;
        public AppObjectsModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                           IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDoSystem)
        {
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _dialogService = dialogService;
            _applicationCommands = applicationCommands;
            UnDoReDo = unDoReDoSystem;
            UnDoReDo.Register(AllModels,true,false);
            
            //    UnDoReDo = unDoReDoSystem;
            Documentation.Name = "Документация";
            SaveAllToDBCommand = new NotifyCommand(OnSaveAllToDB);
            AddNewMaterialCertificateCommand = new NotifyCommand<object>(OnAddNewMaterialCertificate);
            AddNewAggregationDocumentCommand = new NotifyCommand<object>(OnAddNewAggregationDocument);
            AddNewLaboratoryReportCommand = new NotifyCommand<object>(OnAddNewLaboratoryReport);
            AddNewExecutiveSchemeCommand = new NotifyCommand<object>(OnAddNewExecutiveScheme);
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

        private void OnAddNewExecutiveScheme(object obj)
        {
            if (obj is IList list_obj)
            {
                list_obj.Add(new bldExecutiveScheme());

            }
        }

        private void OnAddNewLaboratoryReport(object obj)
        {
            if (obj is IList list_obj)
                list_obj.Add(new bldLaboratoryReport());

        }

        private void OnAddNewAggregationDocument(object obj)
        {
            if (obj is IList list_obj)
                list_obj.Add(new bldAggregationDocument());
            
        }

        private void OnAddNewMaterialCertificate(object obj)
        {
         if(obj is IList list_obj)
              list_obj.Add(new bldMaterialCertificate());
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
                          var res_massage = result.Parameters.GetValue<string>("confirm_dialog_param");
                          var p = new DialogParameters();
                          p.Add("message", $"Готово!");
                          _dialogService.ShowDialog(typeof(MessageDialog).Name, p, (r) => { });
                      }
                  }, _dialogService);

        }

        #endregion
    }
}
