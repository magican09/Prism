using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.ProjectModel.Data.Models;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using BindableBase = Prism.Mvvm.BindableBase;

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
            set { SetProperty(ref _keepAlive , value); }
        }

        
        // public  NotifyCommand SaveCommand { get; private set; }
        public NotifyCommand SaveCommand { get; private set; }
        public NotifyCommand UnDoLeftCommand { get; private set; }
        public NotifyCommand UnDoRightCommand { get; private set; }

        public NotifyCommand TestCommand { get; private set; }
        public NotifyCommand<object> CloseCommand { get; private set; }

        public NotifyCommand RemoveBuildingObjectCommand { get; private set; }
        public NotifyCommand RemoveParticipantCommand { get; private set; }
        public NotifyCommand RemoveResponsibleEmployeeCommand { get; private set; }
        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand AddBuildingObjectsCommand { get; private set; }
        public NotifyCommand AddParticipantCommand { get; private set; }
        public NotifyCommand AddResponsibleEmployeesCommand { get; private set; }
        public NotifyCommand EditBuildingObjectCommand { get; private set; }
        public NotifyCommand EditParticipantCommand { get; private set; }
        public NotifyCommand EditResponsibleEmployeeCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
          private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        public PropertiesChangeJornal _commonChangeJornal { get; set; } = new PropertiesChangeJornal();

        public ProjectViewModel(IDialogService dialogService, IBuildingUnitsRepository buildingUnitsRepository,
            IRegionManager regionManager, IApplicationCommands applicationCommands, IPropertiesChangeJornal propertiesChangeJornal)
        {
            //  SaveCommand = new NotifyCommand(OnSave, CanSave);
            _commonChangeJornal = propertiesChangeJornal as PropertiesChangeJornal;

            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedProject).ObservesCanExecute(()=>KeepAlive);
            UnDoLeftCommand = new NotifyCommand(OnUnDoLeft, CanUnDoLeft);
            UnDoRightCommand = new NotifyCommand(OnUnDoRight, CanUnDoRight);
            
              TestCommand = new NotifyCommand(OnTestCommand);

            CloseCommand = new NotifyCommand<object>(OnClose).ObservesCanExecute(() => KeepAlive); ;
            AddBuildingObjectsCommand = new NotifyCommand(OnAddBuildingObject);
            AddParticipantCommand = new NotifyCommand(OnAddParticipant);
            AddResponsibleEmployeesCommand = new NotifyCommand(OnAddResponsibleEmployees);

            EditBuildingObjectCommand = new NotifyCommand(OnEditBuildingObject,
                                   () => SelectedBuildingObject != null)
               .ObservesProperty(() => SelectedBuildingObject);
            EditParticipantCommand = new NotifyCommand(OnEditParticipant,
                                     () => SelectedParticipant != null)
                 .ObservesProperty(() => SelectedParticipant);
            EditResponsibleEmployeeCommand = new NotifyCommand(OnEditRemoveResponsibleEmployee,
                                        () => SelectedResponsibleEmployee != null)
                    .ObservesProperty(() => SelectedResponsibleEmployee);

            RemoveBuildingObjectCommand = new NotifyCommand(OnRemoveBuildingObject,
                                       () => SelectedBuildingObject != null)
                   .ObservesProperty(() => SelectedBuildingObject);

            RemoveParticipantCommand = new NotifyCommand(OnRemoveParticipant,
                                        () => SelectedParticipant != null)
                    .ObservesProperty(() => SelectedParticipant);
            RemoveResponsibleEmployeeCommand = new NotifyCommand(OnRemoveResponsibleEmployee,
                                        () => SelectedResponsibleEmployee != null)
                    .ObservesProperty(() => SelectedResponsibleEmployee);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
             _applicationCommands.UnDoRightCommand.RegisterCommand(UnDoRightCommand);
            _applicationCommands.UnDoLeftCommand.RegisterCommand(UnDoLeftCommand);
        }

        private bool CanUnDoRight()
        {
            return true;
        }

        private void OnUnDoRight()
        {
            _commonChangeJornal.UnDoRight(this.Id);
        }

        private bool CanUnDoLeft()
        {
            return true;
        }

        private void OnUnDoLeft()
        {
            _commonChangeJornal.UnDoLeft(this.Id);
        }

        private bool CanTest()
        {
           return  true;
        }

        private void OnTestCommand()
        {
            KeepAlive = !KeepAlive;
        }

        private void OnEditRemoveResponsibleEmployee()
        {
            CoreFunctions.EditElementDialog<bldResponsibleEmployee>(SelectedResponsibleEmployee, "Отвественне лицо",
                  (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ResponsibleEmployeeDialogView).Name, "Редактировать", Id);
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnEditParticipant()
        {
            CoreFunctions.EditElementDialog<bldParticipant>(SelectedParticipant, "Учасник строительства",
                  (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ParticipantDialogView).Name, "Редактировать", Id);
           
        }


        private void OnAddResponsibleEmployees()
        {
            bldResponsibleEmployeesGroup All_ResponsibleEmployees = new bldResponsibleEmployeesGroup(
                    _buildingUnitsRepository.ResponsibleEmployees.GetAllResponsibleEmployees());

            NameablePredicate<bldResponsibleEmployeesGroup, bldResponsibleEmployee> predicate_1 = new NameablePredicate<bldResponsibleEmployeesGroup, bldResponsibleEmployee>();
            NameablePredicate<bldResponsibleEmployeesGroup, bldResponsibleEmployee> predicate_2 = new NameablePredicate<bldResponsibleEmployeesGroup, bldResponsibleEmployee>();
            NameablePredicate<bldResponsibleEmployeesGroup, bldResponsibleEmployee> predicate_3 = new NameablePredicate<bldResponsibleEmployeesGroup, bldResponsibleEmployee>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id == SelectedProject.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedProject.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldResponsibleEmployeesGroup, bldResponsibleEmployee> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldResponsibleEmployeesGroup, bldResponsibleEmployee>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);

            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldResponsibleEmployeesGroup, bldResponsibleEmployee>
                 (SelectedProject.ResponsibleEmployees, All_ResponsibleEmployees,
                 nameablePredicatesCollection,
                 _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                         SelectedProject.ResponsibleEmployees = (bldResponsibleEmployeesGroup)
                                result.Parameters.GetValue<object>("current_collection");
                     SaveCommand.RaiseCanExecuteChanged();
                 },
                 typeof(AddbldResponsibleEmployeeToCollectionDialogView).Name,
                 typeof(ResponsibleEmployeeDialogView).Name, Id,
                  "Редактирование списка отвественных работников",
                 "Форма для редактирования отвественных.",
                 "Ответсвенные текущего проекта", "Все отвественные лица");
        }
        private void OnAddParticipant()
        {
            bldParticipantsGroup All_Participants =
                new bldParticipantsGroup(_buildingUnitsRepository.Pacticipants.GetAllParticipants());

          
            NameablePredicate<bldParticipantsGroup, bldParticipant> predicate_1 = new NameablePredicate<bldParticipantsGroup, bldParticipant>();
            NameablePredicate<bldParticipantsGroup, bldParticipant> predicate_2 = new NameablePredicate<bldParticipantsGroup, bldParticipant>();
            NameablePredicate<bldParticipantsGroup, bldParticipant> predicate_3 = new NameablePredicate<bldParticipantsGroup, bldParticipant>();
             predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id == SelectedProject.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedProject.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldParticipantsGroup, bldParticipant> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldParticipantsGroup, bldParticipant>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);

            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldParticipantsGroup, bldParticipant>
                (SelectedProject.Participants, All_Participants,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                         foreach (bldParticipant participant in SelectedProject.Participants)
                            participant.bldProject = SelectedProject;
                     }
                     if (result.Result == ButtonResult.No)
                     {
                         SelectedProject.Participants.UnDoAll(Id);
                     }
                 },
                typeof(AddbldParticipantToCollectionDialogView).Name,
                typeof(ParticipantDialogView).Name, Id,
                "Редактирование списка объектов",
                "Форма для редактирования состава объектов текушего проекта.",
                "Объекты текущего проекта", "Все объекта");

        
    
        }

        private void OnEditBuildingObject()
        {
            CoreFunctions.EditElementDialog<bldObject>(SelectedBuildingObject, "Строительный объект",
                (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ObjectDialogView).Name, "Редактировать", Id);
        }
        private void OnAddBuildingObject()
        {
            bldObjectsGroup All_BuildingObjects = new bldObjectsGroup(_buildingUnitsRepository.Objects.GetldObjectsAsync());//.GetBldObjects(SelectedProject.Id));

            NameablePredicate<bldObjectsGroup, bldObject> predicate_1 = new NameablePredicate<bldObjectsGroup, bldObject>();
            NameablePredicate<bldObjectsGroup, bldObject> predicate_2 = new NameablePredicate<bldObjectsGroup, bldObject>();
            NameablePredicate<bldObjectsGroup, bldObject> predicate_3 = new NameablePredicate<bldObjectsGroup, bldObject>(); 
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el.bldProject  != null &&
                                                        el.bldProject.Id == SelectedProject.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего объекта";
            predicate_2.Predicate = cl => cl.Where(el => el.bldProject != null &&
                                                        el.bldProject.Id != SelectedProject.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;

            NameablePredicateObservableCollection<bldObjectsGroup, bldObject> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldObjectsGroup, bldObject>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);

            CoreFunctions.AddElementToCollectionWhithDialog_Test<bldObjectsGroup,bldObject>
                (SelectedProject.BuildingObjects, All_BuildingObjects,
                 nameablePredicatesCollection,
                _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                         foreach (bldObject bld_obj in SelectedProject.BuildingObjects)
                             bld_obj.bldProject=SelectedProject;
                     }
                     if (result.Result == ButtonResult.No)
                     {
                         SelectedProject.BuildingObjects.UnDoAll(Id);
                     }
                 },
                typeof(AddbldObjectToCollectionDialogView).Name,
                typeof(ObjectDialogView).Name, Id,
                "Редактирование списка объектов",
                "Форма для редактирования состава объектов текушего проекта.",
                "Объекты текущего проекта", "Все объекта");

        }
        private void OnRemoveBuildingObject()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldObjectsGroup, bldObject>
                  (SelectedProject.BuildingObjects, SelectedBuildingObject, "Строительный объект",
                 () => { SelectedBuildingObject = null; SaveCommand.RaiseCanExecuteChanged(); }, _dialogService);

           
        }
        private void OnRemoveResponsibleEmployee()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldResponsibleEmployeesGroup, bldResponsibleEmployee>
                (SelectedProject.ResponsibleEmployees, SelectedResponsibleEmployee, "Ответсвенный представитель",
                () => { SelectedResponsibleEmployee = null; SaveCommand.RaiseCanExecuteChanged(); }, _dialogService);
        }
        private void OnRemoveParticipant()
        {

            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldParticipantsGroup, bldParticipant>
                 (SelectedProject.Participants, SelectedParticipant, "Учасник строительства",
                 () => { SelectedParticipant = null; SaveCommand.RaiseCanExecuteChanged(); }, _dialogService);
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

        public virtual bool CanSave()
        {
            if (SelectedProject != null)
                return !SelectedProject.HasErrors;//&& SelectedProject.PropertiesChangeJornal.Count>0;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public override void OnSave()
        {
                this.OnSave<bldProject>(SelectedProject);
        }
        public override void OnClose(object obj)
        {
            this.OnClose<bldProject>(obj, SelectedProject);
           
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
        //   base.OnCloseWindow();
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
