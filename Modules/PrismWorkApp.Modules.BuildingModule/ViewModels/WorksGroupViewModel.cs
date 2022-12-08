using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class WorksGroupViewModel : BaseViewModel<bldWorksGroup>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Ведомость работ";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldWork _selectedWork;
        public bldWork SelectedWork
        {
            get { return _selectedWork; }
            set { SetProperty(ref _selectedWork, value); }
        }
        private bldWorksGroup _selectedWorksGroup;
        public bldWorksGroup SelectedWorksGroup
        {
            get { return _selectedWorksGroup; }
            set { SetProperty(ref _selectedWorksGroup, value); }
        }

        private ObservableCollection<bldWork>  _selectedWorks = new ObservableCollection<bldWork>();
        public ObservableCollection<bldWork> SelectedWorks
        {
            get { return _selectedWorks; }
            set { SetProperty(ref _selectedWorks, value); }
        }
        private bldConstruction _selectedConstruction;
        public bldConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set { SetProperty(ref _selectedConstruction, value); }
        }
        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyCommand<object> RemovePreviousWorkCommand { get; private set; }
        public NotifyCommand<object> RemoveNextWorkCommand { get; private set; }

        public ObservableCollection<INotifyCommand> NextWorksContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddNextWorkCommand { get; private set; }
          
        public ObservableCollection<INotifyCommand> UnitsOfMeasurementContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> SelectUnitOfMeasurementCommand { get; private set; }
        public NotifyCommand<object> RemoveUnitOfMeasurementCommand { get; private set; }

        public ObservableCollection<INotifyCommand> MaterialsContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddMaterialsCommand { get; private set; }
        public NotifyCommand<object> RemoveMaterialCommand { get; private set; }

        public ObservableCollection<INotifyCommand> ProjectDocumentationContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddProjectsDocumentCommand { get; private set; }
        public NotifyCommand<object> RemoveProjectDocumentCommand { get; private set; }
      
        public ObservableCollection<INotifyCommand>LaboratoryReportsContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddLaboratoryReportsCommand { get; private set; }
        public NotifyCommand<object> RemoveLaboratoryReportCommand { get; private set; }

        public ObservableCollection<INotifyCommand> ExecutiveSchemesContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddExecutiveSchemesCommand { get; private set; }
        public NotifyCommand<object> RemoveExecutiveSchemeCommand { get; private set; }

        public ObservableCollection<INotifyCommand> WorksContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddCreatedFromTemplateWorkCommand { get; private set; }
        public NotifyCommand<object> MoveWorksToAnotherConatructionCommand { get; private set; }
        public NotifyCommand DeleteWorkCommand { get; private set; }

        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
      

        public NotifyCommand AddNewWorkCommand { get; private set; }
        public NotifyCommand RemoveWorkCommand { get; private set; }


        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public WorksGroupViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {
            UnDoReDo = new UnDoReDoSystem();
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedConstruction);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            #region Commands Init
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            RemoveNextWorkCommand = new NotifyCommand<object>(OnRemoveNextWork);
            RemoveNextWorkCommand.Name = "Удалить";
            AddNextWorkCommand = new NotifyCommand<object>(OnAddNextWork);
            AddNextWorkCommand.Name = "Добавить";
            NextWorksContextMenuCommands.Add(AddNextWorkCommand);
            NextWorksContextMenuCommands.Add(RemoveNextWorkCommand);

            SelectUnitOfMeasurementCommand = new NotifyCommand<object>(OnSelectUnitOfMeasurement);
            SelectUnitOfMeasurementCommand.Name = "Установить";
            RemoveUnitOfMeasurementCommand = new NotifyCommand<object>(OnUnitOfMeasurement);
            RemoveUnitOfMeasurementCommand.Name = "Удалить";
            UnitsOfMeasurementContextMenuCommands.Add(SelectUnitOfMeasurementCommand);
            UnitsOfMeasurementContextMenuCommands.Add(RemoveUnitOfMeasurementCommand);
            AddMaterialsCommand = new NotifyCommand<object>(OnAddMaterials);
            AddMaterialsCommand.Name = "Добавить материалы";
            RemoveMaterialCommand = new NotifyCommand<object>(OnRemoveMaterial);
            RemoveMaterialCommand.Name = "Удалить материал";
            MaterialsContextMenuCommands.Add(AddMaterialsCommand);
            MaterialsContextMenuCommands.Add(RemoveMaterialCommand);

            AddProjectsDocumentCommand = new NotifyCommand<object>(OnAddProjectsDocuments);
            AddProjectsDocumentCommand.Name = "Добавить документацию";
            RemoveProjectDocumentCommand= new NotifyCommand<object>(OnRemoveProjectDocument);
            RemoveProjectDocumentCommand.Name = "Удалить документацию";
            ProjectDocumentationContextMenuCommands.Add(AddProjectsDocumentCommand);
            ProjectDocumentationContextMenuCommands.Add(RemoveProjectDocumentCommand);

            AddLaboratoryReportsCommand = new NotifyCommand<object>(OnAddLaboratoryReports);
            AddLaboratoryReportsCommand.Name = "Добавить доеументацию";
            RemoveLaboratoryReportCommand = new NotifyCommand<object>(OnRemoveLaboratoryReport);
            RemoveLaboratoryReportCommand.Name = "Удалить документацию";
            LaboratoryReportsContextMenuCommands.Add(AddLaboratoryReportsCommand);
            LaboratoryReportsContextMenuCommands.Add(RemoveLaboratoryReportCommand);

            AddExecutiveSchemesCommand = new NotifyCommand<object>(OnAddExecutiveSchemes);
            AddExecutiveSchemesCommand.Name = "Добавить исполнительные схемы";
            RemoveExecutiveSchemeCommand = new NotifyCommand<object>(OnRemoveExecutiveScheme);
            RemoveExecutiveSchemeCommand.Name = "Удалить исполнительную схему";
            ExecutiveSchemesContextMenuCommands.Add(AddExecutiveSchemesCommand);
            ExecutiveSchemesContextMenuCommands.Add(RemoveExecutiveSchemeCommand);

            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);
            
                AddNewWorkCommand = new NotifyCommand(OnAddNewWork);
            AddNewWorkCommand.Name = "Создать новую работу";
            DeleteWorkCommand = new NotifyCommand(OnDeleteWork,()=> { return SelectedWorks.Count>0;}).ObservesPropertyChangedEvent(SelectedWorks);
            DeleteWorkCommand.Name = "Удалить";
            _applicationCommands.CreateNewWorkCommand.RegisterCommand(AddNewWorkCommand);
            _applicationCommands.DeleteWorksCommand.RegisterCommand(DeleteWorkCommand);

            AddCreatedFromTemplateWorkCommand = new NotifyCommand<object>(OnAddCreatedFromTemplateWork);
            AddCreatedFromTemplateWorkCommand.Name = "Создать на основании";
            MoveWorksToAnotherConatructionCommand = new NotifyCommand<object>(OnMoveWorksToAnotherConatruction);
            MoveWorksToAnotherConatructionCommand.Name = "Переместить в..";
            WorksContextMenuCommands.Add(AddCreatedFromTemplateWorkCommand);
            WorksContextMenuCommands.Add(MoveWorksToAnotherConatructionCommand);
            WorksContextMenuCommands.Add(DeleteWorkCommand);

            #endregion
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            
            #region Ribbon Init

            #endregion
        }

        private void OnDataGridLostFocus(object obj)
        {
            SelectedWorks.Clear();
        }

        private void OnDataGridSelectionChanged(object works)
        {
            SelectedWorks.Clear();
            foreach (bldWork work in (IList)works)
               SelectedWorks.Add(work);
        }

        private void OnDeleteWork()
        {
            ObservableCollection<bldWork> works_for_delete = new ObservableCollection<bldWork>(SelectedWorks);
            UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
            localUnDoReDo.Register(SelectedConstruction);
            foreach (bldWork work in works_for_delete)
                SelectedConstruction.RemoveWork(work);
            UnDoReDo.AddUnDoReDo(localUnDoReDo);

        }

        private void OnMoveWorksToAnotherConatruction(object obj)
        {
            
        }

        private void OnAddCreatedFromTemplateWork(object work)
        {
            bldWork selected_work = ((IList)work)[0] as bldWork;
            bldWork new_work = selected_work.Clone() as bldWork;
            UnDoReDo.Register(new_work);
            SelectedConstruction.AddWork(new_work);
        }

        private void OnAddNewWork()
        {
            bldWork new_work = new bldWork();
            UnDoReDo.Register(new_work);
            SelectedConstruction.AddWork(new_work);
        }

        private void OnRemoveExecutiveScheme(object obj)
        {
            bldExecutiveScheme scheme_document = ((Tuple<object, object>)obj).Item1 as bldExecutiveScheme;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (scheme_document == null || selected_work == null) return;

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldExecutiveSchemesGroup, bldExecutiveScheme>
                 (scheme_document, "Документ",
                result => {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveExecutiveScheme(scheme_document);
                    }
                }, _dialogService, Id);
        }

        private void OnAddExecutiveSchemes(object obj)
        {
            if (obj == null) return;
            // bldExecutiveScheme removed_document = ((Tuple<object, object>)obj).Item1 as bldExecutiveScheme;
            bldWork scheme_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            ObservableCollection<bldExecutiveScheme> All_ExecutiveSchemes = new ObservableCollection<bldExecutiveScheme>(
                 _buildingUnitsRepository.ExecutiveSchemes.GetAllAsync().Where(ech => !scheme_work.ExecutiveSchemes.Contains(ech)).ToList());

            NameablePredicate<ObservableCollection<bldExecutiveScheme>, bldExecutiveScheme> predicate_1 = new NameablePredicate<ObservableCollection<bldExecutiveScheme>, bldExecutiveScheme>();
            predicate_1.Name = "Показать все схемы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<ObservableCollection<bldExecutiveScheme>, bldExecutiveScheme> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldExecutiveScheme>, bldExecutiveScheme>();
            nameablePredicatesCollection.Add(predicate_1);
            ObservableCollection<bldExecutiveScheme> schemes_for_add_collection = new ObservableCollection<bldExecutiveScheme>();

            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldExecutiveScheme>, bldExecutiveScheme>
                (schemes_for_add_collection, All_ExecutiveSchemes,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       localUnDoReDo.Register(scheme_work);
                       foreach (bldExecutiveScheme scheme in schemes_for_add_collection)
                           scheme_work.AddExecutiveScheme(scheme);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddExecutiveSchemeToCollectionFromListDialogView).Name,
               "Добавить испольнительную схему",
               "Форма испольнительных схем.",
               "Список исполнительных схем", ""); ;
        }

        private void OnRemoveLaboratoryReport(object obj)
        {
            bldLaboratoryReport reportd_document = ((Tuple<object, object>)obj).Item1 as bldLaboratoryReport;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (reportd_document == null || selected_work == null) return;

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldLaboratoryReportsGroup, bldLaboratoryReport>
                 (reportd_document, "Документ",
                result => {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveLaboratoryReport(reportd_document);
                    }
                }, _dialogService, Id);
        }

        private void OnAddLaboratoryReports(object obj)
        {
            if (obj == null) return;
            // bldLaboratoryReport removed_document = ((Tuple<object, object>)obj).Item1 as bldLaboratoryReport;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            ObservableCollection<bldLaboratoryReport> All_LaboratoryRecords = new ObservableCollection<bldLaboratoryReport>(
                 _buildingUnitsRepository.LaboratoryReports.GetAllAsync().Where(lr => !selected_work.LaboratoryReports.Contains(lr)).ToList());

            NameablePredicate<ObservableCollection<bldLaboratoryReport>, bldLaboratoryReport> predicate_1 = new NameablePredicate<ObservableCollection<bldLaboratoryReport>, bldLaboratoryReport>();
            predicate_1.Name = "Показать все документы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<ObservableCollection<bldLaboratoryReport>, bldLaboratoryReport> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldLaboratoryReport>, bldLaboratoryReport>();
            nameablePredicatesCollection.Add(predicate_1);
            ObservableCollection<bldLaboratoryReport> reports_for_add_collection = new ObservableCollection<bldLaboratoryReport>();

            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldLaboratoryReport>, bldLaboratoryReport>
                (reports_for_add_collection, All_LaboratoryRecords,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       localUnDoReDo.Register(selected_work);
                       foreach (bldLaboratoryReport report in reports_for_add_collection)
                           selected_work.AddLaboratoryReport(report);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddLaboratoryReportToCollectionFromListDialogView).Name,
               "Добавить документацию",
               "Форма добавления документации.",
               "Список документов", "");;
        }

        private void OnRemoveProjectDocument(object obj)
        {
            bldProjectDocument removed_document = ((Tuple<object, object>)obj).Item1 as bldProjectDocument;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (removed_document == null || selected_work == null) return;
          
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldProjectDocumentsGroup, bldProjectDocument>
                 (removed_document, "Документ",
                result => {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveProjectDocument(removed_document);
                    }
                }, _dialogService, Id);
        }

        private void OnAddProjectsDocuments(object obj)
        {
            if (obj == null) return;
           // bldProjectDocument removed_document = ((Tuple<object, object>)obj).Item1 as bldProjectDocument;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
           ObservableCollection<bldProjectDocument> All_ProjectDocuments = new ObservableCollection<bldProjectDocument>(
                _buildingUnitsRepository.ProjectDocuments.GetAllAsync().Where(pd => !selected_work.ProjectDocuments.Contains(pd)).ToList());

            NameablePredicate<ObservableCollection<bldProjectDocument>, bldProjectDocument> predicate_1 = new NameablePredicate<ObservableCollection<bldProjectDocument>, bldProjectDocument>();
            predicate_1.Name = "Показать все документы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<ObservableCollection<bldProjectDocument>, bldProjectDocument> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldProjectDocument>, bldProjectDocument>();
            nameablePredicatesCollection.Add(predicate_1);
            ObservableCollection<bldProjectDocument> documents_for_add_collection = new ObservableCollection<bldProjectDocument>();

            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldProjectDocument>, bldProjectDocument>
                (documents_for_add_collection, All_ProjectDocuments,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       localUnDoReDo.Register(selected_work);
                       foreach (bldProjectDocument document in documents_for_add_collection)
                           selected_work.AddProjectDocument(document);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddProjectDocumentToCollectionFromListDialogView).Name,
               "Добавить документацию",
               "Форма добавления документации.",
               "Список документов", "");
        }

        private void OnRemoveMaterial(object obj)
        {
            bldMaterial removed_material = ((Tuple<object, object>)obj).Item1 as bldMaterial;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (removed_material == null || selected_work == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldMaterialsGroup, bldMaterial>
                 ( removed_material, "Материал",
                result => {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveMaterial(removed_material);
                    }
                }, _dialogService, Id);
        }

        private void OnAddMaterials(object obj)
        {
            if (obj == null) return;
          //  bldMaterial removed_material = ((Tuple<object, object>)obj).Item1 as bldMaterial;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;


            ObservableCollection<bldMaterial> All_Materials = new ObservableCollection<bldMaterial>(
                _buildingUnitsRepository.Materials.GetAllAsync().Where(mt=>!selected_work.Materials.Contains(mt)).ToList() );

             NameablePredicate<ObservableCollection<bldMaterial>, bldMaterial> predicate_1 = new NameablePredicate<ObservableCollection<bldMaterial>, bldMaterial>();
            predicate_1.Name = "Показать все материалы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<ObservableCollection<bldMaterial>, bldMaterial> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldMaterial>, bldMaterial>();
            nameablePredicatesCollection.Add(predicate_1);
            ObservableCollection<bldMaterial> materials_for_add_collection = new ObservableCollection<bldMaterial>();
         
            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldMaterial>, bldMaterial>
                (materials_for_add_collection, All_Materials,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       localUnDoReDo.Register(selected_work);
                       foreach (bldMaterial bld_material in materials_for_add_collection)
                           selected_work.AddMaterial(bld_material);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddMaterialToCollectionFromListDialogView).Name,
               "Добавить материалы",
               "Форма добавления материалов.",
               "Список материалов", "");
        }

        private void OnUnitOfMeasurement(object obj)
        {
            bldWork selected_work = obj as bldWork;
            UnDoReDo.Register(selected_work);
            selected_work.UnitOfMeasurement = null;

        }

        private void OnSelectUnitOfMeasurement(object obj)
        {
            bldWork selected_work = obj as bldWork;
            ObservableCollection<bldUnitOfMeasurement> All_UnitOfMeasurements =
                 new ObservableCollection<bldUnitOfMeasurement>(_buildingUnitsRepository.UnitOfMeasurementRepository.GetAllUnits());
            NameablePredicate<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement> predicate_1 = new NameablePredicate<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>();
            predicate_1.Name = "Все единицы измеререния";
            predicate_1.Predicate = (col) => col;
            NameablePredicateObservableCollection<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement> predicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>();
            predicatesCollection.Add(predicate_1);
            ObservableCollection<bldUnitOfMeasurement> units_for_add_collection= new ObservableCollection<bldUnitOfMeasurement>();
            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldUnitOfMeasurement>, bldUnitOfMeasurement>
             (units_for_add_collection, All_UnitOfMeasurements,
              predicatesCollection,
             _dialogService,
              (result) =>
              {
                  if (result.Result == ButtonResult.Yes)
                  {
                      foreach (bldUnitOfMeasurement unit_of_measurement in units_for_add_collection)
                      {
                          UnDoReDo.Register(selected_work);
                          selected_work.UnitOfMeasurement = unit_of_measurement;
                          break;
                      }
                      SaveCommand.RaiseCanExecuteChanged();
                  }
                  if (result.Result == ButtonResult.No)
                  {
                  }
              },
             typeof(AddUnitOfMeasurementToCollectionFromListDialogView).Name,
              "Выбрать единицу измерения",
              "Форма для выбора единицы измерения.",
              "Список един измерения", "");
        }

        private void OnAddNextWork(object works)
        {
            if (works == null) return;
            //     bldWork removed_work = ((Tuple<object, object>)works).Item1 as bldWork; //Не используется 
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;


            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync().Where(wr => wr.Id != selected_work.Id &&
                                             !selected_work.PreviousWorks.Contains(wr) && !selected_work.NextWorks.Contains(wr)).ToList());

            ObservableCollection<bldWork> works_for_add_collection = new ObservableCollection<bldWork>();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_1.Name = "Показать только из текущей конструкции.";
            predicate_1.Predicate = cl => cl.Where(el => el?.bldConstruction != null &&
                                                        el?.bldConstruction.Id == selected_work?.bldConstruction?.Id).ToList();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_2 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_2.Name = "Показать на одну ступень выше, но без работ текущей кострукции";
            predicate_2.Predicate = cl => cl.Where(el => el?.bldConstruction?.Id != selected_work?.bldConstruction?.Id &&
                                                        (el.bldConstruction?.ParentConstruction?.Id == selected_work.bldConstruction?.ParentConstruction?.Id ||
                                                          el.bldConstruction?.bldObject?.Id == selected_work.bldConstruction?.bldObject?.Id)).ToList();
            NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldWork>, bldWork>
              (works_for_add_collection, All_Works,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                        localUnDoReDo.Register(selected_work);
                       foreach (bldWork bld_work in works_for_add_collection)
                           selected_work.AddNextWork(bld_work);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddWorksToCollectionFromListDialogView).Name,
               "Добавить работы как послудующие",
               "Форма добавления послудующих работ.",
               "Список работ", "");

        }
        private void OnRemoveNextWork(object works)
        {
            bldWork removed_work = ((Tuple<object,object>)works).Item1 as bldWork;
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;
            if (removed_work == null || selected_work == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (removed_work, "Последующая работа",
                result => {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveNextWork(removed_work);
                    }
                }, _dialogService, Id);
        }
    
        private void OnDataGridLostSocus(object obj)
        {

            //if (obj == SelectedPreviousWork)
            //{
            //    SelectedNextWork = null;
            //    return;
            //}
            //if (obj == SelectedNextWork)
            //{

            //    SelectedPreviousWork = null;
            //    return;
            //}
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (SelectedConstruction != null)
                return !SelectedConstruction.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public virtual void OnSave()
        {
            base.OnSave<bldConstruction>(SelectedConstruction);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldConstruction>(obj, SelectedConstruction);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
            _applicationCommands.CreateNewWorkCommand.UnregisterCommand(AddNewWorkCommand);
            _applicationCommands.DeleteWorksCommand.UnregisterCommand(AddNewWorkCommand);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
             ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_works_group"];
           // ConveyanceObject navigane_message_construction = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message_works != null)
            {
                SelectedWorksGroup = (bldWorksGroup)navigane_message_works.Object;
                EditMode = navigane_message_works.EditMode;
                if (SelectedConstruction != null) SelectedConstruction.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedConstruction = (bldConstruction) SelectedWorksGroup.ParentObject;
                SelectedConstruction.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo.Register(SelectedConstruction);
                foreach (bldWork work in SelectedConstruction.Works)
                    UnDoReDo.Register(work);
            //    SelectedConstruction.Works[0].UnitOfMeasurement = new bldUnitOfMeasurement("кг");
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_works_group"];
            if (((bldWorksGroup)navigane_message.Object).Id != SelectedWorksGroup.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

    }
}
