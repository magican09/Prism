using System.ComponentModel;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class ResponsibleEmployee : oldNode, INotifyPropertyChanged
    {
        /*  public event PropertyChangedEventHandler PropertyChanged;
      public void OnPropertyChanged([CallerMemberName] string prop = "")
      {
          if (PropertyChanged != null)
              PropertyChanged(this, new PropertyChangedEventArgs(prop));
      }*/
        public int Id { get; set; }
        private string _code;
        public string Code { get { return _code; } set { _code = value; OnPropertyChanged("Code"); } }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _fullname;

        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; NodeName = _fullname; OnPropertyChanged("FullName"); }
        }
        private int _number;
        public int Number { get { return _number; } set { _number = value; OnPropertyChanged("Number"); } }
        private string _nRSId;
        public string NRSId { get { return _nRSId; } set { _nRSId = value; OnPropertyChanged("NRSId"); } }
        private RoleOfResponsible _roleOfResponsible;
        public RoleOfResponsible RoleOfResponsible
        { get { return _roleOfResponsible; } set { _roleOfResponsible = value; OnPropertyChanged("RoleOfResponsible"); } }
        private Document _docConfirmingTheAthority;
        public Document DocConfirmingTheAthority
        { get { return _docConfirmingTheAthority; } set { _docConfirmingTheAthority = value; OnPropertyChanged("DocConfirmingTheAthority"); } }
        private Company _company;
        public Company Company { get { return _company; } set { _company = value; OnPropertyChanged("Company"); } }
        private EmployeePosition _employeePosition;
        public EmployeePosition EmployeePosition { get { return _employeePosition; } set { _employeePosition = value; OnPropertyChanged("EmployeePosition"); } }
    }
    public enum RoleOfResponsible
    {
        CUSTOMER, //Заказчик
        GENERAL_CONTRACTOR, //Генеральный подрядчик
        GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER, //Генеральный подрядчик
        AUTHOR_SUPERVISION, // Авторский надзор 
        WORK_PERFORMER,//Подрядчик
        OTHER,
        NONE
    }

}
