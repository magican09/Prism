using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class Project : oldNode, INotifyPropertyChanged
    {
        /*public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }*/
        public Guid Id { get; set; }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; NodeName = _name; OnPropertyChanged("Name"); OnPropertyChanged("FullName"); }
        }
        private string _fullname;
        public string FullName
        {
            get { return Name + ", " + Address; }
            set { _fullname = value; OnPropertyChanged("FullName"); }
        }
        private string _shortName;
        public string ShortName { get { return _shortName; } set { _shortName = value; OnPropertyChanged("ShortName"); } }
        private string _address;
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged("Address"); } }
        private DateTime _startDate;
        public DateTime StartDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("StartDate"); } }
        //public ObservableCollection <IProject> SubProjects { get; set; }
        //   private ObservableCollection<Work> _works;
        //     public ObservableCollection<Work> Works  { get { return _works; } set { _works = value; OnPropertyChanged("Works"); } }
        private ObservableCollection<BuildingObject> _buildingJbjects;
        public ObservableCollection<BuildingObject> BuildingObjects { get { return _buildingJbjects; } set { _buildingJbjects = value; OnPropertyChanged("BuildingObjects"); } }
        private ObservableCollection<Participant> _participants;
        public ObservableCollection<Participant> Participants
        { get { return _participants; } set { _participants = value; OnPropertyChanged("Participants"); } }
        private ObservableCollection<ResponsibleEmployee> _responsibleEmployees;
        public ObservableCollection<ResponsibleEmployee> ResponsibleEmployees
        { get { return _responsibleEmployees; } set { _responsibleEmployees = value; OnPropertyChanged("ResponsibleEmployees"); } }
        public Project()
        {
            Participants = new ObservableCollection<Participant>();
            //Works = new ObservableCollection<Work>();
            ResponsibleEmployees = new ObservableCollection<ResponsibleEmployee>();
            BuildingObjects = new ObservableCollection<BuildingObject>();
        }
        public Project(string name)
        {
            Participants = new ObservableCollection<Participant>();
            //Works = new ObservableCollection<Work>();
            ResponsibleEmployees = new ObservableCollection<ResponsibleEmployee>();
            Name = name;
        }
    }

}
