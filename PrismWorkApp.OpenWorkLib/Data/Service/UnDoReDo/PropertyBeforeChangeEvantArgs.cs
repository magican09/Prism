using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate void PropertyBeforeChangeEventHandler(object sender, PropertyBeforeChangeEvantArgs e);
    /// <summary>
    /// Класс для передачи параметров из объкта непосредвенно перед изменениямя в нем. 
    /// </summary>
    public class PropertyBeforeChangeEvantArgs : EventArgs
    {
        public string PropertyName { get; set; }///Имя свойство котором будут происходить изменения
        public object LastValue { get; set; }///Старое заначение свойства
        public object NewValue { get; set; }///Новое значение свойства

        public PropertyBeforeChangeEvantArgs(string propName, object last_value, object new_value)
        {
            PropertyName = propName;
            LastValue = last_value;
            NewValue = new_value;
        }
    }

}
