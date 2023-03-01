using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismWorkApp.Core.Commands;
using System;

namespace PrismWorkApp.Core.Dialogs
{
    public class MessageDialogViewModel : BindableBase, IDialogAware
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
        public event Action<IDialogResult> RequestClose;

        public MessageDialogViewModel()
        {
            CloseDialogCommand = new NotifyCommand(CloseDialog);
        }

        private void CloseDialog()
        {
            var result = ButtonResult.OK;
            var p = new DialogParameters();
            p.Add("my_param", "Диалоговое окно закрыто пользователем!");
            RequestClose.Invoke(new DialogResult(result, p));
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
            Message = parameters.GetValue<string>("message");
        }

        public NotifyCommand CloseDialogCommand { get; }

    }
}
