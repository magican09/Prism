using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
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

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private bool _keepAlive = true;

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
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

        //public NotifyCommand EditPreviousWorkCommand { get; private set; }
        //public NotifyCommand EditNextWorkCommand { get; private set; }
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
         
            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedWork);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);


            RemovePreviousWorkCommand = new NotifyCommand(OnRemovePreviousWork,
                                        () => SelectedPreviousWork != null)
                    .ObservesProperty(() => SelectedPreviousWork);
            RemoveNextWorkCommand = new NotifyCommand(OnRemoveNextWork,
                                        () => SelectedNextWork != null)
                    .ObservesProperty(() => SelectedNextWork);

            AddPreviousWorkCommand = new NotifyCommand(OnAddPreviousWork);
            AddNextWorkCommand = new NotifyCommand(OnAddNextWork);

            //EditPreviousWorkCommand = new NotifyCommand(OnEditPreviousWork,
            //                            () => SelectedPreviousWork != null)
            //        .ObservesProperty(() => SelectedPreviousWork);
            //EditNextWorkCommand = new NotifyCommand(OnEditNextWork,
            //                            () => SelectedNextWork != null)
            //        .ObservesProperty(() => SelectedNextWork);
            SaveAOSRsToWordCommand = new NotifyCommand(OnSaveAOSRsToWord);

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

        }

        private void OnSaveAOSRsToWord()
        {
            string folder_path = Functions.GetFolderPath();
            SelectedWork.SaveAOSRsToWord(folder_path);
        }

        private void OnEditNextWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedNextWork, "Последующая работа",
                 (result) => { }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", UnDoReDo);

        }
         private void OnAddNextWork()
        {
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

            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldWork>, bldWork>
               (works_for_add_collection, All_Works,
                nameablePredicatesCollection,
               _dialogService,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        foreach (bldWork bld_work in works_for_add_collection)
                        {
                            SelectedWork.AddNextWork(bld_work);
                        }
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                    if (result.Result == ButtonResult.No)
                    {
                    }
                },
               typeof(AddbldWorkToCollectionDialogView).Name,
               typeof(WorkDialogView).Name, Id,
                "Редактирование списка последующих работ",
                "Форма для редактирования.",
                "Список работ", "Все работы");
            //  
        }

        private void OnAddPreviousWork()
        {
            bldWorksGroup All_Works = new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync().Where(wr => wr.Id != SelectedWork.Id&&
                                            !SelectedWork.PreviousWorks.Contains(wr) && !SelectedWork.NextWorks.Contains(wr)).ToList());

            ObservableCollection<bldWork> works_for_add_collection = new ObservableCollection<bldWork>();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_1.Name = "Показать только из текущей конструсции.";
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

            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldWork>, bldWork>
               (works_for_add_collection, All_Works,
                nameablePredicatesCollection,
               _dialogService,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        foreach (bldWork bld_work in works_for_add_collection)
                        {
                            SelectedWork.AddPreviousWork(bld_work);
                        }
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                    if (result.Result == ButtonResult.No)
                    {
                    }
                },
               typeof(AddbldWorkToCollectionDialogView).Name,
               typeof(WorkDialogView).Name, Id,
                "Редактирование списка предыдущих работ",
                "Форма для редактирования.",
                "Список работ", "Все работы");
            //  
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
        }
        private void OnRemovePreviousWork()
        {

            ObservableCollection<bldWork> works_for_remove_collection = new ObservableCollection<bldWork>();

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                 (SelectedWork.PreviousWorks, SelectedPreviousWork, "Предыдущая работа",
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
                 (SelectedWork.NextWorks, SelectedNextWork, "Последующая работа",
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        SelectedWork.RemoveNextWork(SelectedNextWork);
                        SelectedNextWork = null;
                    }
                }, _dialogService, Id);
        }

        
        private void OnEditPreviousWork()
        {
            CoreFunctions.EditElementDialog<bldWork>(SelectedPreviousWork, "Перыдыдущая работа",
                  (result) => { }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", UnDoReDo);
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
                if (SelectedWork.AOSRDocuments.Count > 0) AllDocuments.Add(SelectedWork.AOSRDocuments.Id, SelectedWork.AOSRDocuments);
                if (SelectedWork.LaboratoryReports.Count > 0) AllDocuments.Add(SelectedWork.LaboratoryReports.Id, SelectedWork.LaboratoryReports);
                if (SelectedWork.ExecutiveSchemes.Count > 0) AllDocuments.Add(SelectedWork.ExecutiveSchemes.Id, SelectedWork.ExecutiveSchemes);
                Title = ResivedWork.ShortName;
                UnDoReDo.Register(SelectedWork);

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

