using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    /// <summary>
    /// Класс команды для регистрации изменения свойства объекта IJornable
    /// </summary>
    public class PropertySetCommand : UnDoRedoCommandBase, IUnDoRedoCommand
    {
        public string Name { get; set; }
        private IJornalable _ModelObject { get; set; }
        private object _Value;
        private object _LastValue;
        private object _Buffer;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter = null)
        {
            _Buffer = _Value;
            _Value = _LastValue;
            _LastValue = _Buffer;
            _ModelObject.JornalingOff();
            _ModelObject.GetType().GetProperty(Name).SetValue(_ModelObject, _Value);
            _ModelObject.ChangesJornal.Add(this);
            ChangedObjects.Add(_ModelObject);
            _ModelObject.JornalingOn();
        }
        public void UnExecute()
        {
            _Buffer = _Value;
            _Value = _LastValue;
            _LastValue = _Buffer;
            _ModelObject.JornalingOff();
            _ModelObject.GetType().GetProperty(Name).SetValue(_ModelObject, _Value);
            _ModelObject.ChangesJornal.Remove(this);
            ChangedObjects.Remove(_ModelObject);
            _ModelObject.JornalingOn();
        }
        /// <summary>
        /// Конструктор для создания команды изменения свойства наблюдаемого UnDoReDoSystem 
        /// </summary>
        /// <param name="model">Объекта IJornalable</param>
        /// <param name="propName"> Имя изменяемого свойства</param>
        /// <param name="new_value">Новое значение свойства</param>
        /// <param name="last_value">Старое значения свойства</param>
        public PropertySetCommand(IJornalable model, string propName, object new_value, object last_value)
        {
            _ModelObject = model;
            Name = propName;
            _Value = new_value;
            _LastValue = last_value;
            UnDoReDo_System = model.UnDoReDoSystem;
            ///Стои обратить внимание, что в отличии от других типовый IUnDoRedoCommand в этой команде в конструкторе
            ///лишь фиксируются значения, так как само действеие превый раз будет выполенено в самом объекте IJornable 
            ///после возврата из события IJornalable.PropertyBeforeChanged
            _ModelObject.JornalingOff();
            _ModelObject.ChangesJornal.Add(this);
            ChangedObjects.Add(_ModelObject);
            _ModelObject.JornalingOn();
        }

    }
}
