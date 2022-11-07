﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public class NotifyCompositeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected Action _TargetExecuteMetod;
        protected Func<bool> _TargetCanExecuteMethod;

        private bool _monitorCommandActivity;
        private ObservableCollection<ICommand> _registeredCommands = new ObservableCollection<ICommand>();

        public ObservableCollection<ICommand> RegisteredCommands { get; set; } = new ObservableCollection<ICommand>();
        private ICommand _LastCommand { get; set; }
        public NotifyCompositeCommand()
        {
            RegisteredCommands.CollectionChanged += OnRegisteredCommandsChanged;
        }

        public NotifyCompositeCommand(bool monitorCommandActivity):base()
        {

        }
        private void OnRegisteredCommandsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                
            }
        }
        #region ICommand members

        bool ICommand.CanExecute(object parameter)
        {

            bool can_execute_val = true;
            bool _LastCommandCanExecuteVal = true;
            bool _RegisteredCommandsCanExecuteVal = true;
            bool _TargetCanExecuteMethod_CanExecuteVal = true;


            foreach (ICommand command in RegisteredCommands)
                if (command.CanExecute(parameter) == false)
                {
                    _RegisteredCommandsCanExecuteVal = false;
                    break;
                }

            if (_LastCommand != null)
                _LastCommandCanExecuteVal = _LastCommand.CanExecute(parameter);
           
            if (_TargetCanExecuteMethod != null)
            {
                _TargetCanExecuteMethod_CanExecuteVal = _TargetCanExecuteMethod();
            }
            if (_TargetCanExecuteMethod == null)
            {
                _TargetCanExecuteMethod_CanExecuteVal =  true ;
            }
            can_execute_val = (RegisteredCommands.Count > 0) & _RegisteredCommandsCanExecuteVal &&
               _LastCommandCanExecuteVal &&
               _TargetCanExecuteMethod_CanExecuteVal;


            return can_execute_val;
        }

        void ICommand.Execute(object parameter)
        {
            foreach (ICommand command in RegisteredCommands)
                command.Execute(parameter);

            if (_TargetExecuteMetod != null)
                _TargetExecuteMetod();
            if (_LastCommand != null)
                _LastCommand.Execute(parameter);
        }


        #endregion

        public virtual void RegisterCommand(ICommand command)
        {
            RegisteredCommands.Add(command);
            command.CanExecuteChanged += RaiseChildrenCanExecuteChanged;
            RaiseCanExecuteChanged();
        }

        private void RaiseChildrenCanExecuteChanged(object sender, EventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        public virtual void UnregisterCommand(ICommand command)
        {
            RegisteredCommands.Remove(command);
            command.CanExecuteChanged -= RaiseChildrenCanExecuteChanged;
            RaiseCanExecuteChanged();
        }
        public void SetExecuteMethod(Action execute_method)
        {
            _TargetExecuteMetod = execute_method;
        }

        public void SetCanExecuteMethod(Func<bool> can_execute_method)
        {
            _TargetCanExecuteMethod = can_execute_method;
        }
        public void SetLastCommand(ICommand mainCommand)
        {
            _LastCommand = mainCommand;
            _LastCommand.CanExecuteChanged += RaiseChildrenCanExecuteChanged;
          

        }
        protected virtual bool ShouldExecute(ICommand command)
        {
            return true;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}