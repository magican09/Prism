using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public class ObservedCommand<TEntity> : IObservedCommand
    {
        //    public event ObservedCommandExecuteEvenHandler ObservedCommandExecuted;
        public UnDoReDoSystem UnDoReDoSystem;
        private Action<TEntity> _ExecuteAction;
        //    private Action<TEntity> _UnExecuteAction;
        private Func<bool> _canExecuteAction;
        private Action<bldObject> onRemoveBuildindObject_Execute;
        private Action<bldObject> onRemoveBuildindObject_UnExecute;
        private Func<bool> onRemoveBuildindObject_CanExecute;

        public IUnDoRedoCommand Command { get; set; }

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

        //public void UnExecute(object parameter = null)
        //{
        //    _UnExecuteAction((TEntity)parameter);
        //    ObservedCommandCreated?.Invoke(this, new ObservedCommandExecuteEventsArgs(Command, "ObservedCommand"));
        //}
        public ObservedCommand(Action<TEntity> execute, Func<bool> canExecute = null)
        {
            _ExecuteAction = execute;
            //  _UnExecuteAction = unExecute;
            _canExecuteAction = canExecute;
        }
        public void SendCommandToUndoRedoSystem(IUnDoRedoCommand command)
        {
            Command = command;
            //     ObservedCommandExecuted?.Invoke(this, new ObservedCommandExecuteEventsArgs(command));
        }
    }
}
