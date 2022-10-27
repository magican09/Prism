using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.ProjectModel.Data.Models;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;

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
        private string _messageReceived="Бла бал бла...!!!";
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { SetProperty(ref _messageReceived, value);  }
             
        }

        public DelegateCommand<object> DataGridLostFocusCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }
        public DelegateCommand AddConstructionCompanyCommand { get; private set; }
        public DelegateCommand EditConstructionCompanyCommand { get; private set; }
        public DelegateCommand RemoveConstructionCompanyCommand { get; private set; }
        
        public DelegateCommand AddEmployerCommand { get; private set; }
        public DelegateCommand EditEmployerCommand { get; private set; }
        public DelegateCommand RemoveEmployerCommand { get; private set; }

        public ParticipantViewModel(IDialogService dialogService,IRegionManager regionManager,IBuildingUnitsRepository buildingUnitsRepository )
        {
            DataGridLostFocusCommand = new DelegateCommand<object>(OnDataGridLostSocus);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CloseCommand = new DelegateCommand<object>(OnClose);
            SaveCommand = new DelegateCommand(OnSave, CanSave);

            EditConstructionCompanyCommand = new DelegateCommand(OnEditConstructionCompany, () => SelectedConstructionCompany != null);


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
            return true;
        }
       public override void OnSave()
        {
            this.OnSave<bldParticipant>(SelectedParticipant);
        }
        public  override void OnClose(object obj)
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
            ConveyanceObject navigate_message  = (ConveyanceObject)navigationContext.Parameters["bld_participant"];
           if(navigate_message !=null)
            {
                ResivedParticipant =(bldParticipant) navigate_message.Object;
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
