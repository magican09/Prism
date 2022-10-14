using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.Core
{
    public class Nodes: ObservableCollection<INode>,INodes ,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string _name ;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }
        private PropertyInfo _propertyInfo;
        public PropertyInfo PropertyName
        {
            get { return _propertyInfo; }
            set {  _propertyInfo= value; OnPropertyChanged("PropertyName"); }
        }
        public PropertyInfo PropertyInfo { get; set; }
    }
}
