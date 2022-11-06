using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.Core.Commands
{
    public class NotifyCompositeCommand 
    {
      /*  private EventHandler _canExecuteChanged;

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


        public ApplicationCompositeCommand() : base()
        {
            RegisteredCommands.CollectionChanged += OnRegisteredCommandsChanged;
        }

        private void OnRegisteredCommandsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ApplicationCommand del_comm in (ObservableCollection<ApplicationCommand>)e.NewItems)
                {

                }
            }
        }

        public ApplicationCompositeCommand(bool monitorCommandActivity) : base(monitorCommandActivity)
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
            if (_canExecuteChanged != null)
            {
                _canExecuteChanged(this, System.EventArgs.Empty);
            }
            base.OnCanExecuteChanged();
        }*/
    }
}
