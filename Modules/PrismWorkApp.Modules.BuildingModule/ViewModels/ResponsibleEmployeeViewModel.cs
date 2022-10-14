using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ResponsibleEmployeeViewModel : LocalBindableBase, INavigationAware
    {
        private string _title = "Отвественный работник";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private SimpleEditableResposibleEmployee _selectedResposibleEmployee;
        public SimpleEditableResposibleEmployee SelectedResposibleEmployee
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
        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
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
        protected readonly IDialogService _dialogService;
        private string _messageReceived = "Бла бал бла...!!!";
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { SetProperty(ref _messageReceived, value); }

        }

        public DelegateCommand ShowMessageDialogCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public ResponsibleEmployeeViewModel(IDialogService dialogService)
        {
            ShowMessageDialogCommand = new DelegateCommand(ShowDialog);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            _dialogService = dialogService;

        }

        private bool CanSave()
        {
            return true;
        }

        virtual  public  void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldResponsibleEmployee>(SelectedResposibleEmployee,
                    "Сохранить", "отвественный работник",
                    "Сохранить",
                    "не сохранять",
                    "Отмена", (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                //        CoreFunctions.CopyObjectReflectionNewInstances(SelectedResposibleEmployee, ResivedResposibleEmployee);
                    }
                    else
                    {

                    }

                }, _dialogService);

            }
            else
            {
                //  bldObject new_bldObject = new bldObject();
                //   CoreFunctions.CopyObjectReflectionNewInstances(SelectedObject, new_bldObject);
                //  Objects?.Add(new_bldObject);

            }

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
