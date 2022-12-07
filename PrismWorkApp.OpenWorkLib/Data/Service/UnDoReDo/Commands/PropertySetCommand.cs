using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class PropertySetCommand : IUnDoRedoCommand
    {
        public string Name { get; set; }
        private IJornalable _ModelObject { get; set; }
        private object _Value;
        private object _LastValue;
        private object _Buffer;
        public event EventHandler CanExecuteChanged;
        public PropertySetCommand(IJornalable model, string propName, object new_value, object last_value)
        {
            _ModelObject = model;
            Name = propName;
            _Value = new_value;
            _LastValue = last_value;
        }
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
            _ModelObject.JornalingOn();
        }
        public void UnExecute()
        {
            _Buffer = _Value;
            _Value = _LastValue;
            _LastValue = _Buffer;
            _ModelObject.JornalingOff();
            _ModelObject.GetType().GetProperty(Name).SetValue(_ModelObject, _Value);
            _ModelObject.JornalingOn();
        }

    }
}
