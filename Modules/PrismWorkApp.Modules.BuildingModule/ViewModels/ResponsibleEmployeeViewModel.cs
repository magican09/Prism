using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ResponsibleEmployeeViewModel : BaseViewModel<bldResponsibleEmployee>, INavigationAware
    {
        private string _title = "Отвественный работник";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bldResponsibleEmployee _selectedResposibleEmployee;
        public bldResponsibleEmployee SelectedResposibleEmployee
        {
            get { return _selectedResposibleEmployee; }
            set { SetProperty(ref _selectedResposibleEmployee, value); }
        }
        private bldResponsibleEmployee _resivedResposibleEmployee;
        public bldResponsibleEmployee ResivedResposibleEmployee
        {
            get { return _resivedResposibleEmployee; }
            set { SetProperty(ref _resivedResposibleEmployee, value); }
        }



        private ObservableCollection<bldResponsibleEmployeeRole> _allResponsibleEmployeesRoles;
        public ObservableCollection<bldResponsibleEmployeeRole> AllResponsibleEmployeesRoles
        {
            get { return _allResponsibleEmployeesRoles; }
            set { SetProperty(ref _allResponsibleEmployeesRoles, value); }
        }
        private ObservableCollection<bldParticipant> _allParticipants;
        public ObservableCollection<bldParticipant> AllParticipants
        {
            get { return _allParticipants; }
            set { SetProperty(ref _allParticipants, value); }
        }
        private bldParticipant _selectedParticipant;
        public bldParticipant SelectedParticipant
        {
            get { return _selectedParticipant; }
            set { SetProperty(ref _selectedParticipant, value); }
        }
        private string _messageReceived = "Бла бал бла...!!!";
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { SetProperty(ref _messageReceived, value); }

        }

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }

        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }


        public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        public ResponsibleEmployeeViewModel(IDialogService dialogService, IRegionManager regionManager,
            IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands,
            IUnDoReDoSystem unDoReDo)
        {
            UnDoReDo = new UnDoReDoSystem();

            SaveCommand = new NotifyCommand(OnSave, CanSave).ObservesProperty(() => SelectedResposibleEmployee);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                          () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
               () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);


            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);

            AllResponsibleEmployeesRoles = new ObservableCollection<bldResponsibleEmployeeRole>(
                _buildingUnitsRepository.ResponsibleEmployeeRoleRepository.GetAllResponsibleEmployeesRoles());
            AllParticipants = new ObservableCollection<bldParticipant>(
                    _buildingUnitsRepository.Pacticipants.GetAllParticipants());
        }



        private void OnDataGridLostSocus(object obj)
        {

            /*   if (obj == Sele)
               {
                   SelectedConstruction = null;
                   return;
               }
               if (obj == SelectedConstruction)
               {
                   SelectedChildBuildingObject = null;
                   return;
               }*/

        }

        private bool CanSave()
        {
            if (SelectedResposibleEmployee != null)
                return !SelectedResposibleEmployee.HasErrors;
            else
                return false;
        }
        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public virtual void OnSave()
        {
            base.OnSave<bldResponsibleEmployee>(SelectedResposibleEmployee);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldResponsibleEmployee>(obj, SelectedResposibleEmployee);
        }
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigate_message = (ConveyanceObject)navigationContext.Parameters["responsible_employee"];
            if (navigate_message != null)
            {
                ResivedResposibleEmployee = (bldResponsibleEmployee)navigate_message.Object;
                EditMode = navigate_message.EditMode;
                if (SelectedResposibleEmployee != null) SelectedResposibleEmployee.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedResposibleEmployee = new SimpleEditableResposibleEmployee();
                SelectedResposibleEmployee.ErrorsChanged += RaiseCanExecuteChanged;
                AllParticipants = new ObservableCollection<bldParticipant>(
                      _buildingUnitsRepository.Pacticipants.GetAllParticipants());
                UnDoReDo.Register(SelectedResposibleEmployee);
            }


        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
