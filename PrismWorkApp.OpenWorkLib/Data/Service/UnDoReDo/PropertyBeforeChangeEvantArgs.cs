using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public delegate void PropertyBeforeChangeEventHandler(object sender, PropertyBeforeChangeEvantArgs e);
        public  class PropertyBeforeChangeEvantArgs: EventArgs
    {
        public string PropertyName { get; set; }
        public object LastValue { get; set; }
        public object NewValue { get; set; }

        public PropertyBeforeChangeEvantArgs(string propName,object last_value,object new_value)
        {
            PropertyName = propName;
            LastValue = last_value;
            NewValue = new_value;
        }
    }
    
}
