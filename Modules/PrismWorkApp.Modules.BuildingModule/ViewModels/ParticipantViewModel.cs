using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.ProjectModel.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ParticipantViewModel : LocalBindableBase, INotifyPropertyChanged, INavigationAware
    {
       
       
       
        private string _title = "Учасник строительства";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private SimpleEditableBldParticipant _selectedParticipant;
        public SimpleEditableBldParticipant SelectedParticipant
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
        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }
        private bldParticipantsGroup _participants;
        public bldParticipantsGroup Participants
        {
            get { return _participants; }
            set { SetProperty(ref _participants, value); }
        }
        private bldParticipantsGroup _allParticipants;
        public bldParticipantsGroup AllParticipants
        {
            get { return _allParticipants; }
            set { SetProperty(ref _allParticipants, value); }
        }
        protected readonly IDialogService _dialogService;
         private string _messageReceived="Бла бал бла...!!!";
        public string MessageReceived
        {
            get { return _messageReceived; }
            set { SetProperty(ref _messageReceived, value);  }
             
        }

        public DelegateCommand ShowMessageDialogCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public ParticipantViewModel(IDialogService dialogService  )
        {
            ShowMessageDialogCommand = new DelegateCommand(ShowDialog);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
           _dialogService = dialogService;
          
        }

        private bool CanSave()
        {
            return true;
        }

        virtual protected void OnSave()
        {
            if (EditMode == ConveyanceObjectModes.EditMode.FOR_EDIT)
            {

                CoreFunctions.ConfirmActionOnElementDialog<bldParticipant>(SelectedParticipant, "Сохранить", "участник строительства",
                    "Сохранить" ,
                    "Не сохранять",
                    "Отмена", (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                 //       CoreFunctions.CopyObjectReflectionNewInstances(SelectedParticipant, ResivedParticipant);
                   
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
            var p  = new DialogParameters();
            p .Add("message", "Это тестовое сообщение диалоговому окну!");

            _dialogService.ShowDialog("MessageDialog", p , result=>
            {
                if(result.Result ==ButtonResult.OK)
                {
                   MessageReceived = result.Parameters.GetValue<string>("my_param");
                }
            });
        }

      

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ConveyanceObject navigate_message  = (ConveyanceObject)navigationContext.Parameters["participant"];
           if(navigate_message !=null)
            {
                ResivedParticipant =(bldParticipant) navigate_message.Object;
                EditMode = navigate_message.EditMode;
                if (SelectedParticipant != null) SelectedParticipant.ErrorsChanged -= RaiseCanExecuteChanged;
                SelectedParticipant = new SimpleEditableBldParticipant();
                SelectedParticipant.ErrorsChanged += RaiseCanExecuteChanged;
              //  CoreFunctions.CopyObjectReflectionNewInstances(ResivedParticipant, SelectedParticipant);

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
