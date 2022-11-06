using Prism;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public class NotifyCommand : DelegateCommandBase, ICommand, IActiveAware
    {
        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;
        public event EventHandler CanExecuteChanged = delegate { };
        protected Action _TargetExecuteMetod;
        protected Func<bool> _TargetCanExecuteMethod;
      
        public NotifyCommand(Action executeMethod)
        {
            _TargetExecuteMetod = executeMethod;
        }
        public NotifyCommand(Action executeMethod, Func<bool> canExecuteMehtod)
        {
            _TargetExecuteMetod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMehtod;
        }
        #region ICommand members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }
            if (_TargetCanExecuteMethod == null)
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
       

        #endregion
        #region DelegateCommand members 
        public NotifyCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {

            var body = (MemberExpression)canExecuteExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value;
            var notify_view_model = (INotifyPropertyChanged)view_model;
            PropertyInfo member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name;
            var sd = member_prop_info.GetValue(view_model);
            Func<bool> prop_get_Delegate = () => { return (bool)member_prop_info.GetValue(view_model); };
            if (!ObservesCanExecutePropertiesDictationary.ContainsKey(prop_name))
            {
                ObservesCanExecutePropertiesDictationary.Add(prop_name, (Func<bool>)prop_get_Delegate);
                notify_view_model.PropertyChanged += ObservesCanExecutePropertyChanged;
            }
            return this;
        }

        private void ObservesCanExecutePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ObservesCanExecutePropertiesDictationary.ContainsKey(e.PropertyName))
            {
                var buffer_delegate = _TargetCanExecuteMethod; //Сохраняем текущее состояние делегата
                _TargetCanExecuteMethod = ObservesCanExecutePropertiesDictationary[e.PropertyName];
                RaiseCanExecuteChanged();
                _TargetCanExecuteMethod = buffer_delegate;//Востанавливаем состяние делегата
            }
        }
        private Dictionary<string, Func<bool>> ObservesCanExecutePropertiesDictationary = new Dictionary<string, Func<bool>>();
        private List<string> ObservesPropertiesNames = new List<string>();
        public  NotifyCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {

            var body = (MemberExpression)propertyExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value;
            var notify_view_model = (INotifyPropertyChanged)view_model;
            var member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name;
            if (!ObservesPropertiesNames.Contains(prop_name))
            {
                ObservesPropertiesNames.Add(prop_name);
                notify_view_model.PropertyChanged += ObservesPropertyChanged;
            }
            return this;
        }

        private void ObservesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ObservesPropertiesNames.Contains(e.PropertyName))
                RaiseCanExecuteChanged();
        }
        #endregion
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        protected override void Execute(object parameter)
        {
          Execute(parameter);
        }

        protected override bool CanExecute(object parameter)
        {
           return CanExecute(parameter);
        }
    }
    public class NotifyCommand<T> : DelegateCommandBase, ICommand, IActiveAware
    {
        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;
        public event EventHandler CanExecuteChanged = delegate { };
        protected Action<T> _TargetExecuteMetod;
        protected Func<T,bool> _TargetCanExecuteMethod;

        public NotifyCommand(Action<T> executeMethod)
        {
            _TargetExecuteMetod = executeMethod;
        }
        public NotifyCommand(Action<T> executeMethod, Func<T,bool> canExecuteMehtod)
        {
            _TargetExecuteMetod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMehtod;
        }
        #region ICommand members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                T param = (T)parameter;
                return _TargetCanExecuteMethod(param);
            }
            if (_TargetCanExecuteMethod == null)
            {
                return true;
            }
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMetod != null)
                _TargetExecuteMetod((T)parameter);
        }


        #endregion
        #region DelegateCommand members 
        public NotifyCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {

            var body = (MemberExpression)canExecuteExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value;
            var notify_view_model = (INotifyPropertyChanged)view_model;
            PropertyInfo member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name;
            var sd = member_prop_info.GetValue(view_model);
            Func<T,bool> prop_get_Delegate = (param) => { return (bool)member_prop_info.GetValue(view_model); };
            if (!ObservesCanExecutePropertiesDictationary.ContainsKey(prop_name))
            {
                ObservesCanExecutePropertiesDictationary.Add(prop_name, (Func<T,bool>)prop_get_Delegate);
                notify_view_model.PropertyChanged += ObservesCanExecutePropertyChanged;
            }
            return this;
        }

        private void ObservesCanExecutePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ObservesCanExecutePropertiesDictationary.ContainsKey(e.PropertyName))
            {
                var buffer_delegate = _TargetCanExecuteMethod; //Сохраняем текущее состояние делегата
                _TargetCanExecuteMethod = ObservesCanExecutePropertiesDictationary[e.PropertyName];
                RaiseCanExecuteChanged();
                _TargetCanExecuteMethod = buffer_delegate;//Востанавливаем состяние делегата
            }
        }
        private Dictionary<string, Func<T,bool>> ObservesCanExecutePropertiesDictationary = new Dictionary<string, Func<T,bool>>();
        private List<string> ObservesPropertiesNames = new List<string>();
        public NotifyCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {

            var body = (MemberExpression)propertyExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value;
            var notify_view_model = (INotifyPropertyChanged)view_model;
            var member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name;
            if (!ObservesPropertiesNames.Contains(prop_name))
            {
                ObservesPropertiesNames.Add(prop_name);
                notify_view_model.PropertyChanged += ObservesPropertyChanged;
            }
            return this;
        }

        private void ObservesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ObservesPropertiesNames.Contains(e.PropertyName))
                RaiseCanExecuteChanged();
        }
        #endregion
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        protected override void Execute(object parameter)
        {
            Execute(parameter);
        }

        protected override bool CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }
    }
}
