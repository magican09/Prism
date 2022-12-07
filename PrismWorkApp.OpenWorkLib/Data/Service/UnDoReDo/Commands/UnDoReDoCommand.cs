using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class UnDoReDoCommand<TEntity> : IUnDoRedoCommand
    {
        //  public UnDoReDoSystem UnDoReDoSystem;
        private Action<TEntity> _ExecuteAction;
        private Action _UnExecuteAction;
        private Func<bool> _canExecuteAction;
        public string Name { get; set; }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            if (_canExecuteAction != null)
                return _canExecuteAction();
            else
                return true;
        }
        public void Execute(object parameter = null)
        {
            _ExecuteAction((TEntity)parameter);
        }

        public void UnExecute()
        {
            _UnExecuteAction();
        }

        public UnDoReDoCommand(Action<TEntity> execute, Action unExecute, Func<bool> canExecute = null)
        {
            _ExecuteAction = execute;
            _UnExecuteAction = unExecute;
            _canExecuteAction = canExecute;
        }

    }
}
