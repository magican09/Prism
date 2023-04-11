using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class UnDoReDoCompositeCommand : UnDoRedoCommandBase, IUnDoRedoCommand, IUnDoReDoCompositeCommand
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private Stack<IUnDoRedoCommand> _Commands = new Stack<IUnDoRedoCommand>();
        private Stack<IUnDoRedoCommand> _ExecutedCommands = new Stack<IUnDoRedoCommand>();
        private Stack<IUnDoRedoCommand> _UnExecutedCommands = new Stack<IUnDoRedoCommand>();

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _UnExecutedCommands.Count > 0;
        }

        public void Execute(object parameter = null)
        {
            while (_UnExecutedCommands.Count > 0)
            {
                _ExecutedCommands.Push(_UnExecutedCommands.Pop());
            }
        }

        public void UnExecute()
        {
            while (_ExecutedCommands.Count > 0)
            {
                _UnExecutedCommands.Push(_ExecutedCommands.Pop());
            }
        }
        public UnDoReDoCompositeCommand(IUnDoReDoSystem unDoReDoSystem)
        {
            UnDoReDo_System = unDoReDoSystem;
        }
        public void Add(IUnDoRedoCommand command)
        {
            _UnExecutedCommands.Push(command);
        }
    }
}
