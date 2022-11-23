using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core.Dialogs
{
    public class ConfirmActionWhithoutCancelDialogViewModel : BindableBase, IDialogAware
    {
        private string _title = "Диалоговое окно сообщения";
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
        private string _cancel;
        public string Cancel
        {
            get { return _cancel; }
            set { SetProperty(ref _cancel, value); }
        }
        public event Action<IDialogResult> RequestClose;
        public NotifyCommand  CloseDialogCommand { get; private set; }
        public NotifyCommand ConfirmDialogCommand { get; private set; }
        public NotifyCommand CancelDialogCommand { get; private set; }

        public ConfirmActionWhithoutCancelDialogViewModel()
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
            Refuse = parameters.GetValue<string>("confirm_button_content");
            Confirm = parameters.GetValue<string>("refuse_button_content");
            Cancel = parameters.GetValue<string>("cancel_button_content");
        }
    }
}
