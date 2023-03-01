using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ParticipantViewModel : BaseViewModel<bldParticipant>, INotifyPropertyChanged, INavigationAware
    {
        private string _title = "Учасник строительства";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldParticipant _selectedParticipant;
        public bldParticipant SelectedParticipant
        {
            get { return _selectedParticipant; }
            set { SetProperty(ref _selectedParticipant, value); }
        }
        private bldParticipant _resivedParticipant;
        public bldParticipant ResivedParticipant
        {
            get { return _resivedParticipant; }
            set { SetProperty(ref _resivedParticipant, value); }
        }

        private bldResponsibleEmployee _selectedResponsibleEmployee;
        public bldResponsibleEmployee SelectedResponsibleEmployee
        {
            get { return _selectedResponsibleEmployee; }
            set { SetProperty(ref _selectedResponsibleEmployee, value); }
        }
        private bldResponsibleEmployeesGroup _responsibleEmployees;
        public bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }
        private bldConstructionCompany _selectedConstructionCompany;
        public bldConstructionCompany SelectedConstructionCompany
        {
            get { return _selectedConstructionCompany; }
            set { SetProperty(ref _selectedConstructionCompany, value); }
        }
        private string _messageReceived = "Бла бал бла...!!!";
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { SetProperty(ref _messageReceived, value); }

        }
        private ObservableCollection<bldParticipantRole> _allParticipantRoles;
        public ObservableCollection<bldParticipantRole> AllParticipantRoles
        {
            get { return _allParticipantRoles; }
            set { SetProperty(ref _allParticipantRoles, value); }
        }
        private ObservableCollection<bldConstructionCompany> _allConstructionCompanies;
        public ObservableCollection<bldConstructionCompany> AllConstructionCompanies
        {
            get { return _allConstructionCompanies; }
            set { SetProperty(ref _allConstructionCompanies, value); }
        }
        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; private set; }
        public NotifyCommand<object> CloseCommand { get; private set; }
        public NotifyCommand AddConstructionCompanyCommand { get; private set; }
        public NotifyCommand EditConstructionCompanyCommand { get; private set; }
        public NotifyCommand RemoveConstructionCompanyCommand { get; private set; }
        public NotifyCommand EditResponsibleEmployeeCommand { get; private set; }
        public NotifyCommand RemoveResponsibleEmployeeCommand { get; private set; }
        public NotifyCommand AddResponsibleEmployeesCommand { get; private set; }
        public NotifyCommand<bldCompany> CompanyChangedCommand { get; private set; }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public ParticipantViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository,
            IApplicationCommands applicationCommands, IUnDoReDoSystem unDoReDo)
        {
            UnDoReDo = new UnDoReDoSystem();

            SaveCommand = new NotifyCommand(OnSave, CanSave).ObservesProperty(() => SelectedParticipant);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                          () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            AddResponsibleEmployeesCommand = new NotifyCommand(OnAddResponsibleEmployees);
            RemoveResponsibleEmployeeCommand = new NotifyCommand(OnRemoveResponsibleEmployee,
                                                 () => SelectedResponsibleEmployee != null)
                             .ObservesProperty(() => SelectedResponsibleEmployee);
            EditResponsibleEmployeeCommand = new NotifyCommand(OnEditResponsibleEmployee,
                                                 () => SelectedResponsibleEmployee != null)
                             .ObservesProperty(() => SelectedResponsibleEmployee);
            CompanyChangedCommand = new NotifyCommand<bldCompany>(OnCompanyChanged);

            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            _buildingUnitsRepository = buildingUnitsRepository;
            _applicationCommands = applicationCommands;
            _dialogService = dialogService;
            _regionManager = regionManager;
            AllParticipantRoles = new ObservableCollection<bldParticipantRole>(buildingUnitsRepository.ParticipantRolesRepository.GetAllAsync());
            AllConstructionCompanies = new ObservableCollection<bldConstructionCompany>(buildingUnitsRepository.ConstructionCompanies.GetAll());
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

        }

        private void OnCompanyChanged(bldCompany company)
        {
            if (SelectedParticipant.ResponsibleEmployees.Count > 0)
                SelectedParticipant.CleareResponsibleEmployees();
        }

        private void OnAddResponsibleEmployees()
        {
            bldResponsibleEmployeesGroup All_ResponsibleEmployees = new bldResponsibleEmployeesGroup(
                    _buildingUnitsRepository.ResponsibleEmployees.GetAllResponsibleEmployees()
                    .Where(re => re.Employee?.Company.Id == SelectedParticipant.ConstructionCompany.Id &&
                     !SelectedParticipant.ResponsibleEmployees.Contains(re)).ToList());
            NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> predicate_1 = new NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> predicate_2 = new NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> predicate_3 = new NameablePredicate<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            predicate_1.Name = "Показать только из текущего проекта.";
            predicate_1.Predicate = cl => cl.Where(el => el?.bldParticipant?.bldProject != null &&
                                                       el?.bldParticipant?.bldProject.Id == SelectedParticipant?.bldProject?.Id).ToList();
            predicate_2.Name = "Показать из всех кроме текущего проекта";
            predicate_2.Predicate = cl => cl.Where(el => el?.bldParticipant?.bldProject != null &&
                                                       el?.bldParticipant?.bldProject.Id != SelectedParticipant?.bldProject?.Id).ToList();
            predicate_3.Name = "Показать все";
            predicate_3.Predicate = cl => cl;
            NameablePredicateObservableCollection<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee> nameablePredicatesCollection = new NameablePredicateObservableCollection<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>();
            nameablePredicatesCollection.Add(predicate_1);
            nameablePredicatesCollection.Add(predicate_2);
            nameablePredicatesCollection.Add(predicate_3);

            ObservableCollection<bldResponsibleEmployee> collection_for_add = new ObservableCollection<bldResponsibleEmployee>();
            CoreFunctions.AddElementToCollectionWhithDialog_Test<ObservableCollection<bldResponsibleEmployee>, bldResponsibleEmployee>
                 (collection_for_add, All_ResponsibleEmployees,
                 nameablePredicatesCollection,
                 _dialogService,
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SaveCommand.RaiseCanExecuteChanged();
                         foreach (bldResponsibleEmployee employee in collection_for_add)
                         {
                             SelectedParticipant.AddResponsibleEmployee(employee);

                         }
                     }
                 },
                 typeof(AddbldResponsibleEmployeeToCollectionDialogView).Name,
                 typeof(ResponsibleEmployeeDialogView).Name, Id,
                  "Редактирование списка отвественных работников",
                 "Форма для редактирования отвественных.",
                 "Ответсвенные текущего проекта", "Все отвественные лица");
        }
        private void OnRemoveResponsibleEmployee()
        {
            CoreFunctions.RemoveElementFromCollectionWhithDialog<bldResponsibleEmployeesGroup, bldResponsibleEmployee>
                 (SelectedResponsibleEmployee, "Ответсвенный представитель",
                 (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         SelectedParticipant.RemoveResponsibleEmployee(SelectedResponsibleEmployee);
                         SelectedResponsibleEmployee = null;
                         SaveCommand.RaiseCanExecuteChanged();
                     }
                 }, _dialogService, Id);

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
        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedResponsibleEmployee)
            {
                //    SelectedResponsibleEmployee = null;
                return;
            }
            if (obj == SelectedConstructionCompany)
            {
                SelectedConstructionCompany = null;
                return;
            }

        }

        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            if (SelectedParticipant != null)
                return !SelectedParticipant.HasErrors;
            else
                return false;
        }
        public override void OnSave()
        {
            base.OnSave<bldParticipant>(SelectedParticipant);
        }
        public override void OnClose(object obj)
        {
            base.OnClose<bldParticipant>(obj, SelectedParticipant);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_participant"];

            if (((bldParticipant)navigane_message.Object).Id != SelectedParticipant.Id)
                return false;
            else
                return true;

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigate_message = (ConveyanceObject)navigationContext.Parameters["bld_participant"];
            if (navigate_message != null)
            {
                ResivedParticipant = (bldParticipant)navigate_message.Object;
                EditMode = navigate_message.EditMode;
                if (SelectedParticipant != null) SelectedParticipant.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedParticipant = ResivedParticipant;
                SelectedParticipant.ErrorsChanged += RaiseCanExecuteChanged;
                UnDoReDo.Register(SelectedParticipant);

            }
            Title = $"{SelectedParticipant.Code} {SelectedParticipant.Name}";


        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
