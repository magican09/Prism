using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core.Events
{
    public class EventMessage
    {
        private object _value;
        public DateTime Time { get; set; }
        public int From { get; set; }
        public int  To { get; set; }
        public Type  Type { get; set; }
        public string ParameterName { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        
        public object Value
        { get { return _value; ; } 
            set { _value = value; Type=value?.GetType(); } }
    }
}
