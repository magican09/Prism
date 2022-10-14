using PrismWorkApp.ProjectModel.Data.Interfaces;
using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class Company :oldNode, ICompany, INotifyPropertyChanged
    {
       /* public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }*/
        public int Id { get; set; }
        private string _name;
        public string Name { get { return _name; } 
            set { _name = value;NodeName = _name; OnPropertyChanged("Name"); } }
        private string _fullname;
        public string FullName
        {
            get { return Name + ", ОГРН " + OGRN + ", ИНН " + INN + Address; }
            set { _fullname = value; OnPropertyChanged("FullName"); }
        }
        private string _address;
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged("Address"); } }
        private string _ogrn;
        public string OGRN { get { return _ogrn; } set { _ogrn = value; OnPropertyChanged("OGRN"); } }
        private string _inn;
        public string INN { get { return _inn; } set { _inn = value; OnPropertyChanged("INN"); } }
        private string _contacts;
        public string Contacts { get { return _contacts; } set { _contacts = value; OnPropertyChanged("Contacts"); } }
        // public  Company SROCompany { get; set; }

    }
}
