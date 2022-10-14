using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class  ConstructionCompany: Company , INotifyPropertyChanged
    {
     /*   public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }*/
     /*   private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        private string _fullname;
        public string FullName { get { return _fullname; } set { _fullname = value; OnPropertyChanged("FullName"); } }
  */
        private  Company _sROIssuingCompany;
        public Company SROIssuingCompany { get { return _sROIssuingCompany; } set { _sROIssuingCompany = value; OnPropertyChanged("SROIssuingCompany"); } }
    }
}
