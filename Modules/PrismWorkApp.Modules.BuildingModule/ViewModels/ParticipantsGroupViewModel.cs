using Prism;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ParticipantsGroupViewModel : BaseViewModel<bldParticipantsGroup>, INotifyPropertyChanged, INavigationAware, IActiveAware
    {
        private string _title = "Участники проекта";
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
        private bldProject _selectedProject;
        public bldProject SelectedProject
        {
            get { return _selectedProject; }
            set { SetProperty(ref _selectedProject, value); }
        }
        private bldParticipantsGroup _selectedParticipantsGroup;
        public bldParticipantsGroup SelectedParticipantsGroup
        {
            get { return _selectedParticipantsGroup; }
            set { SetProperty(ref _selectedParticipantsGroup, value); }
        }

        private ObservableCollection<bldParticipant> _selectedbldParticipantsList = new ObservableCollection<bldParticipant>();
        public ObservableCollection<bldParticipant>  SelectedbldParticipantsList
        {
            get { return _selectedbldParticipantsList; }
            set { SetProperty(ref _selectedbldParticipantsList, value); }
        }

        public IBuildingUnitsRepository _buildingUnitsRepository { get; }
        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand<object> DataGridSelectionChangedCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; protected set; }
        public NotifyCommand ReDoCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        public NotifyCommand RemoveParticipantCommand { get; private set; }
        public NotifyCommand AddParticipantCommand { get; private set; }

        public ParticipantsGroupViewModel(IDialogService dialogService,
            IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands)
        {

            UnDoReDo = new UnDoReDoSystem();
     
            SaveCommand = new NotifyCommand(OnSave, CanSave)
                .ObservesProperty(() => SelectedProject);
            CloseCommand = new NotifyCommand<object>(OnClose);
            UnDoCommand = new NotifyCommand(() => { UnDoReDo.UnDo(1); },
                                     () => { return UnDoReDo.CanUnDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);
            ReDoCommand = new NotifyCommand(() => UnDoReDo.ReDo(1),
                                     () => { return UnDoReDo.CanReDoExecute(); }).ObservesPropertyChangedEvent(UnDoReDo);

            #region Commands Init
            _applicationCommands = applicationCommands;


            DataGridSelectionChangedCommand = new NotifyCommand<object>(OnDataGridSelectionChanged);
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostFocus);

            #endregion

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            IsActiveChanged += OnActiveChanged;
        }
       

        public void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private bool CanSave()
        {
            if (SelectedProject != null)
                return !SelectedProject.HasErrors;// && SelectedWork.UnDoReDoSystem.Count > 0;
            else
                return false;
        }
        
        public virtual void OnSave()
        {
            base.OnSave<bldProject>(SelectedProject);
        }
        public virtual void OnClose(object obj)
        {
            base.OnClose<bldProject>(obj, SelectedProject);
        }
        #region  INavigationAware realization

        #endregion

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message_works = (ConveyanceObject)navigationContext.Parameters["bld_project"];
            if (navigane_message_works != null)
            {
                EditMode = navigane_message_works.EditMode;
                if (SelectedProject != null) SelectedProject.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedProject = (bldProject)navigane_message_works.Object;
                SelectedProject.ErrorsChanged += RaiseCanExecuteChanged;
                SelectedParticipantsGroup = SelectedProject.Participants;
                UnDoReDo.Register(SelectedProject);
                foreach (bldParticipant  participant in SelectedProject.Participants)
                    UnDoReDo.Register(participant);
                Title = $"{SelectedProject.Code} {SelectedProject.ShortName}";


            }
        }
        #region DataGrid events
        private void OnDataGridSelectionChanged(object participants)
        {
            SelectedbldParticipantsList.Clear();
            foreach (bldParticipant participant in (IList)participants)
                SelectedbldParticipantsList.Add(participant);
        }
        private void OnDataGridLostFocus(object obj)
        {
            SelectedbldParticipantsList.Clear();
        }
        #endregion
        #region on Activate event  
        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
                RegisterAplicationCommands();
            else
                UnRegisterAplicationCommands();

        }
        private void RegisterAplicationCommands()
        {
            _applicationCommands.SaveAllCommand.RegisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.RegisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.RegisterCommand(UnDoCommand);
         }
        private void UnRegisterAplicationCommands()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
            
        }
        #endregion
    }
}
