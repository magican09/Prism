using Prism;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public interface  IApplicationCommands
    {
        CompositeCommand SaveAllCommand { get; }
    }
    public class ApplicationCommands : IApplicationCommands
    {
         private CompositeCommand _saveAllCommand = new CompositeCommand();
         public CompositeCommand SaveAllCommand
         {
             get { return _saveAllCommand; }
         }

        // public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }
    public class ApplicationCompositeCommand : CompositeCommand
    {
        private EventHandler _canExecuteChanged;

        public new event EventHandler CanExecuteChanged
        {
            add
            {
                _canExecuteChanged += value;
            }
            remove
            {
                _canExecuteChanged -= value;
            }
        }

        private ObservableCollection<ApplicationCommand> _registeredCommands = new ObservableCollection<ApplicationCommand>();

        public ObservableCollection<ApplicationCommand> RegisteredCommands { get; set; } = new ObservableCollection<ApplicationCommand>();


        public ApplicationCompositeCommand():base()
        {
            RegisteredCommands.CollectionChanged += OnRegisteredCommandsChanged;
        }

        private void OnRegisteredCommandsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action== NotifyCollectionChangedAction.Add)
            {
                foreach(ApplicationCommand del_comm in (ObservableCollection<ApplicationCommand>)e.NewItems)
                {

                }
            }    
        }

        public ApplicationCompositeCommand(bool monitorCommandActivity):base(monitorCommandActivity)
        {

        }

        private int can_executes_counter = 0;
        public override bool CanExecute(object parameter)
        {
            can_executes_counter++;
            return base.CanExecute(parameter);
        }
        public override void Execute(object parameter)
        {
            var fdf = this;
            base.Execute(parameter);
        }
        protected override void OnCanExecuteChanged()
        {
            if(_canExecuteChanged!=null)
            {
                _canExecuteChanged(this, System.EventArgs.Empty);
            }
            base.OnCanExecuteChanged();
        }
    }
    public class ApplicationCommand : ICommand, IActiveAware
    {
        Action _TargetExecuteMetod;
        Func<bool> _TargetCanExecuteMethod;
        public event EventHandler CanExecuteChanged = delegate { };
        public bool IsActive { get; set; }
         public event EventHandler IsActiveChanged;

        public ApplicationCommand(Action executeMethod)
        {
            _TargetExecuteMetod = executeMethod;
        }
        public ApplicationCommand(Action executeMethod,Func<bool> canExecuteMehtod)
        {
            _TargetExecuteMetod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMehtod;
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #region ICommand members

        bool ICommand.CanExecute(object parameter)
        {
            if(_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod(); 
            }
            if(_TargetCanExecuteMethod==null)
            {
                return true;
            }
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMetod != null)
                _TargetExecuteMetod();
        }
      /*  protected override bool CanExecute(object parameter)
        {
            if (_TargetExecuteMetod != null)
                _TargetExecuteMetod();
        }
        protected override void Execute(object parameter)
        {


        }*/

        #endregion
        #region DelegateCommand members 
        public ApplicationCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            return new ApplicationCommand(() =>{ });
           
        }
       
        public ApplicationCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            return new ApplicationCommand(() => { });
        }
        #endregion
    }


}
