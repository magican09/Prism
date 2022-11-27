using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;

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

        private bldResponsibleEmployeesGroup _responsibleEmployees;
        public bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }
        private bldResponsibleEmployeesGroup _allResponsibleEmployees;
        public bldResponsibleEmployeesGroup AllResponsibleEmployees
        {
            get { return _allResponsibleEmployees; }
            set { SetProperty(ref _allResponsibleEmployees, value); }
        }

        private string _messageReceived = "Бла бал бла...!!!";
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { SetProperty(ref _messageReceived, value); }

        }

        public DelegateCommand<object> DataGridLostFocusCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }

        public ResponsibleEmployeeViewModel(IDialogService dialogService, IRegionManager regionManager, IBuildingUnitsRepository buildingUnitsRepository)
        {
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CloseCommand = new DelegateCommand<object>(OnClose);

            _dialogService = dialogService;

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

        private bool canExecuteMethod()
        {
            return false;
        }

        private void ShowDialog()
        {
            var p = new DialogParameters();
            p.Add("message", "Это тестовое сообщение диалоговому окну!");

            _dialogService.ShowDialog("MessageDialog", p, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    MessageReceived = result.Parameters.GetValue<string>("my_param");
                }
            });
        }

        public virtual void OnSave()
        {
            this.OnSave<bldResponsibleEmployee>(SelectedResposibleEmployee);
        }
        public virtual void OnClose(object obj)
        {
            this.OnClose<bldResponsibleEmployee>(obj, SelectedResposibleEmployee);
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
                //   CoreFunctions.CopyObjectReflectionNewInstances(ResivedResposibleEmployee, SelectedResposibleEmployee);

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
