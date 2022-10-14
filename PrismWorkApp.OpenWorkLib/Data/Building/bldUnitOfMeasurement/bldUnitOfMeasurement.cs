using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldUnitOfMeasurement:BindableBase,IbldUnitOfMeasurement, IEntityObject
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }//Код
        private DateTime _date;
        private string _name ="ед.изм.";
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

        public bldUnitOfMeasurement(string name)
        {
            Name = name;
        }
        public bldUnitOfMeasurement()
        {

        }
    }
}
