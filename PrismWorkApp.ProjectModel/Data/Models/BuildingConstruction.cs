using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class BuildingConstruction : oldNode, INotifyPropertyChanged
    {
        /* public event PropertyChangedEventHandler PropertyChanged;
         public void OnPropertyChanged([CallerMemberName] string prop = "")
         {
             if (PropertyChanged != null)
                 PropertyChanged(this, new PropertyChangedEventArgs(prop));
         }*/
        private string _name;
        public string Name { get { return _name; } set { _name = value; NodeName = _name; OnPropertyChanged("Name"); } }
        private string _fullname;
        public string FullName { get { return _fullname; } set { _fullname = value; OnPropertyChanged("FullName"); } }
        private Project _project;
        public Project Project { get { return _project; } set { _project = value; OnPropertyChanged("Project"); } }
        private BuildingObject _buildingObject;
        public BuildingObject BuildingObject
        {
            get { return _buildingObject; }
            set { _buildingObject = value; OnPropertyChanged("BuildingObject"); }
        }

        private ObservableCollection<Work> _works;
        public ObservableCollection<Work> Works { get { return _works; } set { _works = value; OnPropertyChanged("Works"); } }
        private ObservableCollection<AOSRDocument> _aOSRDocuments;
        public ObservableCollection<AOSRDocument> AOSRDocuments
        {
            get { return _aOSRDocuments; }
            set { _aOSRDocuments = value; OnPropertyChanged("AOSRDocuments"); }
        }

        public BuildingConstruction()
        {

            Works = new ObservableCollection<Work>();
            AOSRDocuments = new ObservableCollection<AOSRDocument>();
        }
    }
}
