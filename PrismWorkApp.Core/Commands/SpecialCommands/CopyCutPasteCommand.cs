using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public class CopyCutPasteCommands<T> 
    {
        public NotifyCommand<T> CopyCommand { get; private set; }
        public NotifyCommand<T> CutCommand { get; private set; }
        public NotifyCommand<T> PasteCommand { get; private set; }
        public ObservableCollection<T> Buffer { get; set; }
        public CopyCutPasteCommands(Action<T> copyExecuteMethod, Action<T>cutExecuteMethod, Action<T> pasteExecuteMethod)
        {
            CopyCommand = new NotifyCommand<T>(copyExecuteMethod);
            CutCommand = new NotifyCommand<T>(cutExecuteMethod);
            PasteCommand = new NotifyCommand<T>(pasteExecuteMethod);

        }

       
    }
}
