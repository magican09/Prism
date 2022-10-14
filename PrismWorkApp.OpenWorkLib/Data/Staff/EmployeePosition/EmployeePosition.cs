using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public class EmployeePosition:BindableBase,IEmployeePosition, IEntityObject
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); StoredId = value; }
        }
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
       
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _shortName;
        public string ShortName
        {
            get { return _shortName; }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }
        public EmployeePosition()
        {

        }
        public EmployeePosition(string name)
        {
            Name=name;
        }
    }
}
