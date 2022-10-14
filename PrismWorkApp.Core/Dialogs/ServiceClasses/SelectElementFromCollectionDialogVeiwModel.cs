using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core.Dialogs
{
    public class SelectElementFromCollectionDialogVeiwModel<TContainer,T>: BindableBase, IDialogAware
        where TContainer:ICollection<T>, new()
        where T:class, new()
    {
        private string _title = "Выбрать";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        private string _confirm;
        public string Confirm
        {
            get { return _confirm; }
            set { SetProperty(ref _confirm, value); }
        }
        private string _refuse;
        public string Refuse
        {
            get { return _refuse; }
            set { SetProperty(ref _refuse, value); }
        }
        private TContainer _currentCollection;

        public TContainer CurrentCollection
        {
            get { return _currentCollection; }
            set { SetProperty(ref _currentCollection, value); }
        }
        private T _selectedElement;
        public T SelectedElement
        {
            get { return _selectedElement; }
            set { SetProperty(ref _selectedElement, value); }
        }
        private T _resivedElement;
        public T ResivedElement
        {
            get { return _resivedElement; }
            set { SetProperty(ref _resivedElement, value); }
        }
        public event Action<IDialogResult> RequestClose;
        public DelegateCommand CloseDialogCommand { get; private set; }
        public DelegateCommand ConfirmDialogCommand { get; private set; }
        public SelectElementFromCollectionDialogVeiwModel()
        {
            CloseDialogCommand = new DelegateCommand(CloseDialog);
            ConfirmDialogCommand = new DelegateCommand(ConfirmDialog,CanConfirmDialog).
                ObservesProperty(()=>SelectedElement);
        }

        private bool CanConfirmDialog()
        {
            return SelectedElement != null;
        }

        private void ConfirmDialog()
        {
            var result = ButtonResult.Yes;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Подтверждено пользователем!");
            param.Add("element", SelectedElement);
            RequestClose.Invoke(new DialogResult(result, param));
        }

        private void CloseDialog()
        {
            var result = ButtonResult.No;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Отменено пользователем!");
            RequestClose.Invoke(new DialogResult(result, param));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
           
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
            CurrentCollection = parameters.GetValue<TContainer>("current_collection");
            Refuse = parameters.GetValue<string>("confirm_button_content");
            Confirm = parameters.GetValue<string>("refuse_button_content");
        }
    }
}
