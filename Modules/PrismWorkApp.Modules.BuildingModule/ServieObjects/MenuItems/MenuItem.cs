using DevExpress.Mvvm;
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
        private string text;
        private bool isEnabled = true;
        private ObservableCollection<MenuItem> subItems;
        public MenuItem()
        {
            this.SubItems = new ObservableCollection<MenuItem>();
        }
      
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
        public ICommand Command
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
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnNotifyPropertyChanged("Text");
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

    }

}
