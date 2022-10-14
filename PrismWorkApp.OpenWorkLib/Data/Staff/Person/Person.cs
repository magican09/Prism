using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class Person : BindableBase, IPerson, IEntityObject
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
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
        private string _fathername;
        public string Fathername
        {
            get { return _fathername; }
            set { SetProperty(ref _fathername, value); }
        }

    }
}
