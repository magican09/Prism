using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class WorkViewModel : BaseViewModel<bldWork>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Работа";
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
        private bldWork _resivedWork;
        public bldWork ResivedWork
        {
            get { return _resivedWork; }
            set { SetProperty(ref _resivedWork, value); }
        }
        private bldConstruction _selectedConstruction;
        public bldConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set { SetProperty(ref _selectedConstruction, value); }
        }
        private bldConstruction _resivedConstruction;
        public bldConstruction ResivedConstruction
        {
            get { return _resivedConstruction; }
            set { SetProperty(ref _resivedConstruction, value); }
        }
        private bldObject _resivedObject;
        public bldObject ResivedObject
        {
            get { return _resivedObject; }
            set { SetProperty(ref _resivedObject, value); }
        }
        private bldObject _selectedObject;
        public bldObject SelectedObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value); }
        }

        private bldWork _selectedPreviousWork;
        public bldWork SelectedPreviousWork
        {
            get { return _selectedPreviousWork; }
            set { SetProperty(ref _selectedPreviousWork, value); }
        }
        private bldWork _selectedNextWork;
        public bldWork SelectedNextWork
        {
            get { return _selectedNextWork; }
            set { SetProperty(ref _selectedNextWork, value); }
        }
        private bldMaterialsGroup _materials;
        public bldMaterialsGroup Materials
        {
            get { return _materials; }
            set { SetProperty(ref _materials, value); }
        }
        private bldMaterial _selectedMaterial;
        public bldMaterial SelectedMaterial
        {
            get { return _selectedMaterial; }
            set { SetProperty(ref _selectedMaterial, value); }
        }
        private bldLaboratoryReport _selecteLaboratoryReport;
        public bldLaboratoryReport SelecteLaboratoryReport
        {
            get { return _selecteLaboratoryReport; }
            set { SetProperty(ref _selecteLaboratoryReport, value); }
        }
        private bldExecutiveScheme _selecteExecutiveScheme;
        public bldExecutiveScheme SelecteExecutiveScheme
        {
            get { return _selecteExecutiveScheme; }
            set { SetProperty(ref _selecteExecutiveScheme, value); }
        }
        public Dictionary<Guid, object> _allDocuments = new Dictionary<Guid, object>();
        public Dictionary<Guid, object> AllDocuments
        {
            get { return _allDocuments; }
            set { SetProperty(ref _allDocuments, value); }
        }
        private object _selectedDocumentsList;
        public object SelectedDocumentsList
        {
            get { return _selectedDocumentsList; }
            set { SetProperty(ref _selectedDocumentsList, value); }
        }

        private bldResponsibleEmployee _selectedResponsibleEmployee;
        public bldResponsibleEmployee SelectedResponsibleEmployee
        {
            get { return _selectedResponsibleEmployee; }
            set { SetProperty(ref _selectedResponsibleEmployee, value); }
        }


        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyCommand RemovePreviousWorkCommand { get; private set; }
        public NotifyCommand RemoveNextWorkCommand { get; private set; }

        public NotifyCommand AddPreviousWorkCommand { get; private set; }
        public NotifyCommand AddNextWorkCommand { get; private set; }
        public NotifyCommand<object> AddCreatedFromTemplateWorkCommand { get; private set; }
        public NotifyCommand CreateNewWorkCommand { get; private set; }
     
        public NotifyCommand AddMaterialsCommand { get; private set; }
        public NotifyCommand RemoveMaterialCommand { get; private set; }

        public NotifyCommand AddLaboratoryReportsCommand { get; private set; }
        public NotifyCommand RemoveLaboratoryReportCommand { get; private set; }

        public NotifyCommand AddExecutiveSchemesCommand { get; private set; }
        public NotifyCommand RemoveExecutiveSchemeCommand { get; private set; }
        public NotifyCommand SaveAOSRsToWordCommand { get; private set; }
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }


        public WorkViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {
            UnDoReDo = new UnDoReDoSystem();
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            _applicationCommands = applicationCommands;

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                 .ObservesProperty(() => SelectedConstruction);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            RemovePreviousWorkCommand = new NotifyCommand(OnRemovePreviousWork,
                                        () => SelectedPreviousWork != null)
                    .ObservesProperty(() => SelectedPreviousWork);
            RemoveNextWorkCommand = new NotifyCommand(OnRemoveNextWork,
                                        () => SelectedNextWork != null)
                    .ObservesProperty(() => SelectedNextWork);
            AddPreviousWorkCommand = new NotifyCommand(OnAddPreviousWorks);
            AddNextWorkCommand = new NotifyCommand(OnAddNextWorks);
            
            AddMaterialsCommand = new NotifyCommand(OnAddMaterials);
            AddMaterialsCommand.Name = "Добавить материалы";
            RemoveMaterialCommand = new NotifyCommand(OnRemoveMaterial,()=> SelectedMaterial!=null).ObservesProperty(()=>SelectedMaterial);
            RemoveMaterialCommand.Name = "Удалить материал";

            AddLaboratoryReportsCommand = new NotifyCommand(OnAddLaboratoryReports);
            AddLaboratoryReportsCommand.Name = "Добавить документ";
            RemoveLaboratoryReportCommand = new NotifyCommand(OnRemoveLaboratoryReport, () => SelecteLaboratoryReport != null).ObservesProperty(() => SelecteLaboratoryReport);
            RemoveLaboratoryReportCommand.Name = "Удалить документ";
           
            AddExecutiveSchemesCommand = new NotifyCommand(OnAddExecutiveSchemes);
            AddExecutiveSchemesCommand.Name = "Добавить документ";
            RemoveExecutiveSchemeCommand = new NotifyCommand(OnRemoveExecutiveScheme, () => SelecteExecutiveScheme != null).ObservesProperty(() => SelecteExecutiveScheme);
            RemoveExecutiveSchemeCommand.Name = "Удалить документ";

            SaveAOSRsToWordCommand = new NotifyCommand(OnSaveAOSRsToWord);

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
 
        }

        private void OnRemoveExecutiveScheme()
        {
            bldExecutiveScheme scheme_report = SelecteExecutiveScheme;
            if (scheme_report == null || SelectedWork == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldExecutiveSchemesGroup, bldExecutiveScheme>
                 (scheme_report, "Документ",
                result =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        SelectedWork.AddExecutiveScheme(scheme_report);
                    }
                }, _dialogService, Id);
        }

        private void OnAddExecutiveSchemes()
        {
            bldExecutiveSchemesGroup All_Schemes = new bldExecutiveSchemesGroup("Все схемы");
            foreach (bldExecutiveScheme scheme in _buildingUnitsRepository.ExecutiveSchemes.GetAllAsync().Where(el => !SelectedWork.ExecutiveSchemes.Contains(el)).ToList())
                All_Schemes.Add(scheme);

            NameablePredicate<bldExecutiveSchemesGroup, bldExecutiveScheme> predicate_1 = new NameablePredicate<bldExecutiveSchemesGroup, bldExecutiveScheme>();
            predicate_1.Name = "Показать все документы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<bldExecutiveSchemesGroup, bldExecutiveScheme> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldExecutiveSchemesGroup, bldExecutiveScheme>();
            nameablePredicatesCollection.Add(predicate_1);
            bldExecutiveSchemesGroup schemes_for_add_collection = new bldExecutiveSchemesGroup();

            CoreFunctions.AddElementsToCollectionWhithDialogList<bldExecutiveSchemesGroup, bldExecutiveScheme>
                (schemes_for_add_collection, All_Schemes,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       localUnDoReDo.Register(SelectedWork);
                       UnDoReDo.UnRegister(SelectedWork);
                       foreach (bldExecutiveScheme bld_scheme in schemes_for_add_collection)
                           SelectedWork.AddExecutiveScheme(bld_scheme);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                       UnDoReDo.Register(SelectedWork);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddExecutiveSchemeToCollectionFromListDialogView).Name,
               "Добавить документ",
               "Форма добавления документов.",
               "Список документов", "");
        }

        private void OnRemoveLaboratoryReport()
        {
            bldLaboratoryReport removed_report = SelecteLaboratoryReport;
            if (removed_report == null || SelectedWork == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldLaboratoryReportsGroup, bldLaboratoryReport>
                 (removed_report, "Документ",
                result =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                       SelectedWork.RemoveLaboratoryReport(removed_report);
                    }
                }, _dialogService, Id);
        }

        private void OnAddLaboratoryReports()
        {
            if (SelectedWork == null) return;
            bldLaboratoryReportsGroup All_Materials = new bldLaboratoryReportsGroup("Все лабораторные заключения");
            foreach (bldLaboratoryReport report in _buildingUnitsRepository.LaboratoryReports.GetAllAsync().Where(mt => !SelectedWork.LaboratoryReports.Contains(mt)).ToList())
                All_Materials.Add(report);

            NameablePredicate<bldLaboratoryReportsGroup, bldLaboratoryReport> predicate_1 = new NameablePredicate<bldLaboratoryReportsGroup, bldLaboratoryReport>();
            predicate_1.Name = "Показать все документы";
            predicate_1.Predicate = cl => cl;
            NameablePredicateObservableCollection<bldLaboratoryReportsGroup, bldLaboratoryReport> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldLaboratoryReportsGroup, bldLaboratoryReport>();
            nameablePredicatesCollection.Add(predicate_1);
            bldLaboratoryReportsGroup reports_for_add_collection = new bldLaboratoryReportsGroup();

            CoreFunctions.AddElementsToCollectionWhithDialogList<bldLaboratoryReportsGroup, bldLaboratoryReport>
                (reports_for_add_collection, All_Materials,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       localUnDoReDo.Register(SelectedWork);
                       UnDoReDo.UnRegister(SelectedWork);
                       foreach (bldLaboratoryReport bld_report in reports_for_add_collection)
                           SelectedWork.AddLaboratoryReport(bld_report);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                       UnDoReDo.Register(SelectedWork);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddLaboratoryReportToCollectionFromListDialogView).Name,
               "Добавить документ",
               "Форма добавления документов.",
               "Список документов", "");
        }

        private void OnSaveAOSRsToWord()
        {
            string folder_path = Functions.GetFolderPath();
            SelectedWork.SaveAOSRsToWord(folder_path);
        }

        
        private void OnAddNextWorks()
        {
            if (SelectedWork == null) return;
            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync().Where(wr => wr.Id != SelectedWork.Id &&
                                             !SelectedWork.PreviousWorks.Contains(wr) && !SelectedWork.NextWorks.Contains(wr)).ToList());
            ObservableCollection<bldWork> works_for_add_collection = new ObservableCollection<bldWork>();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_1.Name = "Показать только из текущей конструкции.";
            predicate_1.Predicate = cl => cl.Where(el => el?.bldConstruction != null &&
                                                        el?.bldConstruction.Id == SelectedWork?.bldConstruction?.Id).ToList();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_2 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_2.Name = "Показать на одну ступень выше, но без работ текущей кострукции";
            predicate_2.Predicate = cl => cl.Where(el => el?.bldConstruction?.Id != SelectedWork?.bldConstruction?.Id &&
                                                        (el.bldConstruction?.ParentConstruction?.Id == SelectedWork.bldConstruction?.ParentConstruction?.Id ||
                                                          el.bldConstruction?.bldObject?.Id == SelectedWork.bldConstruction?.bldObject?.Id)).ToList();
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
                       localUnDoReDo.Register(SelectedWork);
                       UnDoReDo.UnRegister(SelectedWork); 
                       foreach (bldWork bld_work in works_for_add_collection)
                           SelectedWork.AddNextWork(bld_work);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                       UnDoReDo.Register(SelectedWork);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddWorksToCollectionFromListDialogView).Name,
               "Добавить работы как последующие",
               "Форма добавления последующих работ.",
               "Список работ", "");

        }
        private void OnAddPreviousWorks()
        {
            if (SelectedWork == null) return;
            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync().Where(wr => wr.Id != SelectedWork.Id &&
                                             !SelectedWork.PreviousWorks.Contains(wr) && !SelectedWork.PreviousWorks.Contains(wr)).ToList());
            ObservableCollection<bldWork> works_for_add_collection = new ObservableCollection<bldWork>();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_1.Name = "Показать только из текущей конструкции.";
            predicate_1.Predicate = cl => cl.Where(el => el?.bldConstruction != null &&
                                                        el?.bldConstruction.Id == SelectedWork?.bldConstruction?.Id).ToList();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_2 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_2.Name = "Показать на одну ступень выше, но без работ текущей кострукции";
            predicate_2.Predicate = cl => cl.Where(el => el?.bldConstruction?.Id != SelectedWork?.bldConstruction?.Id &&
                                                        (el.bldConstruction?.ParentConstruction?.Id == SelectedWork.bldConstruction?.ParentConstruction?.Id ||
                                                          el.bldConstruction?.bldObject?.Id == SelectedWork.bldConstruction?.bldObject?.Id)).ToList();
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
                       localUnDoReDo.Register(SelectedWork);
                       UnDoReDo.UnRegister(SelectedWork);
                       foreach (bldWork bld_work in works_for_add_collection)
                           SelectedWork.AddPreviousWork(bld_work);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                       UnDoReDo.Register(SelectedWork);
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
        private void OnAddMaterials()
        {
            if (SelectedWork == null) return;
            bldMaterialsGroup All_Materials = new bldMaterialsGroup("Все материалы");
            foreach (bldMaterial material in _buildingUnitsRepository.Materials.GetAllAsync().Where(mt => !SelectedWork.Materials.Contains(mt)).ToList())
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
                       localUnDoReDo.Register(SelectedWork);
                       UnDoReDo.UnRegister(SelectedWork);
                       foreach (bldMaterial bld_material in materials_for_add_collection)
                           SelectedWork.AddMaterial(bld_material);
                       SaveCommand.RaiseCanExecuteChanged();
                       UnDoReDo.AddUnDoReDo(localUnDoReDo);
                       UnDoReDo.Register(SelectedWork);
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
        private void OnRemoveMaterial()
        {
            bldMaterial removed_material = SelectedMaterial;
            if (removed_material == null || SelectedWork == null) return;
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldMaterialsGroup, bldMaterial>
                 (removed_material, "Материал",
                result =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDo.Register(SelectedWork);
                        SelectedWork.RemoveMaterial(removed_material);
                    }
                }, _dialogService, Id);
        }
        private void OnDataGridLostSocus(object obj)
        {
        
            if (obj == SelectedPreviousWork)
            {
                SelectedNextWork = null;
                return;
            }
            if (obj == SelectedNextWork)
            {

                SelectedPreviousWork = null;
                return;
            }
            if (obj == SelectedMaterial)
            {
                SelectedMaterial = null;
                return;
            }
            if (obj == SelecteLaboratoryReport)
            {
                SelecteLaboratoryReport = null;
                return;
            }
        }
        private void OnRemovePreviousWork()
        {

            ObservableCollection<bldWork> works_for_remove_collection = new ObservableCollection<bldWork>();

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 ( SelectedPreviousWork, "Предыдущая работа",
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SelectedWork.RemovePreviousWork(SelectedPreviousWork);
                         SelectedPreviousWork = null;
                     }
                 }, _dialogService, Id); ;
        }
        private void OnRemoveNextWork()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (SelectedNextWork, "Последующая работа",
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        SelectedWork.RemoveNextWork(SelectedNextWork);
                        SelectedNextWork = null;
                    }
                }, _dialogService, Id);
        }

          

        private bool CanSave()
        {
            if (SelectedWork != null)
                return !SelectedWork.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public virtual void OnSave()
        {
            base.OnSave<bldWork>(SelectedWork);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldWork>(obj, SelectedWork);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message_work = (ConveyanceObject)navigationContext.Parameters["bld_work"];
            ConveyanceObject navigane_message_construction = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message_work != null)
            {
                ResivedWork = (bldWork)navigane_message_work.Object;
                ResivedConstruction = (bldConstruction)navigane_message_construction.Object;
                //  ResivedObject = (bldObject)navigane_message_object.Object;
                SelectedWork = ResivedWork;
                EditMode = navigane_message_work.EditMode;
                if (SelectedWork != null) SelectedWork.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedWork = ResivedWork;
                SelectedWork.ErrorsChanged += RaiseCanExecuteChanged;
        
                AllDocuments.Clear();
                if (SelectedWork.AOSRDocument!=null) AllDocuments.Add(SelectedWork.AOSRDocument.Id, SelectedWork.AOSRDocument);
                if (SelectedWork.LaboratoryReports.Count > 0) AllDocuments.Add(SelectedWork.LaboratoryReports.Id, SelectedWork.LaboratoryReports);
                if (SelectedWork.ExecutiveSchemes.Count > 0) AllDocuments.Add(SelectedWork.ExecutiveSchemes.Id, SelectedWork.ExecutiveSchemes);
            
                UnDoReDo.Register(SelectedWork);
                Title = $"{SelectedWork.Code} {SelectedWork.ShortName}";
              
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //  AllDocuments.Clear();
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_work"];
            if (((bldWork)navigane_message.Object).Id != SelectedWork.Id)
                return false;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }



    }
}

