using DevExpress.Mvvm;
using PrismWorkApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class MenuItem :BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnNotifyPropertyChanged(string ptopertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(ptopertyName));
            }
        }
        private string name;
        private bool isEnabled = true;
        private ObservableCollection<MenuItem> subItems;
      
      
        public Uri IconUrl
        {
            get;
            set;
        }
        public bool IsSeparator
        {
            get;
            set;
        }
        public INotifyCommand Command
        {
            get;
            set;
        }
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                if (this.isEnabled != value)
                {
                    this.isEnabled = value;
                    this.OnNotifyPropertyChanged("IsEnabled");
                }
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNotifyPropertyChanged("Name");
                }
            }
        }
        public ObservableCollection<MenuItem> SubItems
        {
            get
            {
                if (this.subItems == null)
                {
                    this.subItems = new ObservableCollection<MenuItem>();
                }
                return this.subItems;
            }
            set
            {
                if (this.subItems != value)
                {
                    this.subItems = value;
                    this.OnNotifyPropertyChanged("SubItems");
                }
            }
        }
        public MenuItem()
        {
            this.SubItems = new ObservableCollection<MenuItem>();
        }

    }

}
