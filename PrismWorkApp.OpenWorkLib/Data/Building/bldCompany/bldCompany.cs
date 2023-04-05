using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldCompany : BindableBase, IbldCompany
    {
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
            get
            {
                SetProperty(ref _fullName, Name + ", ОГРН " + OGRN + ", ИНН " + INN + Address);
                return _fullName;
            }
            set { SetProperty(ref _fullName, value); }
        }
        private string _ogrn;
        public string OGRN
        {
            get { return _ogrn; }
            set { SetProperty(ref _ogrn, value); }
        }
        private string _inn;
        public string INN
        {
            get { return _inn; }
            set { SetProperty(ref _inn, value); }
        }
        private string _contacts;
        public string Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }


        //    public bldConstructionCompany bldConstructionCompany { get; set; }

        //public Guid bldResponsibleEmployeeId { get; set; }
        //  public bldResponsibleEmployee bldResponsibleEmployee { get; set; }

        //public Guid bldParticipantId { get; set; }
        // public bldParticipant bldParticipant { get; set; }
    }
}
