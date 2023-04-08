using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class EmployeePosition : BindableBase, IEmployeePosition, IEntityObject
    {


        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
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
            Name = name;
        }
    }
}
