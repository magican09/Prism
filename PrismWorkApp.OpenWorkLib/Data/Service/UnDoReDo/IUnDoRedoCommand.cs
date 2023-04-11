using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoRedoCommand : ICommand,IKeyable,INameable
    {
        public ObservableCollection<IJornalable> ChangedObjects { get; set; }
        /// <summary>
        /// Система UnDoReDoSystem в которой была зарегистрирована данная команда
        /// </summary>
        public IUnDoReDoSystem UnDoReDo_System { get;  }
        void Execute(object parameter = null);
        bool CanExecute(object parameter=null);
        public void UnExecute();
        public DateTime Date { get; set; }
        public int Index { get; set; }
    }
}
