using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public class UnDoReDoSystem : IUnDoReDoSystem
    {
        private Stack<IUnDoRedoCommand> _UnDoCommands = new Stack<IUnDoRedoCommand>();
        private Stack<IUnDoRedoCommand> _ReDoCommands = new Stack<IUnDoRedoCommand>();
        private ObservableCollection<IJornalable> _RegistedModels = new ObservableCollection<IJornalable>();

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void ReDo(int levels)
        {
            for (int ii = 0; ii < levels; ii++)
            {
                if (_ReDoCommands.Count > 0)
                {
                    IUnDoRedoCommand command = _ReDoCommands.Pop();
                    command.Execute();
                    _UnDoCommands.Push(command);
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReDo"));
        }
        public void UnDoAll()
        {
            while (_UnDoCommands.Count > 0)
            {
                UnDo(1);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDoAll"));
        }
        public void UnDo(int levels)
        {
            for (int ii = 0; ii < levels; ii++)
            {
                if (_UnDoCommands.Count > 0)
                {
                    IUnDoRedoCommand command = _UnDoCommands.Pop();
                    command.UnExecute();
                    _ReDoCommands.Push(command);
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDo"));
        }
        public bool AllUnDoIsDone()
        {
            return _UnDoCommands.Count == 0;
        }
        public void ClearStacks()
        {
            _UnDoCommands.Clear();
            _ReDoCommands.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClearStacks"));
        }
        public bool CanUnDoExecute()
        {
            return _UnDoCommands.Count > 0;
        }
        public bool CanReDoExecute()
        {
            return _ReDoCommands.Count > 0;
        }
        public void Register(IJornalable obj)
        {
            if (_RegistedModels.Contains(obj))
                return;
            else
                _RegistedModels.Add(obj);
            obj.PropertyBeforeChanged += OnModelPropertyBeforeChanged;
            obj.UnDoReDoCommandCreated += OnObservedCommandCreated;
        }
        public void UnRegister(IJornalable obj)
        {
            if (_RegistedModels.Contains(obj))
            {
                obj.PropertyBeforeChanged -= OnModelPropertyBeforeChanged;
                obj.UnDoReDoCommandCreated -= OnObservedCommandCreated;
                _RegistedModels.Remove(obj);
            }
        }
        private void OnObservedCommandCreated(object sender, UnDoReDoCommandCreateEventsArgs e)
        {
            _UnDoCommands.Push(e.Command);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnCommandCreated"));
        }
        private void OnModelPropertyBeforeChanged(object sender, PropertyBeforeChangeEvantArgs e)
        {
            PropertySetCommand setCommand =
                new PropertySetCommand(sender as IJornalable, e.PropertyName, e.NewValue, e.LastValue);
            _UnDoCommands.Push(setCommand);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnModelPropertyBeforeChanged"));
        }
        public UnDoReDoSystem()
        {

        }

    }
}
