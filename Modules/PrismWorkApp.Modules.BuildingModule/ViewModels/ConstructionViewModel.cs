using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ConstructionViewModel : BaseViewModel<bldConstruction>, INavigationAware// IRegionMemberLifetime

    {
        private string _title = "Конструкция";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
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
        private bldConstruction _selectedChildConstruction;
        public bldConstruction SelectedChildConstruction
        {
            get { return _selectedChildConstruction; }
            set { SetProperty(ref _selectedChildConstruction, value); }
        }

        private bldWork _selectedWork;
        public bldWork SelectedWork
        {
            get { return _selectedWork; }
            set { SetProperty(ref _selectedWork, value); }
        }

        private bldConstructionsGroup _Constructions;
        public bldConstructionsGroup Constructions
        {
            get { return _Constructions; }
            set { SetProperty(ref _Constructions, value); }
        }
        private bldWorksGroup _Works;
        public bldWorksGroup Works
        {
            get { return _Works; }
            set { SetProperty(ref _Works, value); }
        }


        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }

        public NotifyCommand RemoveConstructionCommand { get; private set; }
        public NotifyCommand RemoveWorkCommand { get; private set; }

        public NotifyCommand AddConstructionCommand { get; private set; }
        public NotifyCommand AddWorkCommand { get; private set; }

        public NotifyCommand EditConstructionCommand { get; private set; }
        public NotifyCommand EditWorkCommand { get; private set; }

        public NotifyCommand SaveAOSRsToWordCommand { get; private set; }
        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;

        public ConstructionViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands,
              IPropertiesChangeJornal propertiesChangeJornal)
        {
            CommonChangeJornal = propertiesChangeJornal as PropertiesChangeJornal;

            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            SaveCommand = new NotifyCommand(OnSave, CanSave).ObservesProperty(() => SelectedConstruction);
            CloseCommand = new NotifyCommand<object>(OnClose);
            #region Add Commands
            AddWorkCommand = new NotifyCommand(OnAddWork);
            AddConstructionCommand = new NotifyCommand(OnAddConstruction);
            #endregion
            #region Remove Commands
            RemoveConstructionCommand = new NotifyCommand(OnRemoveConstruction,
                                                () => SelectedChildConstruction != null)
                            .ObservesProperty(() => SelectedChildConstruction);
            RemoveWorkCommand = new NotifyCommand(OnRemoveWork,
                                        () => SelectedWork != null)
                    .ObservesProperty(() => SelectedWork);
            #endregion
            #region Edit Commands

            EditConstructionCommand = new NotifyCommand(OnEditConstruction,
                                        () => SelectedChildConstruction != null)
                    .ObservesProperty(() => SelectedChildConstruction);
            EditWorkCommand = new NotifyCommand(OnEditWork,
                                        () => SelectedWork != null)
                    .ObservesProperty(() => SelectedWork);
            #endregion


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
            SelectedConstruction.SaveAOSRsToWord(folder_path);
        }

        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedChildConstruction)
            {
                SelectedWork = null;
                return;
            }
            if (obj == SelectedWork)
            {

                SelectedChildConstruction = null;
                return;
            }
        }
        private void OnAddWork()
        {
            bldWorksGroup Works =
                new bldWorksGroup(_buildingUnitsRepository.Works.GetbldWorksAsync());
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_1 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_1.Name = "Показать только из текущего объекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldConstruction.bldObject != null &&
                                                        el.bldConstruction.bldObject.Id == SelectedConstruction.bldObject.Id).ToList();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_2 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_2.Name = "Показать все кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldConstruction.bldObject != null &&
                                                          el.bldConstruction?.bldObject.Id != SelectedConstruction?.bldObject.Id).ToList();
            NameablePredicate<ObservableCollection<bldWork>, bldWork> predicate_3 = new NameablePredicate<ObservableCollection<bldWork>, bldWork>();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldWork>, bldWork>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            ObservableCollection<bldWork> objects_for_add_collection = new ObservableCollection<bldWork>();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldWork>, bldWork>
                (objects_for_add_collection, Works,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         foreach (bldWork work in objects_for_add_collection)
                         {
                             UnDoReDoSystem.Register(work);
                             SelectedConstruction.AddWork(work);
                             UnDoReDoSystem.UnRegister(work);
                         }
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                     if (result.Result == ButtonResult.No)
                     {
                     }
                 },
                typeof(AddbldWorkToCollectionDialogView).Name,
                typeof(WorkDialogView).Name, Id,
                "Редактирование списка работ",
                "Форма для редактирования состава работ текушей коснтрукции.",
                "Работы текущей конструкции", "Все работы");
        }
        private void OnAddConstruction()
        {
            bldConstructionsGroup Constructions =
            new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetbldConstructionsAsync());
            NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction> predicate_1 = new NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldObject?.bldProject.Id == SelectedConstruction?.bldObject?.bldProject?.Id).ToList();
            NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction> predicate_2 = new NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction>();
            predicate_2.Name = "Показать все кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldObject?.Id != SelectedConstruction?.bldObject?.Id).ToList();
            NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction> predicate_3 = new NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction>();
            predicate_3.Name = "Показать  из  все из других проектов";
            predicate_3.Predicate = cl => cl.Where(el => el.bldObject?.bldProject.Id != SelectedConstruction?.bldObject?.bldProject?.Id).ToList();
            NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction> predicate_4 = new NameablePredicate<ObservableCollection<bldConstruction>, bldConstruction>();
            predicate_4.Name = "Показать все";
            predicate_4.Predicate = cl => cl;

            NameablePredicateObservableCollection<ObservableCollection<bldConstruction>, bldConstruction> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldConstruction>, bldConstruction>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            nameablePredicatesCollection.Add(predicate_4);
            ObservableCollection<bldConstruction> objects_for_add_collection = new ObservableCollection<bldConstruction>();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldConstruction>, bldConstruction>
                (objects_for_add_collection, Constructions,
                nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         foreach (bldConstruction construction in objects_for_add_collection)
                         {
                             UnDoReDoSystem.Register(construction);
                             SelectedConstruction.AddConstruction(construction);
                             UnDoReDoSystem.UnRegister(construction);
                         }
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                     if (result.Result == ButtonResult.No)
                     {

                     }
                 },
                typeof(AddbldConstructionToCollectionDialogView).Name,
                typeof(ConstructionDialogView).Name, Id,
                "Редактирование списка конструкций",
                "Форма для редактирования состава коснструций объекта.",
                "Конструкции текущего объекта", "Все конструкции");
        }
        private void OnEditWork()
        {
            UnDoReDoSystem.Register(SelectedWork);
            CoreFunctions.EditElementDialog<bldWork>(SelectedWork, "Работа",
                  (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(WorkDialogView).Name, "Редактировать", Guid.Empty);
            UnDoReDoSystem.UnRegister(SelectedWork);
        
        }
        private void OnEditConstruction()
        {
            UnDoReDoSystem.Register(SelectedChildConstruction);
            CoreFunctions.EditElementDialog<bldConstruction>(SelectedChildConstruction, "Строительная конструкция",
                (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", Guid.Empty);
            UnDoReDoSystem.UnRegister(SelectedChildConstruction);
        }
        private void OnRemoveWork()
        {

            
          
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldWorksGroup, bldWork>
                  (SelectedConstruction.Works, SelectedWork, "Работу",
                 () =>
                 {
                     SelectedConstruction.RemoveWork(SelectedWork);
                     SelectedWork = null;
                     SaveCommand.RaiseCanExecuteChanged();
                 }, _dialogService, Guid.Empty);
        }
        private void OnRemoveConstruction()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldConstructionsGroup, bldConstruction>
                  (SelectedConstruction.Constructions, SelectedChildConstruction, "Строительную конструкцию",
                 () =>
                 {
                     SelectedConstruction.RemoveConstruction(SelectedChildConstruction);
                     SelectedChildConstruction = null;
                     SaveCommand.RaiseCanExecuteChanged();
                 }, _dialogService, Guid.Empty);
        }
        private bool CanSave()
        {
            if (SelectedConstruction != null)
                return !SelectedConstruction.HasErrors;// && SelectedConstruction.PropertiesChangeJornal.Count > 0;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public virtual void OnSave()
        {
            this.OnSave<bldConstruction>(SelectedConstruction);
        }
        public virtual void OnClose(object obj)
        {
            this.OnClose<bldConstruction>(obj, SelectedConstruction);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (navigane_message != null)
            {
                ResivedConstruction = (bldConstruction)navigane_message.Object;
                SelectedConstruction = ResivedConstruction;
                EditMode = navigane_message.EditMode;
                if (SelectedConstruction != null) SelectedConstruction.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedConstruction = ResivedConstruction;
                SelectedConstruction.ErrorsChanged += RaiseCanExecuteChanged;
                Title = ResivedConstruction.Name;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_construction"];
            if (((bldConstruction)navigane_message.Object).Id != SelectedConstruction.Id)
                return false;
            else
                return true;
        }




    }

}
