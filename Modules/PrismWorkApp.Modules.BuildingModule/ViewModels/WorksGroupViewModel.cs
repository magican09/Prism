using Prism;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class WorksGroupViewModel : BaseViewModel<bldWorksGroup>, INotifyPropertyChanged, INavigationAware, IActiveAware
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


        //private bldMaterial _selectedMaterial;
        //public bldMaterial SelectedMaterial
        //{
        //    get { return _selectedMaterial; }
        //    set { SetProperty(ref _selectedMaterial, value); }
        //}
        private bldWorksGroup _selectedWorksGroup;
        public bldWorksGroup SelectedWorksGroup
        {
            get { return _selectedWorksGroup; }
            set { SetProperty(ref _selectedWorksGroup, value); }
        }

        private ObservableCollection<bldWork> _selectedWorks = new ObservableCollection<bldWork>();
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
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyCommand<object> RemovePreviousWorkCommand { get; private set; }
        public NotifyCommand<object> RemoveNextWorkCommand { get; private set; }

        public ObservableCollection<INotifyCommand> NextWorksContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddNextWorkCommand { get; private set; }

        public NotifyCommand<object> CopyCommand { get; private set; }
        public NotifyCommand<object> CutCommand { get; private set; }
        public NotifyCommand<object> PasteCommand { get; private set; }
        private ObservableCollection<CopiedCutedObject> _objectsBuffer = new ObservableCollection<CopiedCutedObject>();


        public ObservableCollection<INotifyCommand> UnitsOfMeasurementContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> SelectUnitOfMeasurementCommand { get; private set; }
        public NotifyCommand<object> RemoveUnitOfMeasurementCommand { get; private set; }

        public ObservableCollection<INotifyCommand> MaterialsContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddMaterialsCommand { get; private set; }
        public NotifyCommand<object> RemoveMaterialCommand { get; private set; }
        public NotifyCommand<object> AddCreatedFromTemplateMaterialCommand { get; private set; }
        public NotifyCommand<object> CopyMaterialsCommand { get; private set; }
        public NotifyCommand<object> CutMaterialsCommand { get; private set; }
        public NotifyCommand<object> PasteMaterialsCommand { get; private set; }
        public CopyCutPasteCommands<BindableBase> CopyCutPasteCommand { get; private set; }

        public ObservableCollection<INotifyCommand> ProjectDocumentationContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddProjectsDocumentCommand { get; private set; }
        public NotifyCommand<object> RemoveProjectDocumentCommand { get; private set; }

        public ObservableCollection<INotifyCommand> LaboratoryReportsContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddLaboratoryReportsCommand { get; private set; }
        public NotifyCommand<object> RemoveLaboratoryReportCommand { get; private set; }

        public ObservableCollection<INotifyCommand> ExecutiveSchemesContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand<object> AddExecutiveSchemesCommand { get; private set; }
        public NotifyCommand<object> RemoveExecutiveSchemeCommand { get; private set; }

        public ObservableCollection<INotifyCommand> WorksContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand CreateNewWorkCommand { get; private set; }
        public NotifyCommand<object> AddCreatedFromTemplateWorkCommand { get; private set; }
        public NotifyCommand MoveWorksToAnotherConatructionCommand { get; private set; }
        public NotifyCommand AddWorksFromAnotherConatructionCommand { get; private set; }
        public NotifyCommand DeleteWorkCommand { get; private set; }
        public NotifyCommand SaveWorksExecutiveDocumentationCommand { get; private set; }


        public NotifyCommand RemoveWorkCommand { get; private set; }
        // public NotifyCommand RemoveWorkCommand { get; private set; }



        public IBuildingUnitsRepository _buildingUnitsRepository { get; }

        //ObservableCollection<GanttChartItem> items = new ObservableCollection<GanttChartItem>
        //{
        //    new GanttChartItem { Content = "Task 1" },
        //    new GanttChartItem { Content = "Task 1.1", Indentation = 1, Start = new DateTime(), Finish = new DateTime()},
        //    new GanttChartItem { Content = "Task 1.2", Indentation = 1, Start = new DateTime(), Finish = new DateTime()},
        //    new GanttChartItem { Content = "Task 2" },
        //    new GanttChartItem { Content = "Task 2.1", Indentation = 1, Start = new DateTime(), Finish = new DateTime()},
        //    new GanttChartItem { Content = "Task 2.2", Indentation = 1,  },
        //    new GanttChartItem { Content = "Task 2.2.1", Indentation = 2, Start = new DateTime(), Finish = new DateTime() },
        //    new GanttChartItem { Content = "Task 2.2.2", Indentation = 2, Start = new DateTime(), IsMilestone = true,  }

        //};
        public WorksGroupViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {
            UnDoReDo = new UnDoReDoSystem();
            // DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            ApplicationCommands = applicationCommands;


            #region Commands Init
            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedConstruction);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            RemoveNextWorkCommand = new NotifyCommand<object>(OnRemoveNextWork);
            RemoveNextWorkCommand.Name = "Удалить";
            AddNextWorkCommand = new NotifyCommand<object>(OnAddNextWork);
            AddNextWorkCommand.Name = "Добавить";
            NextWorksContextMenuCommands.Add(AddNextWorkCommand);
            NextWorksContextMenuCommands.Add(RemoveNextWorkCommand);

            CopyCommand = new NotifyCommand<object>(OnCopy);
            CopyCommand.Name = "Копировать";
            CutCommand = new NotifyCommand<object>(OnCut);
            CutCommand.Name = "Вырезать";
            PasteCommand = new NotifyCommand<object>(OnPaste, (ob) => _objectsBuffer.Count > 0).ObservesPropertyChangedEvent(_objectsBuffer);
            PasteCommand.Name = "Вставить";


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
            AddCreatedFromTemplateMaterialCommand = new NotifyCommand<object>(OnAddCreatedFromTemplateMaterial);
            AddCreatedFromTemplateMaterialCommand.Name = "Создать на основании материал";
            MaterialsContextMenuCommands.Add(AddMaterialsCommand);
            MaterialsContextMenuCommands.Add(RemoveMaterialCommand);
            MaterialsContextMenuCommands.Add(AddCreatedFromTemplateMaterialCommand);
            MaterialsContextMenuCommands.Add(CopyCommand);
            MaterialsContextMenuCommands.Add(CutCommand);
            MaterialsContextMenuCommands.Add(PasteCommand);


            AddProjectsDocumentCommand = new NotifyCommand<object>(OnAddProjectsDocuments);
            AddProjectsDocumentCommand.Name = "Добавить документацию";
            RemoveProjectDocumentCommand = new NotifyCommand<object>(OnRemoveProjectDocument);
            RemoveProjectDocumentCommand.Name = "Удалить документацию";
            ProjectDocumentationContextMenuCommands.Add(AddProjectsDocumentCommand);
            ProjectDocumentationContextMenuCommands.Add(RemoveProjectDocumentCommand);

            AddLaboratoryReportsCommand = new NotifyCommand<object>(OnAddLaboratoryReports);
            AddLaboratoryReportsCommand.Name = "Добавить документацию";
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
            ExecutiveSchemesContextMenuCommands.Add(CopyCommand);
            ExecutiveSchemesContextMenuCommands.Add(CutCommand);
            ExecutiveSchemesContextMenuCommands.Add(PasteCommand);

            #region Work Context Menu
            CreateNewWorkCommand = new NotifyCommand(OnAddNewWork);
            CreateNewWorkCommand.Name = "Создать новую работу";
            AddCreatedFromTemplateWorkCommand = new NotifyCommand<object>(OnAddCreatedFromTemplateWork, (ob) => { return SelectedWorks.Count == 1; }).ObservesPropertyChangedEvent(SelectedWorks);
            AddCreatedFromTemplateWorkCommand.Name = "Создать на основании";
            DeleteWorkCommand = new NotifyCommand(OnDeleteWork, () => SelectedWorks.Count > 0).ObservesPropertyChangedEvent(SelectedWorks);
            DeleteWorkCommand.Name = "Удалить";

            MoveWorksToAnotherConatructionCommand = new NotifyCommand(OnMoveWorksToAnotherConatruction, () => SelectedWorks.Count > 0).ObservesPropertyChangedEvent(SelectedWorks);
            MoveWorksToAnotherConatructionCommand.Name = "Переместить в другую консрукцию";
            AddWorksFromAnotherConatructionCommand = new NotifyCommand(OnAddWorksFromAnotherConatruction);
            AddWorksFromAnotherConatructionCommand.Name = "Добавить из другой консрукции";
            SaveWorksExecutiveDocumentationCommand = new NotifyCommand(OnSaveWorksExecutionDocumentation, () => { return SelectedWorks.Count > 0; }).ObservesPropertyChangedEvent(SelectedWorks);
            SaveWorksExecutiveDocumentationCommand.Name = "Выгрузить ИД";

            WorksContextMenuCommands.Add(AddCreatedFromTemplateWorkCommand);
            WorksContextMenuCommands.Add(SaveWorksExecutiveDocumentationCommand);
            WorksContextMenuCommands.Add(MoveWorksToAnotherConatructionCommand);
            WorksContextMenuCommands.Add(AddWorksFromAnotherConatructionCommand);
            WorksContextMenuCommands.Add(DeleteWorkCommand);
            #endregion

            #endregion

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;

            #region Ribbon Init

            #endregion

            ApplicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
            ApplicationCommands.MoveWorkCommand.RegisterCommand(MoveWorksToAnotherConatructionCommand);
            ApplicationCommands.SaveExecutiveDocumentsCommand.RegisterCommand(SaveWorksExecutiveDocumentationCommand);
            ApplicationCommands.AddWorkCommand.RegisterCommand(AddWorksFromAnotherConatructionCommand);
            ApplicationCommands.DeleteWorkCommand.RegisterCommand(DeleteWorkCommand);
            ApplicationCommands.CreateWorkFromTemplateCommand.RegisterCommand(AddCreatedFromTemplateWorkCommand);
            ApplicationCommands.CreateWorkCommand.RegisterCommand(CreateNewWorkCommand);
        }

        private void OnCopy(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            _objectsBuffer.Clear();
            foreach (object tpl_obj in ((Tuple<object, object>)obj).Item1 as IList)
                _objectsBuffer.Add(new CopiedCutedObject(selected_work, tpl_obj, CopyCutPaste.COPIED));
        }
        private void OnCut(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            _objectsBuffer.Clear();
            foreach (object tpl_obj in ((Tuple<object, object>)obj).Item1 as IList)
                _objectsBuffer.Add(new CopiedCutedObject(selected_work, tpl_obj, CopyCutPaste.CUTED));
        }
        private void OnPaste(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
            UnDoReDo.SetChildrenUnDoReDoSystem(localUnDoReDo);
            localUnDoReDo.Register(selected_work);
            foreach (CopiedCutedObject copy_cut_obj in _objectsBuffer)
            {
                switch (copy_cut_obj.Element?.GetType()?.Name)
                {
                    case (nameof(bldMaterial)):
                        if (copy_cut_obj.ActionType == CopyCutPaste.COPIED)
                        {
                            bldMaterial new_obj = (copy_cut_obj.Element as bldMaterial).Clone() as bldMaterial;
                            selected_work.AddMaterial(new_obj);
                        }
                        if (copy_cut_obj.ActionType == CopyCutPaste.CUTED)
                        {
                            bldWork from_work = copy_cut_obj.FromObject as bldWork;
                            localUnDoReDo.Register(from_work);
                            (copy_cut_obj.FromObject as bldWork).RemoveMaterial(copy_cut_obj.Element as bldMaterial);
                            selected_work.AddMaterial(copy_cut_obj.Element as bldMaterial);
                        }
                        break;
                    case (nameof(bldExecutiveScheme)):
                        if (copy_cut_obj.ActionType == CopyCutPaste.COPIED)
                        {
                            bldExecutiveScheme new_obj = (copy_cut_obj.Element as bldExecutiveScheme).Clone() as bldExecutiveScheme;
                            selected_work.AddExecutiveScheme(new_obj);
                        }
                        if (copy_cut_obj.ActionType == CopyCutPaste.CUTED)
                        {
                            bldWork from_work = copy_cut_obj.FromObject as bldWork;
                            localUnDoReDo.Register(from_work);
                            (copy_cut_obj.FromObject as bldWork).RemoveExecutiveScheme(copy_cut_obj.Element as bldExecutiveScheme);
                            selected_work.AddExecutiveScheme(copy_cut_obj.Element as bldExecutiveScheme);
                        }
                        break;
                }

            }



            _objectsBuffer.Clear();
            UnDoReDo.AddUnDoReDo(localUnDoReDo);
            UnDoReDo.UnSetChildrenUnDoReDoSystem(localUnDoReDo);
            SaveCommand.RaiseCanExecuteChanged();
        }





        private void OnCopyMaterials(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            _objectsBuffer.Clear();
            foreach (object tpl_obj in ((Tuple<object, object>)obj).Item1 as IList)
                if (tpl_obj is bldMaterial material) _objectsBuffer.Add(new CopiedCutedObject(selected_work, material, CopyCutPaste.COPIED));

        }
        private void OnCutMaterials(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            _objectsBuffer.Clear();
            foreach (object tpl_obj in ((Tuple<object, object>)obj).Item1 as IList)
                if (tpl_obj is bldMaterial material) _objectsBuffer.Add(new CopiedCutedObject(selected_work, material, CopyCutPaste.CUTED));
        }


        private void OnPasteMaterials(object obj)
        {
            if (obj == null) return;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
            UnDoReDo.SetChildrenUnDoReDoSystem(localUnDoReDo);
            localUnDoReDo.Register(selected_work);
            if (_objectsBuffer.Count == 0)
                ;
            foreach (CopiedCutedObject copy_cut_obj in _objectsBuffer)
            {
                if (copy_cut_obj.ActionType == CopyCutPaste.COPIED)
                {
                    bldMaterial new_material = (copy_cut_obj.Element as bldMaterial).Clone() as bldMaterial;
                    selected_work.AddMaterial(new_material);
                }
                if (copy_cut_obj.ActionType == CopyCutPaste.CUTED)
                {
                    bldWork from_work = copy_cut_obj.FromObject as bldWork;
                    localUnDoReDo.Register(from_work);
                    (copy_cut_obj.FromObject as bldWork).RemoveMaterial(copy_cut_obj.Element as bldMaterial);
                    selected_work.AddMaterial(copy_cut_obj.Element as bldMaterial);
                }

            }
            _objectsBuffer.Clear();
            UnDoReDo.AddUnDoReDo(localUnDoReDo);
            UnDoReDo.UnSetChildrenUnDoReDoSystem(localUnDoReDo);
            SaveCommand.RaiseCanExecuteChanged();

        }



        private void OnAddWorksFromAnotherConatruction()
        {
            bldConstructionsGroup All_Consructions = new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetAllAsync().Where(cn => cn.Id != SelectedConstruction.Id).ToList());

            bldConstructionsGroup constructions_for_add_collection = new bldConstructionsGroup();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_1 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_1.Name = "Показать только из текущего уровня.";
            predicate_1.Predicate = cl => cl.Where(el => (el?.bldObject != null || el.ParentConstruction?.Id != null) &&
                        (el?.bldObject.Id == SelectedConstruction?.bldObject?.Id || el.ParentConstruction?.Id != SelectedConstruction?.ParentConstruction?.Id)).ToList();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_2 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_2.Name = "Показать все";
            predicate_2.Predicate = cl => cl;
            NameablePredicateObservableCollection<bldConstructionsGroup, bldConstruction> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldConstructionsGroup, bldConstruction>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            CoreFunctions.AddElementsToCollectionWhithDialogList<bldConstructionsGroup, bldConstruction>
        (constructions_for_add_collection, All_Consructions,
         nameablePredicatesCollection,
        _dialogService,
         (result) =>
         {
             if (result.Result == ButtonResult.Yes)
             {
                 UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                 bldConstruction selected_from_construction = constructions_for_add_collection[0];
                 localUnDoReDo.Register(SelectedConstruction);

                 bldWorksGroup All_Works = selected_from_construction.Works;
                 ObservableCollection<bldWork> works_for_add_collection = new ObservableCollection<bldWork>();

                 NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
                 predicate_1_1.Predicate = cl => cl;
                 NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork>();
                 nameablePredicatesCollection.Add(predicate_1_1);
                 CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldWork>, bldWork>
             (works_for_add_collection, All_Works,
              nameablePredicatesCollection,
             _dialogService,
              (result) =>
              {
                  if (result.Result == ButtonResult.Yes)
                  {

                      foreach (bldWork bld_work in works_for_add_collection)
                          SelectedConstruction.AddWork(bld_work);

                      UnDoReDo.AddUnDoReDo(localUnDoReDo);
                      SaveCommand.RaiseCanExecuteChanged();
                  }
                  if (result.Result == ButtonResult.No)
                  {

                  }
              },
             typeof(AddWorksToCollectionFromListDialogView).Name,
              "Добавить работы как послудующие",
              "Форма добавления послудующих работ.",
              "Список работ", "", "Добавить");


             }
             if (result.Result == ButtonResult.No)
             {

             }
         },
        typeof(AddbldConstructionToCollectionFromListDialogView).Name,
         "Выбрать конструкцию",
         "Форма выбора конструкции",
         "Список кострукций", "", "Выбрать");

        }

        private void OnSaveWorksExecutionDocumentation()
        {
            ObservableCollection<bldWork> works_for_doc_save = new ObservableCollection<bldWork>(SelectedWorks);
            string folder_path = Functions.GetFolderPath();
            foreach (bldWork work in works_for_doc_save)
                work.SaveAOSRsToWord(folder_path);
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
            UnDoReDo.UnRegister(SelectedConstruction);
            UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
            localUnDoReDo.Register(SelectedConstruction);
            foreach (bldWork work in works_for_delete)
                if (SelectedConstruction.Works.Contains(work))
                    SelectedConstruction.RemoveWork(work);
            UnDoReDo.AddUnDoReDo(localUnDoReDo);
            UnDoReDo.Register(SelectedConstruction);

        }
        private void OnMoveWorksToAnotherConatruction()
        {
            DialogParameters param = new DialogParameters();
            param.Add("common_collection", new ObservableCollection<IEntityObject>(SelectedConstruction.bldProject.BuildingObjects));
            ObservableCollection<IEntityObject> current_collection = new ObservableCollection<IEntityObject>();
            current_collection.Add(SelectedConstruction);
            param.Add("current_element_collection", current_collection);
            param.Add("element_type", typeof(bldConstruction));

            _dialogService.ShowDialog(typeof(SelectConstructionFromTreeViewDialog).Name, param,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        bldConstruction construction_for_work_adding = result.Parameters.GetValue<bldConstruction>("selected_element");
                        ObservableCollection<bldWork> works_for_move = new ObservableCollection<bldWork>(SelectedWorks);
                        UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                        localUnDoReDo.Register(construction_for_work_adding);
                        construction_for_work_adding.AddWorkGroup(works_for_move);
                        SaveCommand.RaiseCanExecuteChanged();
                        UnDoReDo.AddUnDoReDo(localUnDoReDo);
                    }
                });
        }
        private void OnMoveWorksToAnotherConatruction12112()
        {
            // if ((selected_entities as IList) is IList<bldWork> selected_works)
            {
                bldWorksGroup selected_works = new bldWorksGroup(SelectedWorks);
                bldConstructionsGroup All_Consructions = new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetAllAsync().Where(cn => cn.Id != SelectedConstruction.Id).ToList());

                ObservableCollection<bldConstruction> constructions_for_add_collection = new ObservableCollection<bldConstruction>();
                NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction> predicate_1 = new NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction>();
                predicate_1.Name = "Показать только из текущего уровня.";
                predicate_1.Predicate = cl => cl.Where(el => (el?.bldObject != null || el.ParentConstruction?.Id != null) &&
                            (el?.bldObject.Id == SelectedConstruction?.bldObject?.Id || el.ParentConstruction?.Id != SelectedConstruction?.ParentConstruction?.Id)).ToList();
                NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction> predicate_2 = new NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction>();
                predicate_2.Name = "Показать все";
                predicate_2.Predicate = cl => cl;
                NameablePredicateObservableCollection<ObservableCollection<bldConstruction>, bldConstruction> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldConstruction>, bldConstruction>();
                nameablePredicatesCollection.Add(predicate_1);
                nameablePredicatesCollection.Add(predicate_2);
                CoreFunctions.AddElementsToCollectionWhithDialogList<ObservableCollection<bldConstruction>, bldConstruction>
            (constructions_for_add_collection, All_Consructions,
             nameablePredicatesCollection,
            _dialogService,
             (result) =>
             {
                 if (result.Result == ButtonResult.Yes)
                 {
                     UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                     foreach (bldConstruction bld_construction in constructions_for_add_collection)
                     {
                         // bldWorksGroup works_for_removed = new bldWorksGroup(selected_works);
                         foreach (bldWork bld_work in selected_works)
                         {
                             localUnDoReDo.Register(bld_construction);
                             bld_construction.AddWork(bld_work);
                         }
                         break;
                     }
                     SaveCommand.RaiseCanExecuteChanged();
                     UnDoReDo.AddUnDoReDo(localUnDoReDo);
                 }
                 if (result.Result == ButtonResult.No)
                 {
                 }
             },
            typeof(AddbldConstructionToCollectionFromListDialogView).Name,
             "Выбрать конструкцию",
            "Форма выбора конструкции",
              "Список кострукций", "");
            }

        }

        private void OnAddCreatedFromTemplateWork(object work)
        {
            bldWork selected_work = SelectedWork;
            bldWork new_work = selected_work.Clone() as bldWork;
            new_work.AOSRDocument = new bldAOSRDocument();
            new_work.AOSRDocument.RegId = selected_work.Code;
            new_work.AOSRDocument.Date = (DateTime)selected_work.EndTime;
            UnDoReDo.Register(new_work);
            UnDoReDo.Register(new_work.AOSRDocument);
            SelectedConstruction.AddWork(new_work);

        }

        private void OnAddNewWork()
        {
            bldWork new_work = new bldWork();
            new_work.UnitOfMeasurement = new bldUnitOfMeasurement();
            new_work.WorkArea = new bldWorkArea();
            new_work.AOSRDocument = new bldAOSRDocument();
            //  UnDoReDo.Register(new_work);
            SelectedConstruction.AddWork(new_work);
        }

        private void OnRemoveExecutiveScheme(object obj)
        {
            bldExecutiveScheme scheme_document = ((Tuple<object, object>)obj).Item1 as bldExecutiveScheme;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (scheme_document == null || selected_work == null) return;

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldExecutiveSchemesGroup, bldExecutiveScheme>
                 (scheme_document, "Документ",
                result =>
                {
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
            bldExecutiveSchemesGroup All_ExecutiveSchemes = new bldExecutiveSchemesGroup(
                 _buildingUnitsRepository.ExecutiveSchemes.GetAllAsync().Where(ech => !scheme_work.ExecutiveSchemes.Contains(ech)).ToList());

            NameablePredicate<bldExecutiveSchemesGroup, bldExecutiveScheme> predicate_1 = new NameablePredicate<bldExecutiveSchemesGroup, bldExecutiveScheme>();
            predicate_1.Name = "Показать все схемы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<bldExecutiveSchemesGroup, bldExecutiveScheme> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldExecutiveSchemesGroup, bldExecutiveScheme>();
            nameablePredicatesCollection.Add(predicate_1);
            bldExecutiveSchemesGroup schemes_for_add_collection = new bldExecutiveSchemesGroup();

            CoreFunctions.AddElementsToCollectionWhithDialogList<bldExecutiveSchemesGroup, bldExecutiveScheme>
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
                result =>
                {
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
            bldLaboratoryReportsGroup All_LaboratoryRecords = new bldLaboratoryReportsGroup(
                 _buildingUnitsRepository.LaboratoryReports.GetAllAsync().Where(lr => !selected_work.LaboratoryReports.Contains(lr)).ToList());

            NameablePredicate<bldLaboratoryReportsGroup, bldLaboratoryReport> predicate_1 = new NameablePredicate<bldLaboratoryReportsGroup, bldLaboratoryReport>();
            predicate_1.Name = "Показать все документы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<bldLaboratoryReportsGroup, bldLaboratoryReport> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldLaboratoryReportsGroup, bldLaboratoryReport>();
            nameablePredicatesCollection.Add(predicate_1);
            bldLaboratoryReportsGroup reports_for_add_collection = new bldLaboratoryReportsGroup();

            CoreFunctions.AddElementsToCollectionWhithDialogList<bldLaboratoryReportsGroup, bldLaboratoryReport>
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
               "Список документов", ""); ;
        }

        private void OnRemoveProjectDocument(object obj)
        {
            bldProjectDocument removed_document = ((Tuple<object, object>)obj).Item1 as bldProjectDocument;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (removed_document == null || selected_work == null) return;

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldProjectDocumentsGroup, bldProjectDocument>
                 (removed_document, "Документ",
                result =>
                {
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
            //ObservableCollection<bldProjectDocument> All_ProjectDocuments = new ObservableCollection<bldProjectDocument>(
            //     _buildingUnitsRepository.ProjectDocuments.GetAllAsync().Where(pd => !selected_work.ProjectDocuments.Contains(pd)).ToList());
            ObservableCollection<IbldDocument> All_ProjectDocuments = new ObservableCollection<IbldDocument>();
            var bld_documentation = SelectedConstruction.Documentation.Where(dc => dc.GetType() == typeof(bldProjectDocument));
            foreach (var document in bld_documentation)
                All_ProjectDocuments.Add(document as bldProjectDocument);
            DialogParameters param = new DialogParameters();
            param.Add("common_collection", new ObservableCollection<IbldDocument>(All_ProjectDocuments));
            param.Add("current_element_collection", new ObservableCollection<IbldDocument>(SelectedWork.ProjectDocuments));
            param.Add("element_type", typeof(bldProjectDocument));
            _dialogService.ShowDialog(typeof(SelectDocumentFromTreeViewDialog).Name, param,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        bldProjectDocument documents_for_adding = result.Parameters.GetValue<bldProjectDocument>("selected_element");

                        SelectedWork.AddProjectDocument(documents_for_adding);
                        SaveCommand.RaiseCanExecuteChanged();

                    }

                });

        }
        #region Material methods
        private void OnRemoveMaterial(object obj)
        {
            bldMaterial removed_material = ((Tuple<object, object>)obj).Item1 as bldMaterial;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (removed_material == null || selected_work == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldMaterialsGroup, bldMaterial>
                 (removed_material, "Материал",
                result =>
                {
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
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            bldMaterialsGroup All_Materials = new bldMaterialsGroup("Все материалы");
            foreach (bldMaterial material in _buildingUnitsRepository.Materials.GetAllAsync().Where(mt => !selected_work.Materials.Contains(mt)).ToList())
                All_Materials.Add(material);

            NameablePredicate<bldMaterialsGroup, bldMaterial> predicate_1 = new NameablePredicate<bldMaterialsGroup, bldMaterial>();
            predicate_1.Name = "Показать все материалы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<bldMaterialsGroup, bldMaterial> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldMaterialsGroup, bldMaterial>();
            nameablePredicatesCollection.Add(predicate_1);
            bldMaterialsGroup materials_for_add_collection = new bldMaterialsGroup();

            CoreFunctions.AddElementsToCollectionWhithDialogList<bldMaterialsGroup, bldMaterial>
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
        private void OnAddCreatedFromTemplateMaterial(object obj)
        {
            bldMaterial selected_material = ((Tuple<object, object>)obj).Item1 as bldMaterial;
            bldWork selected_work = ((Tuple<object, object>)obj).Item2 as bldWork;
            if (selected_material != null)
            {
                bldMaterial new_material = selected_material.Clone() as bldMaterial;
                ConveyanceObject conveyanceObject = new ConveyanceObject(new_material, ConveyanceObjectModes.EditMode.FOR_EDIT);
                var dialog_params = new DialogParameters();
                dialog_params.Add("selected_element_conveyance_object", conveyanceObject);

                _dialogService.ShowDialog(nameof(MaterialDialogView), dialog_params, (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         _buildingUnitsRepository.Materials.Add(new_material);
                         var confirm_dialog_params = new DialogParameters();
                         confirm_dialog_params.Add("massege", "Добавить в текущую работу?");
                         confirm_dialog_params.Add("confirm_button_content", "Добавить");
                         confirm_dialog_params.Add("refuse_button_content", "Отмена");
                         confirm_dialog_params.Add("cancel_button_content", "Закрыть");

                         _dialogService.ShowDialog(nameof(ConfirmActionDialog), confirm_dialog_params, (result_dialog) =>
                         {
                             if (result_dialog.Result == ButtonResult.Yes)
                             {
                                 selected_work.AddMaterial(new_material);
                             }
                         });


                     }
                     if (result.Result == ButtonResult.No)
                     {

                     }
                 });




            }
        }
        #endregion
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
            ObservableCollection<bldUnitOfMeasurement> units_for_add_collection = new ObservableCollection<bldUnitOfMeasurement>();
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
        private void OnAddPreviousWorks(object works)
        {
            if (works == null) return;
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;


            if (selected_work == null) return;
            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync().Where(wr => wr.Id != selected_work.Id &&
                                             !selected_work.PreviousWorks.Contains(wr) && !selected_work.PreviousWorks.Contains(wr)).ToList());
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
                           selected_work.AddPreviousWork(bld_work);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddWorksToCollectionFromListDialogView).Name,
               "Добавить работы как предыдущие",
               "Форма добавления предыдущие работ.",
               "Список работ", "");

        }
        private void OnRemovePreviousWork(object works)
        {
            bldWork removed_work = ((Tuple<object, object>)works).Item1 as bldWork;
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;
            if (removed_work == null || selected_work == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (removed_work, "Предыдущая работа работа",
                result =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemovePreviousWork(removed_work);
                    }
                }, _dialogService, Id);
        }

        private void OnAddNextWork(object works)
        {
            if (works == null) return;
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
            bldWork removed_work = ((Tuple<object, object>)works).Item1 as bldWork;
            bldWork selected_work = ((Tuple<object, object>)works).Item2 as bldWork;
            if (removed_work == null || selected_work == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (removed_work, "Последующая работа",
                result =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(selected_work);
                        selected_work.RemoveNextWork(removed_work);
                    }
                }, _dialogService, Id);
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
            ApplicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            ApplicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            ApplicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
            ApplicationCommands.CreateWorkCommand.UnregisterCommand(CreateNewWorkCommand);
            ApplicationCommands.CreateWorkFromTemplateCommand.UnregisterCommand(AddCreatedFromTemplateWorkCommand);
            ApplicationCommands.AddWorkCommand.UnregisterCommand(AddWorksFromAnotherConatructionCommand);
            ApplicationCommands.DeleteWorkCommand.UnregisterCommand(DeleteWorkCommand);
            ApplicationCommands.MoveWorkCommand.UnregisterCommand(MoveWorksToAnotherConatructionCommand);
            ApplicationCommands.SaveExecutiveDocumentsCommand.UnregisterCommand(SaveWorksExecutiveDocumentationCommand);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message_works != null)
            {
                EditMode = navigane_message_works.EditMode;
                if (SelectedConstruction != null) SelectedConstruction.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedConstruction = (bldConstruction)navigane_message_works.Object;
                SelectedConstruction.ErrorsChanged += RaiseCanExecuteChanged;
                SelectedWorksGroup = SelectedConstruction.Works;
                UnDoReDo.Register(SelectedConstruction);
                foreach (bldWork work in SelectedConstruction.Works)
                {
                    UnDoReDo.Register(work);
                    if (work.AOSRDocument == null)
                        work.AOSRDocument = new bldAOSRDocument();
                    UnDoReDo.Register(work.AOSRDocument);
                }
                Title = $"{SelectedConstruction.Code} {SelectedConstruction.ShortName}";


            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (((bldConstruction)navigane_message.Object).Id != SelectedConstruction.Id)
            {

                return false;
            }
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }


    }
}
