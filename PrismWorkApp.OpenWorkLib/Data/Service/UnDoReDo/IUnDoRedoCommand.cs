using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoRedoCommand : ICommand,IKeyable,INameable
    {
         ObservableCollection<IJornalable> ChangedObjects { get; set; }
        /// <summary>
        /// Система UnDoReDoSystem в которой была зарегистрирована данная команда
        /// </summary>
         IUnDoReDoSystem UnDoReDo_System { get;  }
        void Execute(object parameter = null);
        bool CanExecute(object parameter=null);
         void UnExecute();
         DateTime Date { get; set; }
         int Index { get; set; }
    }
}
