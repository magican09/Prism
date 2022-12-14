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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ObjectViewModel : BaseViewModel<bldObject>, INotifyPropertyChanged, INavigationAware
    {

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bldObject _selectedObject;
        public bldObject SelectedBuildingObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value); }
        }
        private bldObject _resivedObject;
        public bldObject ResivedObject
        {
            get { return _resivedObject; }
            set { SetProperty(ref _resivedObject, value); }
        }
        private bldObject _ChildSelectedBuildingObject;
        public bldObject SelectedChildBuildingObject
        {
            get { return _ChildSelectedBuildingObject; }
            set { SetProperty(ref _ChildSelectedBuildingObject, value); }
        }

        private bldObjectsGroup _objects;
        public bldObjectsGroup Objects
        {
            get { return _objects; }
            set { SetProperty(ref _objects, value); }
        }
        private bldObjectsGroup _allObjects;
        public bldObjectsGroup AllObjects
        {
            get { return _allObjects; }
            set { SetProperty(ref _allObjects, value); }
        }
        private bldObjectsGroup _buildingObjects;
        public bldObjectsGroup BuildingObjects
        {
            get { return _buildingObjects; }
            set { SetProperty(ref _buildingObjects, value); }
        }


        private bldConstruction _selectedConstruction;
        public bldConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set { SetProperty(ref _selectedConstruction, value); }
        }
        private bldParticipant _selectedParticipant;
        public bldParticipant SelectedParticipant
        {
            get { return _selectedParticipant; }
            set { SetProperty(ref _selectedParticipant, value); }
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
        public NotifyCommand AddBuildingObjectsCommand { get; private set; }
        public NotifyCommand AddParticipantCommand { get; private set; }
        public NotifyCommand AddResponsibleEmployeesCommand { get; private set; }
        public NotifyCommand AddConstructionCommand { get; private set; }
       
        public NotifyCommand EditBuildingObjectCommand { get; private set; }
        public NotifyCommand EditConstructionCommand { get; private set; }
        public NotifyCommand EditParticipantCommand { get; private set; }
        public NotifyCommand EditResponsibleEmployeeCommand { get; private set; }

        public NotifyCommand RemoveBuildingObjectCommand { get; private set; }
        public NotifyCommand RemoveConstructionCommand { get; private set; }
        public NotifyCommand RemoveParticipantCommand { get; private set; }
        public NotifyCommand RemoveResponsibleEmployeeCommand { get; private set; }
        public NotifyCommand SaveAOSRsToWordCommand { get; private set; }


        private readonly IBuildingUnitsRepository _buildingUnitsRepository;
        private IApplicationCommands _applicationCommands;
        public ObjectViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository,
             IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDo)
        {
            UnDoReDoSystem CommonUnDoReDo = unDoReDo as UnDoReDoSystem;
            UnDoReDo = new UnDoReDoSystem();
         
            SaveCommand = new NotifyCommand(OnSave, CanSave).ObservesProperty(() => SelectedBuildingObject);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                               () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            #region Add Commands
            AddBuildingObjectsCommand = new NotifyCommand(OnAddBuildingObject);
            AddParticipantCommand = new NotifyCommand(OnAddParticipant);
            AddResponsibleEmployeesCommand = new NotifyCommand(OnAddResponsibleEmployees);
            AddConstructionCommand = new NotifyCommand(OnAddConstruction);
            #endregion
            #region Remove Commands
            RemoveBuildingObjectCommand = new NotifyCommand(OnRemoveBuildingObject,
                                     () => SelectedChildBuildingObject != null)
                 .ObservesProperty(() => SelectedChildBuildingObject);
            RemoveConstructionCommand = new NotifyCommand(OnRemoveConstruction,
                                        () => SelectedConstruction != null)
                .ObservesProperty(() => SelectedConstruction);
            RemoveParticipantCommand = new NotifyCommand(OnRemoveParticipant,
                                       () => SelectedParticipant != null)
                   .ObservesProperty(() => SelectedParticipant);
            RemoveResponsibleEmployeeCommand = new NotifyCommand(OnRemoveResponsibleEmployee,
                                        () => SelectedResponsibleEmployee != null)
                    .ObservesProperty(() => SelectedResponsibleEmployee);
            #endregion
            #region Edit Commands
            EditBuildingObjectCommand = new NotifyCommand(OnEditBuildingObject,
                                         () => SelectedChildBuildingObject != null)
                     .ObservesProperty(() => SelectedChildBuildingObject);
            EditConstructionCommand = new NotifyCommand(OnEditConstruction,
                                         () => SelectedConstruction != null)
                     .ObservesProperty(() => SelectedConstruction);
            EditParticipantCommand = new NotifyCommand(OnEditParticipant,
                                     () => SelectedParticipant != null)
                 .ObservesProperty(() => SelectedParticipant);
            EditResponsibleEmployeeCommand = new NotifyCommand(OnEditResponsibleEmployee,
                                        () => SelectedResponsibleEmployee != null)
                    .ObservesProperty(() => SelectedResponsibleEmployee);
            #endregion

            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            SaveAOSRsToWordCommand = new NotifyCommand(OnSaveAOSRsToWord);

            _dialogService = dialogService;
            _regionManager = regionManager;
            _buildingUnitsRepository = buildingUnitsRepository;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
        }
        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedChildBuildingObject)
            {
                SelectedConstruction = null;
                return;
            }
            if (obj == SelectedConstruction)
            {
                SelectedChildBuildingObject = null;
                return;
            }

        }

        private void OnAddBuildingObject()
        {
            bldObjectsGroup All_BuildingObjects = new bldObjectsGroup(_buildingUnitsRepository.Objects.GetldObjectsAsync().Where(ob=>ob.Id!=SelectedBuildingObject.Id).ToList());//.GetBldObjects(SelectedProject.Id));

            NameablePredicate<bldObjectsGroup, bldObject> predicate_1 = new NameablePredicate<bldObjectsGroup, bldObject>();
            NameablePredicate<bldObjectsGroup, bldObject> predicate_2 = new NameablePredicate<bldObjectsGroup, bldObject>();
            NameablePredicate<bldObjectsGroup, bldObject> predicate_3 = new NameablePredicate<bldObjectsGroup, bldObject>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id == SelectedBuildingObject?.bldProject?.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего проекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedBuildingObject?.bldProject?.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldObjectsGroup, bldObject> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldObjectsGroup, bldObject>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            bldObjectsGroup objects_for_add_collection = new bldObjectsGroup();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldObjectsGroup, bldObject>
                (objects_for_add_collection, All_BuildingObjects,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         foreach (bldObject bld_obj in objects_for_add_collection)
                         {
                             SelectedBuildingObject.AddBuildindObject(bld_obj);
                         }
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                     if (result.Result == ButtonResult.No)
                     {

                     }
                 },
                typeof(AddbldObjectToCollectionDialogView).Name,
                typeof(ObjectDialogView).Name, Id,
                "Редактирование списка объектов",
                "Форма для редактирования состава объектов текушего проекта.",
                "Объекты текущего проекта", "Все объекта");
            //  
        }
        private void OnAddConstruction()
        {
            bldConstructionsGroup AllConstructions =
            new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetbldConstructionsAsync());
         //   new bldConstructionsGroup(_buildingUnitsRepository.Constructions.GetbldConstructionsAsync().Where(cn => !SelectedBuildingObject.Constructions.Contains(cn)).ToList());
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_1 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldObject?.bldProject?.Id == SelectedBuildingObject?.bldProject?.Id).ToList();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_2 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_2.Name = "Показать все кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldObject?.Id != SelectedBuildingObject?.Id).ToList();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_3 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_3.Name = "Показать  из  все из других проектов";
            predicate_3.Predicate = cl => cl.Where(el => el.bldObject?.bldProject?.Id != SelectedBuildingObject?.bldProject?.Id).ToList();
            NameablePredicate<bldConstructionsGroup, bldConstruction> predicate_4 = new NameablePredicate<bldConstructionsGroup, bldConstruction>();
            predicate_4.Name = "Показать все";
            predicate_4.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldConstructionsGroup, bldConstruction> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldConstructionsGroup, bldConstruction>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            nameablePredicatesCollection.Add(predicate_4);
            bldConstructionsGroup objects_for_add_collection = new bldConstructionsGroup();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldConstructionsGroup, bldConstruction>
                (objects_for_add_collection, AllConstructions,
                nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                        foreach (bldConstruction construction in objects_for_add_collection)
                         {
                             SelectedBuildingObject.AddConstruction(construction);
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
        private void OnAddParticipant()
        {
            bldParticipantsGroup All_Participants =
                new bldParticipantsGroup(_buildingUnitsRepository.Pacticipants.GetAllParticipants());

            NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant> predicate_1 = new NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant>();
            NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant> predicate_2 = new NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant>();
            NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant> predicate_3 = new NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id == SelectedBuildingObject?.bldProject?.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedBuildingObject?.bldProject?.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<ObservableCollection<bldParticipant>, bldParticipant> nameablePredicatesCollection =
                new NameablePredicateObservableCollection<ObservableCollection<bldParticipant>, bldParticipant>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            ObservableCollection<bldParticipant> collection_for_add = new ObservableCollection<bldParticipant>();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldParticipant>, bldParticipant>
                (collection_for_add, All_Participants,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                         foreach (bldParticipant participant in collection_for_add)
                         {
                             SelectedBuildingObject.AddParticipant(participant);
                         }
                     }
                     if (result.Result == ButtonResult.No)
                     {
                     }
                 },
                typeof(AddbldParticipantToCollectionDialogView).Name,
                typeof(ParticipantDialogView).Name, Id,
                "Редактирование списка объектов",
                "Форма для редактирования состава объектов текушего проекта.",
                "Объекты текущего проекта", "Все объекта");
        }
        private void OnAddResponsibleEmployees()
        {
            //bldResponsibleEmployeesGroup All_ResponsibleEmployees = new bldResponsibleEmployeesGroup(
            //        _buildingUnitsRepository.ResponsibleEmployees.GetAllResponsibleEmployees());
            //NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> predicate_1 = new NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            //NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> predicate_2 = new NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            //NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> predicate_3 = new NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            //predicate_1.Name = "Показать только из текущего проекта.";
            ////predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
            ////                                            el.bldProject.Id == SelectedBuildingObject?.bldProject?.Id).ToList();
            //predicate_2.Name = "Показать из всех кроме текущего проекта";
            ////predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
            ////                                            el.bldProject.Id != SelectedBuildingObject?.bldProject?.Id).ToList();
            ////predicate_3.Name = "Показать все";
            //predicate_3.Predicate = cl => cl;
            //NameablePredicateObservableCollection<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            //nameablePredicatesCollection.Add(predicate_1);
            //nameablePredicatesCollection.Add(predicate_2);
            //nameablePredicatesCollection.Add(predicate_3);

            //ObservableCollection<bldResponsibleEmployee> collection_for_add = new ObservableCollection<bldResponsibleEmployee>();
            //CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>
            //     (collection_for_add, All_ResponsibleEmployees,
            //     nameablePredicatesCollection,
            //     _dialogService,
            //     (result) =>
            //     {
            //         if (result.Result == ButtonResult.Yes)
            //         {
            //             SaveCommand.RaiseCanExecuteChanged();
            //             foreach (bldResponsibleEmployee employee in collection_for_add)
            //             {
            //                 SelectedBuildingObject.AddResponsibleEmployee(employee);
            //             }
            //         }
            //     },
            //     typeof(AddbldResponsibleEmployeeToCollectionDialogView).Name,
            //     typeof(ResponsibleEmployeeDialogView).Name, Id,
            //      "Редактирование списка отвественных работников",
            //     "Форма для редактирования отвественных.",
            //     "Ответсвенные текущего проекта", "Все отвественные лица");
        }

        private void OnEditBuildingObject()
        {
            CoreFunctions.EditElementDialog<bldObject>(SelectedBuildingObject, "Строительный объект",
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDoSystem undoredu_from_editDialog = result.Parameters.GetValues<UnDoReDoSystem>("undo_redo").FirstOrDefault();
                        UnDoReDo.AddUnDoReDo(undoredu_from_editDialog);
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                }, _dialogService, typeof(ObjectDialogView).Name, "Редактировать", UnDoReDo);
         }
        private void OnEditConstruction()
        {
             CoreFunctions.EditElementDialog<bldConstruction>(SelectedConstruction, "Строительная конструкция",
                (result) => 
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        UnDoReDoSystem undoredu_from_editDialog = result.Parameters.GetValues<UnDoReDoSystem>("undo_redo").FirstOrDefault();
                        UnDoReDo.AddUnDoReDo(undoredu_from_editDialog);
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                }, _dialogService, typeof(ConstructionDialogView).Name, "Редактировать", UnDoReDo);
         }
        private void OnEditResponsibleEmployee()
        {

            CoreFunctions.EditElementDialog<bldResponsibleEmployee>(SelectedResponsibleEmployee, "Отвественне лицо",
                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          UnDoReDoSystem undoredu_from_editDialog = result.Parameters.GetValues<UnDoReDoSystem>("undo_redo").FirstOrDefault();
                          UnDoReDo.AddUnDoReDo(undoredu_from_editDialog);
                          SaveCommand.RaiseCanExecuteChanged();
                      }
                  }, _dialogService, typeof(ResponsibleEmployeeDialogView).Name, "Редактировать", UnDoReDo);
 
        }
        private void OnEditParticipant()
        {
             CoreFunctions.EditElementDialog<bldParticipant>(SelectedParticipant, "Учасник строительства",
                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          UnDoReDoSystem undoredu_from_editDialog = result.Parameters.GetValues<UnDoReDoSystem>("undo_redo").FirstOrDefault();
                          UnDoReDo.AddUnDoReDo(undoredu_from_editDialog);
                          SaveCommand.RaiseCanExecuteChanged();
                      }
                  }, _dialogService, typeof(ParticipantDialogView).Name, "Редактировать", UnDoReDo);
          

        }

        private void OnRemoveBuildingObject()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldObjectsGroup, bldObject>
                  (SelectedChildBuildingObject, "Строительный объект",
                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          SelectedBuildingObject.RemoveBuildindObject(SelectedChildBuildingObject);
                          SelectedChildBuildingObject = null;
                          SaveCommand.RaiseCanExecuteChanged();
                      }
                 }, _dialogService, Id);
       }
        private void OnRemoveConstruction()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldConstructionsGroup, bldConstruction>
                  ( SelectedConstruction, "Строительную конструкцию",
                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          SelectedBuildingObject.RemoveConstruction(SelectedConstruction);
                          SelectedConstruction = null;
                          SaveCommand.RaiseCanExecuteChanged();
                      }
                 }, _dialogService, Id);
        }
        private void OnRemoveResponsibleEmployee()
        {
            //CoreFunctions.RemoveElementFromCollectionWhithDialog<bldResponsibleEmployeesGroup, bldResponsibleEmployee>
            //     (SelectedBuildingObject.ResponsibleEmployees, SelectedResponsibleEmployee, "Ответсвенный представитель",
            //    () =>
            //    {
            //        SelectedBuildingObject.RemoveResponsibleEmployee(SelectedResponsibleEmployee);
            //        SelectedResponsibleEmployee = null;
            //        SaveCommand.RaiseCanExecuteChanged();
            //    }, _dialogService, Id);

        }
        private void OnRemoveParticipant()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldParticipantsGroup, bldParticipant>
                 ( SelectedParticipant, "Учасник строительства",
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SelectedBuildingObject.RemoveParticipant(SelectedParticipant);
                         SelectedParticipant = null;
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                }, _dialogService, Id);

        }

        private void OnSaveAOSRsToWord()
        {
            string folder_path = Functions.GetFolderPath();
            SelectedBuildingObject.SaveAOSRsToWord(folder_path);
        }

        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public virtual bool CanSave()
        {
            if (SelectedBuildingObject != null)
                return !SelectedBuildingObject.HasErrors;// && SelectedBuildingObject.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        public virtual void OnSave()
        {
            base.OnSave<bldObject>(SelectedBuildingObject);
          
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldObject>(obj, SelectedBuildingObject);
        }

        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_object"];

            if (((bldObject)navigane_message.Object).Id != SelectedBuildingObject.Id)
                return false;
            else
                return true;

        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_object"];
            if (navigane_message != null)
            {
                ResivedObject = (bldObject)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                if (SelectedBuildingObject != null) SelectedBuildingObject.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedBuildingObject = ResivedObject;
                SelectedBuildingObject.ErrorsChanged += RaiseCanExecuteChanged;
                Title = ResivedObject.Name;
               UnDoReDo.Register(SelectedBuildingObject);
                Title = $"{SelectedBuildingObject.Code} {SelectedBuildingObject.ShortName}";
            }
        }
    }
}
