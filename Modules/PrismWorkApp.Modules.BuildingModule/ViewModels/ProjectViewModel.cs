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
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ProjectViewModel : BaseViewModel<bldProject>, INavigationAware

    {
        private string _title = "Проект";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldProject _selectedProject;
        public bldProject SelectedProject
        {
            get { return _selectedProject; }
            set { SetProperty(ref _selectedProject, value); }
        }
        private bldProject _resivedProject;
        public bldProject ResivedProject
        {
            get { return _resivedProject; }
            set { SetProperty(ref _resivedProject, value); }
        }
        private bldObject _selectedBuildingObject;
        public bldObject SelectedBuildingObject
        {
            get { return _selectedBuildingObject; }
            set { SetProperty(ref _selectedBuildingObject, value); }
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
        private bldObjectsGroup _buildingObjects;
        public bldObjectsGroup BuildingObjects
        {
            get { return _buildingObjects; }
            set { SetProperty(ref _buildingObjects, value); }
        }
        private bldParticipantsGroup _participants;
        public bldParticipantsGroup Participants
        {
            get { return _participants; }
            set { SetProperty(ref _participants, value); }
        }
        private bldResponsibleEmployeesGroup _responsibleEmployees;
        public bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }

        private bool _keepAlive = true;

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { SetProperty(ref _keepAlive, value); }
        }


        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }
        public NotifyCommand RemoveBuildingObjectCommand { get; private set; }
        public NotifyCommand RemoveParticipantCommand { get; private set; }
       public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand AddBuildingObjectsCommand { get; private set; }
        public NotifyCommand AddParticipantCommand { get; private set; }
        public NotifyCommand EditBuildingObjectCommand { get; private set; }
        public NotifyCommand EditParticipantCommand { get; private set; }
         public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
    


        public ProjectViewModel(IDialogService dialogService, IBuildingUnitsRepository buildingUnitsRepository,
            IRegionManager regionManager, IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDo)
        {
            //    UnDoReDo = unDoReDo as UnDoReDoSystem;
            UnDoReDo = new UnDoReDoSystem();

            SaveCommand = new NotifyCommand(OnSave, CanSave).ObservesProperty(() => SelectedProject);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                          () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            AddBuildingObjectsCommand = new NotifyCommand(OnAddBuildingObject);
            AddParticipantCommand = new NotifyCommand(OnAddParticipant);
          
            EditBuildingObjectCommand = new NotifyCommand(OnEditBuildingObject,
                                   () => SelectedBuildingObject != null)
               .ObservesProperty(() => SelectedBuildingObject);
            EditParticipantCommand = new NotifyCommand(OnEditParticipant,
                                     () => SelectedParticipant != null)
                 .ObservesProperty(() => SelectedParticipant);
           

            RemoveBuildingObjectCommand = new NotifyCommand(OnRemoveBuildingObject,
                                       () => SelectedBuildingObject != null)
                   .ObservesProperty(() => SelectedBuildingObject);

            RemoveParticipantCommand = new NotifyCommand(OnRemoveParticipant,
                                        () => SelectedParticipant != null)
                    .ObservesProperty(() => SelectedParticipant);
             DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
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
       
        private void OnAddParticipant()
        {
            bldParticipantsGroup All_Participants =
                new bldParticipantsGroup(_buildingUnitsRepository.Pacticipants.GetAllParticipants());

            NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant> predicate_1 = new NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant>();
            NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant> predicate_2 = new NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant>();
            NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant> predicate_3 = new NameablePredicate<ObservableCollection<bldParticipant>, bldParticipant>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id == SelectedProject.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedProject.Id).ToList();
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
                         //   UnDoReDo.Register(participant);
                             SelectedProject.AddParticipant(participant);
                        //    UnDoReDo.UnRegister(participant);

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
        private void OnAddBuildingObject()
        {
            bldObjectsGroup All_BuildingObjects = new bldObjectsGroup(_buildingUnitsRepository.Objects.GetldObjectsAsync());//.GetBldObjects(SelectedProject.Id));

            NameablePredicate<ObservableCollection<bldObject>, bldObject> predicate_1 = new NameablePredicate<ObservableCollection<bldObject>, bldObject>();
            NameablePredicate<ObservableCollection<bldObject>, bldObject> predicate_2 = new NameablePredicate<ObservableCollection<bldObject>, bldObject>();
            NameablePredicate<ObservableCollection<bldObject>, bldObject> predicate_3 = new NameablePredicate<ObservableCollection<bldObject>, bldObject>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id == SelectedProject.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего проекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedProject.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<ObservableCollection<bldObject>, bldObject> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldObject>, bldObject>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);
            ObservableCollection<bldObject> objects_for_add_collection = new ObservableCollection<bldObject>();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldObject>, bldObject>
                (objects_for_add_collection, All_BuildingObjects,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         foreach (bldObject bld_obj in objects_for_add_collection)
                         {
                   //         UnDoReDo.Register(bld_obj);
                             SelectedProject.AddBuildindObject(bld_obj);
                  //          UnDoReDo.UnRegister(bld_obj);
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
        private void OnRemoveBuildingObject()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldObjectsGroup, bldObject>
                  (SelectedProject.BuildingObjects, SelectedBuildingObject, "Строительный объект",
                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          SelectedProject.RemoveBuildindObject(SelectedBuildingObject);
                          SelectedBuildingObject = null;
                          SaveCommand.RaiseCanExecuteChanged();
                      }
                 }, _dialogService, Id);


        }
          private void OnRemoveParticipant()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldParticipantsGroup, bldParticipant>
                 (SelectedProject.Participants, SelectedParticipant, "Учасник строительства",
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SelectedProject.RemoveParticipant(SelectedParticipant);
                         SelectedParticipant = null;
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                }, _dialogService, Id);

        }
        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedBuildingObject)
            {
                // SelectedBuildingObject = null;
                SelectedParticipant = null;
                SelectedResponsibleEmployee = null;
                return;
            }
            if (obj == SelectedParticipant)
            {
                SelectedBuildingObject = null;
                //  SelectedParticipant = null;
                SelectedResponsibleEmployee = null;
                return;
            }
            if (obj == SelectedResponsibleEmployee)
            {
                SelectedBuildingObject = null;
                SelectedParticipant = null;
                //SelectedResponsibleEmployee = null;
                return;
            }
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public virtual bool CanSave()
        {
            if (SelectedProject != null)
                return !SelectedProject.HasErrors;//&& SelectedProject.UnDoReDoSystem.Count>0;
            else
                return false;
        }
        public override void OnSave()
        {
            base.OnSave<bldProject>(SelectedProject);
        }
        public override void OnClose(object obj)
        {
            base.OnClose<bldProject>(obj, SelectedProject);

        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_project"];
            if (navigane_message != null)
            {
                ResivedProject = (bldProject)navigane_message.Object;
                EditMode = navigane_message.EditMode;
                if (SelectedProject != null) SelectedProject.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedProject = ResivedProject;
                SelectedProject.ErrorsChanged += RaiseCanExecuteChanged;
                Title = ResivedProject.ShortName;
               UnDoReDo.Register(SelectedProject);
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_project"];

            if (((bldProject)navigane_message.Object).Id != SelectedProject.Id)
                return false;
            else
                return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }

}
