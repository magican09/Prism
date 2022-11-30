namespace PrismWorkApp.OpenWorkLib.Data
{
    public class Employee : Person, IEmployee, IEntityObject
    {
        private decimal _salary;
        public decimal Salary
        {
            get { return _salary; }
            set { SetProperty(ref _salary, value); }
        }
        private EmployeePosition _position;
        public virtual EmployeePosition Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }
        private bldCompany _company;
        [NavigateProperty]
        public bldCompany Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
