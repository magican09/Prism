using Prism;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
        public ObservableCollection<bldParticipant> SelectedbldParticipantsList
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

        public ObservableCollection<INotifyCommand> ParticipantsContextMenuCommands { get; set; } = new ObservableCollection<INotifyCommand>();
        public NotifyCommand CreateNewParticipantCommand { get; private set; }
        public NotifyCommand ChangeParticipantsListCommand { get; private set; }
        public NotifyCommand AddParticipantsListCommand { get; private set; }
        public NotifyCommand<object> AddCreatedFromTemplateParticipantCommand { get; private set; }
        public NotifyCommand RemoveParticipantCommand { get; private set; }


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

            CreateNewParticipantCommand = new NotifyCommand(OnCreateNewParticipant);
            CreateNewParticipantCommand.Name = "Создать нового участника";
            ChangeParticipantsListCommand = new NotifyCommand(OnChangeParticipantsListCommand);
            ChangeParticipantsListCommand.Name = "Изменить состав учасников";
            AddParticipantsListCommand = new NotifyCommand(OnAddParticipantsListCommand);
            AddParticipantsListCommand.Name = "Добавить участников";

            ParticipantsContextMenuCommands.Add(CreateNewParticipantCommand);
            ParticipantsContextMenuCommands.Add(ChangeParticipantsListCommand);
            ParticipantsContextMenuCommands.Add(AddParticipantsListCommand);


            #endregion

            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            _regionManager = regionManager;
            IsActiveChanged += OnActiveChanged;
        }

        private void OnAddParticipantsListCommand()
        {
            bldConstructionCompanyGroup All_companies = new bldConstructionCompanyGroup(_buildingUnitsRepository.ConstructionCompanies.GetAllAsync());

            NameablePredicate<bldConstructionCompanyGroup, bldConstructionCompany> predicate_1 = new NameablePredicate<bldConstructionCompanyGroup, bldConstructionCompany>();
            predicate_1.Name = "Показать все компании";
            predicate_1.Predicate = cl => cl;
            predicate_1.CollectionSelectPredicate = col =>
            {
                ObservableCollection<NameableObjectPointer> out_coll = new ObservableCollection<NameableObjectPointer>();
                foreach (bldConstructionCompany company in col)
                {
                    NameableObjectPointer objectPointer = new NameableObjectPointer();
                    objectPointer.Code = company.Code;
                    objectPointer.Name = company.Name;
                    objectPointer.Annotation = $"{company.INN}, {company.Address}";
                    objectPointer.ObjectPointer = company;
                    out_coll.Add(objectPointer);
                }
                return out_coll;
            };
            NameablePredicateObservableCollection<bldConstructionCompanyGroup, bldConstructionCompany> nameablePredicatesCollection = new NameablePredicateObservableCollection<bldConstructionCompanyGroup, bldConstructionCompany>();
            nameablePredicatesCollection.Add(predicate_1);
            bldConstructionCompanyGroup empl_for_add_collection = new bldConstructionCompanyGroup();

            CoreFunctions.AddElementsToCollectionWhithDialogList<bldConstructionCompanyGroup, bldConstructionCompany>
                (empl_for_add_collection, All_companies,
               nameablePredicatesCollection,
              _dialogService,
               (result) =>
               {
                   if (result.Result == ButtonResult.Yes)
                   {
                       //UnDoReDoSystem localUnDoReDo = new UnDoReDoSystem();
                       //localUnDoReDo.Register(SelectedProject);
                       //UnDoReDo.UnRegister(SelectedWork);
                       foreach (bldConstructionCompany company in empl_for_add_collection)
                       {
                           bldParticipant participant = new bldParticipant();
                           participant.ConstructionCompany = company;
                           bldParticipantRole role = _buildingUnitsRepository.ParticipantRolesRepository.GetAllAsync().Where(rl => rl.RoleCode == ParticipantRole.NONE).FirstOrDefault();
                           if (role == null)
                           {
                               role = new bldParticipantRole(ParticipantRole.NONE);
                               role.Name = "Не определено";
                               role.FullName = "Не определено";
                               _buildingUnitsRepository.ParticipantRolesRepository.Add(role);

                           }
                           participant.Role = role;
                           UnDoReDo.Register(participant);
                           SelectedProject.AddParticipant(participant);
                       }
                       SaveCommand.RaiseCanExecuteChanged();
                       //UnDoReDo.AddUnDoReDoSysAsCommand(localUnDoReDo);
                       //UnDoReDo.Register(SelectedWork);
                   }
                   if (result.Result == ButtonResult.No)
                   {
                   }
               },
              typeof(AddConstructionCompaniesToCollectionFromListDialogView).Name,
               "Добавить компанию в качестве участника",
               "Форма добавления участников проекта.",
               "Список компаний", "");
        }

        private void OnChangeParticipantsListCommand()
        {

        }
        #region Commands methods


        private void OnCreateNewParticipant()
        {

            //   
        }

        #endregion

        #region Window operate methods
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
        public override void OnWindowClose()
        {
            _applicationCommands.SaveAllCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.ReDoCommand.UnregisterCommand(ReDoCommand);
            _applicationCommands.UnDoCommand.UnregisterCommand(UnDoCommand);
        }
        #endregion


        #region  INavigationAware realization
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            ConveyanceObject navigane_message = (ConveyanceObject)navigationContext.Parameters["bld_project"];
            if (((bldProject)navigane_message.Object).Id != SelectedProject.Id)
            {

                return false;
            }
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

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
                foreach (bldParticipant participant in SelectedProject.Participants)
                    UnDoReDo.Register(participant);
                Title = $"{SelectedProject.Code} {SelectedProject.ShortName}";


            }
        }
        #endregion

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
