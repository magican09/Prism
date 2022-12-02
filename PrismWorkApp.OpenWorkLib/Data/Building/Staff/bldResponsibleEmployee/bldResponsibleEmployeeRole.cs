using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   public  class bldResponsibleEmployeeRole : BindableBase, IKeyable, INameable
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }
        private string _shortName;
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set { SetProperty(ref _shortName, value); }
        }
        
        private RoleOfResponsible _roleCode;
        public RoleOfResponsible RoleCode
        {
            get { return _roleCode; }
            set
            {
                SetProperty(ref _roleCode, value);
                OnPropertyChanged("Name");
                OnPropertyChanged("FullName");

            }
        }
        public bldResponsibleEmployeeRole()
        {

        }
        public bldResponsibleEmployeeRole(RoleOfResponsible role)
        {
            RoleCode = role;
        }
    }
}
