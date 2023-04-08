using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using System;

namespace PrismWorkApp.Core.Dialogs
{
    public class InputTextValueDialogViewModel : BindableBase, IDialogAware
    {
        private string _title = "Диалоговое окно ввода";
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

        private string _inputText;
        public string InputText
        {
            get { return _refuse; }
            set { SetProperty(ref _refuse, value); }
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
        private string _cancel;
        public string Cancel
        {
            get { return _cancel; }
            set { SetProperty(ref _cancel, value); }
        }
        public event Action<IDialogResult> RequestClose;
        public NotifyCommand CloseDialogCommand { get; private set; }
        public NotifyCommand ConfirmDialogCommand { get; private set; }
        public NotifyCommand CancelDialogCommand { get; private set; }

        public InputTextValueDialogViewModel()
        {
            CloseDialogCommand = new NotifyCommand(CloseDialog);
            ConfirmDialogCommand = new NotifyCommand(ConfirmDialog);
            CancelDialogCommand = new NotifyCommand(CancelDialog);

        }

        private void ConfirmDialog()
        {
            var result = ButtonResult.Yes;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Подтверждено пользователем!");
            param.Add("input_text", InputText);
            RequestClose.Invoke(new DialogResult(result, param));
        }

        private void CloseDialog()
        {
            var result = ButtonResult.No;
            var param = new DialogParameters();
            param.Add("confirm_dialog_param", "Отменено пользователем!");
            RequestClose.Invoke(new DialogResult(result, param));
        }
        private void CancelDialog()
        {
            var result = ButtonResult.Cancel;
            var param = new DialogParameters();
            param.Add("cancel_dialog_param", "Отменено пользователем!");
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
            Message = parameters.GetValue<string>("massege");
            InputText = parameters.GetValue<string>("input_text");

        }
    }
}
