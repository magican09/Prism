using PrismWorkApp.ProjectModel.Data.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class EmployeePosition : IEmployeePosition, INotifyPropertyChanged
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
        private string _fullname;
        public string FullName { get { return Name; } set { Name = value; OnPropertyChanged("FullName"); } }//Временно!!

        public EmployeePosition(string name)
        {
            Name = name;
        }

    }
}
