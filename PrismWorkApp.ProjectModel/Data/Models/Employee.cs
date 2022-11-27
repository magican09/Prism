using PrismWorkApp.ProjectModel.Data.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class Employee : IEmployee, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public int Id { get; set; }
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }

        private string _fullName;
        public string FullName { get { return _fullName; } set { _fullName = value; OnPropertyChanged("FullName"); } }
        private int _number;
        public int Number { get { return _number; } set { _number = value; OnPropertyChanged("Number"); } }
        public Employee()
        {

        }
        public Employee(string fullName)
        {
            FullName = fullName;
        }
        private IEmployeePosition _employeePosition;
        public IEmployeePosition EmployeePosition { get { return _employeePosition; } set { _employeePosition = value; OnPropertyChanged("EmployeePosition"); } }

        // public IBuildingObject BuildingObject { get; set; }
    }
}
