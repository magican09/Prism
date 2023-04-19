using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using System;
using System.Collections;

namespace PrismWorkApp.Core.Dialogs
{
    public class SelectElementFromCollectionDialogVeiwModel<TContainer, T> : BindableBase, IDialogAware
        where TContainer : ICollection, IList, new()
        where T : OpenWorkLib.Data.IEntityObject
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
        public NotifyCommand CloseDialogCommand { get; private set; }
        public NotifyCommand ConfirmDialogCommand { get; private set; }
        public NotifyCommand CreateNewCommand { get; private set; }
        public SelectElementFromCollectionDialogVeiwModel()
        {
            CloseDialogCommand = new NotifyCommand(CloseDialog);
            ConfirmDialogCommand = new NotifyCommand(ConfirmDialog, CanConfirmDialog).
                ObservesProperty(() => SelectedElement);
            CreateNewCommand = new NotifyCommand(OnCreateNew);

        }

        private void OnCreateNew()
        {
           // SelectedElement = new T();
            CurrentCollection.Add(SelectedElement);
            //var result = ButtonResult.Yes;
            //var param = new DialogParameters();
            //param.Add("confirm_dialog_param", "Создан новый элемент");
            //param.Add("element", SelectedElement);
            //RequestClose.Invoke(new DialogResult(result, param));
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

