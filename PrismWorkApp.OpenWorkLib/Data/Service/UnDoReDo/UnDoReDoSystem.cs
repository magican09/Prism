using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class UnDoReDoSystem : IUnDoReDoSystem, IUnDoRedoCommand
    {
        private Stack<IUnDoRedoCommand> _UnDoCommands = new Stack<IUnDoRedoCommand>();
        private int _UnDoCounter = 0;
        private Stack<IUnDoRedoCommand> _ReDoCommands = new Stack<IUnDoRedoCommand>();
        private ObservableCollection<IJornalable> _RegistedModels = new ObservableCollection<IJornalable>();

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event EventHandler CanExecuteChanged;

        public Guid Id { get; set; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool UnDo(int levels)
        {
            for (int ii = 0; ii < levels; ii++)
            {
                if (_UnDoCommands.Count > 0)
                {
                    IUnDoRedoCommand command = _UnDoCommands.Pop();
                    command.UnExecute();
                    _ReDoCommands.Push(command);
                    _UnDoCounter++;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDo"));
            return (_UnDoCommands.Count > 0) ? true : false;
        }
        public bool ReDo(int levels)
        {
            for (int ii = 0; ii < levels; ii++)
            {
                if (_ReDoCommands.Count > 0)
                {
                    IUnDoRedoCommand command = _ReDoCommands.Pop();
                    command.Execute();
                    _UnDoCommands.Push(command);
                    _UnDoCounter--;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReDo"));
            return (_ReDoCommands.Count > 0) ? true : false;

        }
        public void UnDoAll()
        {

            while (_UnDoCommands.Count > 0)
            {
                UnDo(1);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDoAll"));
        }

        public bool AllUnDoIsDone()
        {
            return _UnDoCommands.Count == 0;
        }
        public bool IsSatcksEmpty()
        {
            return _UnDoCommands.Count == 0 && _ReDoCommands.Count == 0;
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
        public void AddUnDoReDo(IUnDoReDoSystem unDoReDo)
        {
            _UnDoCommands.Push((UnDoReDoSystem)unDoReDo);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddUnDoReDo"));
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

        public void Execute(object parameter = null) //ReDo all UnDoed 
        {
            ReDo(_UnDoCounter);
        }
        public void UnExecute()
        {
            UnDoAll();
        }

        public bool CanExecute(object parameter)
        {
            return _UnDoCommands.Count > 0 || _ReDoCommands.Count > 0;
        }

        public UnDoReDoSystem()
        {
            Id = Guid.NewGuid();
        }

    }
}
