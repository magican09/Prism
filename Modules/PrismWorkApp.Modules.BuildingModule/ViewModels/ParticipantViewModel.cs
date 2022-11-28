using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.ComponentModel;

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

        public NotifyCommand<object> DataGridLostFocusCommand { get; private set; }
        public NotifyCommand SaveCommand { get; private set; }
        public NotifyCommand<object> CloseCommand { get; private set; }
        public NotifyCommand AddConstructionCompanyCommand { get; private set; }
        public NotifyCommand EditConstructionCompanyCommand { get; private set; }
        public NotifyCommand RemoveConstructionCompanyCommand { get; private set; }

        public NotifyCommand AddEmployerCommand { get; private set; }
        public NotifyCommand EditEmployerCommand { get; private set; }
        public NotifyCommand RemoveEmployerCommand { get; private set; }

        public ParticipantViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
        {
            DataGridLostFocusCommand = new NotifyCommand<object>(OnDataGridLostSocus);
            SaveCommand = new NotifyCommand(OnSave, CanSave);
            CloseCommand = new NotifyCommand<object>(OnClose);
            SaveCommand = new NotifyCommand(OnSave, CanSave);

            EditConstructionCompanyCommand = new NotifyCommand(OnEditConstructionCompany, () => SelectedConstructionCompany != null);


            _dialogService = dialogService;
            _regionManager = regionManager;

        }

        private void OnEditConstructionCompany()
        {
            // CoreFunctions.EditElementDialog<bldConstructionCompany>(SelectedConstructionCompany, "Участник строительства",
            //    (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ObjectDialogView).Name, "Редактировать", Id);
        }

        private void OnDataGridLostSocus(object obj)
        {

            if (obj == SelectedResponsibleEmployee)
            {
                SelectedResponsibleEmployee = null;
                return;
            }
            if (obj == SelectedConstructionCompany)
            {
                SelectedConstructionCompany = null;
                return;
            }

        }
        /*
                private void OnEditBuildingObject()
                {
                    CoreFunctions.EditElementDialog<bldObject>(SelectedBuildingObject, "Строительный объект",
                     (result) => { SaveCommand.RaiseCanExecuteChanged(); }, _dialogService, typeof(ObjectDialogView).Name, "Редактировать", Id);
                }
                private void OnAddBuildingObject()
                {
                    BuildingObjects = new bldObjectsGroup(_buildingUnitsRepository.Objects.GetldObjectsAsync());//.GetBldObjects(SelectedProject.Id));
                    if (SelectedBuildingObject.BuildingObjects == null) SelectedBuildingObject.BuildingObjects = new bldObjectsGroup();
                    CoreFunctions.AddElementToCollectionWhithDialog<bldObjectsGroup, bldObject>
                          (SelectedBuildingObject.BuildingObjects, BuildingObjects, _dialogService,
                            (result) =>
                            {

                                if (result.Result == ButtonResult.Yes)
                                {
                                    SaveCommand.RaiseCanExecuteChanged();
                                }
                                if (result.Result == ButtonResult.No)
                                {
                                    SelectedBuildingObject.BuildingObjects.UnDoAll(Id);
                                }
                            },
                           typeof(AddbldObjectToCollectionDialogView).Name,
                            typeof(ObjectDialogView).Name, Id,
                           "Редактирование списка объектов",
                           "Форма для редактирования состава объектов проекта.",
                          "Объекты текущего проекта", "Все объекты");

                }
                private void OnRemoveBuildingObject()
                {

                    CoreFunctions.RemoveElementFromCollectionWhithDialog<bldObjectsGroup, bldObject>
                       (SelectedBuildingObject.BuildingObjects, SelectedChildBuildingObject, "Строительный объект"
                       , () =>
                       {
                           SelectedChildBuildingObject = null;
                           SaveCommand.RaiseCanExecuteChanged();
                       }, _dialogService);
                }
                */
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
            this.OnSave<bldParticipant>(SelectedParticipant);
        }
        public override void OnClose(object obj)
        {
            this.OnClose<bldParticipant>(obj, SelectedParticipant);
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

            }


        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
