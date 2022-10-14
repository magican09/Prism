using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class KeyValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string _key;
        public string Key
        {
            get { return _key; }
            set {  _key= value; OnPropertyChanged("Key"); }
        }
        private object _value;
        public object Value
        {
            get { return _value; }
            set {  _value = value; OnPropertyChanged("Key"); }
        }
       
        public KeyValue()
        {

        }
        public KeyValue(string key,object value)
        {
            Key = key;
            Value = value;
        }
    }
}
