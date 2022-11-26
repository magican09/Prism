using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public class UnDoReDoSystem : INotifyPropertyChanged
    {
        private Stack<IUnDoRedoCommand> _UnDoCommands = new Stack<IUnDoRedoCommand>();
        private Stack<IUnDoRedoCommand> _ReDoCommands = new Stack<IUnDoRedoCommand>();
        private ObservableCollection<IJornalable> _RegistedModels = new ObservableCollection<IJornalable>();

        public event PropertyChangedEventHandler PropertyChanged =  delegate{};

        public void ReDo(int levels)
        {
            for(int ii=0;ii< levels;ii++)
            {
                if(_ReDoCommands.Count>0)
                {
                    IUnDoRedoCommand command = _ReDoCommands.Pop();
                    command.Execute();
                    _UnDoCommands.Push(command);
                }
            }
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("ReDo"));
        }
        public void UnDo (int levels)
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
            obj._PropertyBeforeChanged += OnModelPropertyBeforeChanged;
            var prop_infoes = obj.GetType().GetProperties().Where(pr=>pr.GetIndexParameters().Length==0);
            foreach(PropertyInfo property_info in prop_infoes)
            {
                var prop_value = property_info.GetValue(obj);
                if (prop_value is IObservedCommand command)
                {
                    command.ObservedCommandCreated += OnObservedCommandCreated;
                }
            }
        }

        private void OnObservedCommandCreated(object sender, ObservedCommandExecuteEventsArgs e)
        {
            _UnDoCommands.Push(e.Command);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnCommandCreated"));
        }


        private void OnModelPropertyBeforeChanged(object sender, PropertyBeforeChangeEvantArgs e)
        {
            PropertySetCommand setCommand = 
                new PropertySetCommand(sender as IJornalable,e.PropertyName,e.NewValue,e.LastValue);
           _UnDoCommands.Push(setCommand);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnModelPropertyBeforeChanged"));
        }
        public UnDoReDoSystem()
        {
            
        }
     
    }
}
