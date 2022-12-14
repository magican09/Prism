using Prism;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace PrismWorkApp.Core.Commands
{
    public class NotifyCommand : NotifyCommandBase, ICommand, IActiveAware, INotifyCommand
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged = delegate { };
        public event EventHandler CanExecuteChanged = delegate { };
        protected Action _TargetExecuteMetod;
        protected Func<bool> _TargetCanExecuteMethod;
        //public NotifyCommand(Action executeMethod)
        //{
        //    _TargetExecuteMetod = executeMethod;
        //}
        public NotifyCommand(Action executeMethod)
        {
            RegisterActiveAwareEventHandler(executeMethod);
             _TargetExecuteMetod = executeMethod;
        }

        private void RegisterActiveAwareEventHandler(Action executeMethod)
        {
            var command_parent_object = executeMethod.Target;
            if (command_parent_object is IActiveAware active_aware_object)
            {
                active_aware_object.IsActiveChanged += OnIsActivateChaged;
            }
        }
        public NotifyCommand(Action executeMethod, Func<bool> canExecuteMehtod)
        {
            RegisterActiveAwareEventHandler(executeMethod);
            _TargetExecuteMetod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMehtod;
        }
        private void OnIsActivateChaged(object sender, EventArgs e)
        {
            if (sender is IActiveAware active_aware_object)
            {
                IsActive = active_aware_object.IsActive;
                IsActiveChanged.Invoke(this, new EventArgs());
            }
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
        #region NotifyCommand members 
        /// <summary>
        /// Добавляем метод get свойства типа bool в качестве метода CanExecute
        /// </summary>
        /// <param name="canExecuteExpression"></param>
        /// <returns></returns>
        public NotifyCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {

            var body = (MemberExpression)canExecuteExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value; //Значение родиельского объекта VeiwModel
            var notify_view_model = (INotifyPropertyChanged)view_model;
            PropertyInfo member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name; //Получаем имся свойства к которому привязываемся
            Func<bool> prop_get_Delegate = () => { return (bool)member_prop_info.GetValue(view_model); };//Получаем делегат метода get свойства
            if (!ObservesCanExecutePropertiesDictationary.ContainsKey(prop_name))//Добаляем делегат в список наблюдаемых делегатов в качестве CanExecute
            {
                ObservesCanExecutePropertiesDictationary.Add(prop_name, (Func<bool>)prop_get_Delegate);
                notify_view_model.PropertyChanged += ObservesCanExecutePropertyChanged;//Подписываемся на событие PropertyChanged
            }
            return this;
        }
        /// <summary>
        /// Метод вызывается событиями PropertyChaned и вызывает соотвествующий метод CanExecute добавленный в список ранее   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservesCanExecutePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ObservesCanExecutePropertiesDictationary.ContainsKey(e.PropertyName))//Ищем зпись с методов в словаре по имени свойства
            {
                var buffer_delegate = _TargetCanExecuteMethod; //Сохраняем текущее состояние делегата
                _TargetCanExecuteMethod = ObservesCanExecutePropertiesDictationary[e.PropertyName];
                RaiseCanExecuteChanged();
                _TargetCanExecuteMethod = buffer_delegate;//Востанавливаем состяние делегата
            }
        }
        private Dictionary<string, Func<bool>> ObservesCanExecutePropertiesDictationary = new Dictionary<string, Func<bool>>();
        private List<string> ObservesPropertiesNames = new List<string>();
        /// <summary>
        /// Добавляем свойство в список свойств для отслеживания изменений
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public NotifyCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {

            var body = (MemberExpression)propertyExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value; //Значение родиельского объекта VeiwModel
            var notify_view_model = (INotifyPropertyChanged)view_model;
            var member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name;
            if (!ObservesPropertiesNames.Contains(prop_name))//Добавляем имя наблюдаемого свойства в список наоблюдаемых свойств
            {
                ObservesPropertiesNames.Add(prop_name);
                notify_view_model.PropertyChanged += ObservesPropertyChanged;//Подписываемся на событие PropertyChanged
            }
            return this;
        }
        public NotifyCommand ObservesPropertyChangedEvent(INotifyPropertyChanged obj)
        {
            obj.PropertyChanged += RaiseCanExecuteChanged;//Подписываемся на событие PropertyChanged
            return this;
        }



        /// <summary>
        /// Метод вызываемый собятием PropertyChanged  объктом ViewModel на котором находятис свойства на изменения которых подписаны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ObservesPropertiesNames.Contains(e.PropertyName))
                RaiseCanExecuteChanged();
        }
        #endregion
        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            RaiseCanExecuteChanged();
            //   CanExecuteChanged(this, EventArgs.Empty);
        }
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

    public class NotifyCommand<T> : NotifyCommandBase, ICommand, IActiveAware, INotifyCommand
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged = delegate { };
        public event EventHandler CanExecuteChanged = delegate { };
        protected Action<T> _TargetExecuteMetod;
        protected Func<T, bool> _TargetCanExecuteMethod;
        public NotifyCommand(Action<T> executeMethod)
        {
            RegisterActiveAwareEventHandler(executeMethod);
            _TargetExecuteMetod = executeMethod;
        }
        public NotifyCommand(Action<T> executeMethod, Func<T, bool> canExecuteMehtod)
        {
            RegisterActiveAwareEventHandler(executeMethod);
            _TargetExecuteMetod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMehtod;
        }
        private void RegisterActiveAwareEventHandler(Action<T> executeMethod)
        {
            var command_parent_object = executeMethod.Target;
            if (command_parent_object is IActiveAware active_aware_object)
            {
                active_aware_object.IsActiveChanged += OnIsActivateChaged;
            }
        }
        private void OnIsActivateChaged(object sender, EventArgs e)
        {
            if (sender is IActiveAware active_aware_object)
            {
                IsActive = active_aware_object.IsActive;
                IsActiveChanged?.Invoke(this, new EventArgs());
            }
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
        #region NotifyCommand members 
        public NotifyCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {

            var body = (MemberExpression)canExecuteExpression.Body;
            var view_model_expression = (ConstantExpression)body.Expression;
            var view_model = view_model_expression.Value;
            var notify_view_model = (INotifyPropertyChanged)view_model;
            PropertyInfo member_prop_info = body.Member as PropertyInfo;
            string prop_name = member_prop_info.Name;
            var sd = member_prop_info.GetValue(view_model);
            Func<T, bool> prop_get_Delegate = (param) => { return (bool)member_prop_info.GetValue(view_model); };
            if (!ObservesCanExecutePropertiesDictationary.ContainsKey(prop_name))
            {
                ObservesCanExecutePropertiesDictationary.Add(prop_name, (Func<T, bool>)prop_get_Delegate);
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
        private Dictionary<string, Func<T, bool>> ObservesCanExecutePropertiesDictationary = new Dictionary<string, Func<T, bool>>();
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
        public NotifyCommand<T> ObservesPropertyChangedEvent(INotifyPropertyChanged obj)
        {
            obj.PropertyChanged += RaiseCanExecuteChanged;//Подписываемся на событие PropertyChanged
            return this;
        }
        #endregion
        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            RaiseCanExecuteChanged();
            //   CanExecuteChanged(this, EventArgs.Empty);
        }
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
