using PrismWorkApp.ProjectModel.Data.Interfaces;
using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Models
{
    public class BuildingObject:Project, INotifyPropertyChanged
    {
      /*  public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }*/
          private Project _project;
        public Project Project { get { return _project; } set { _project = value; OnPropertyChanged("Project"); } }
       private ObservableCollection<BuildingConstruction > _buildingConstructions;
        public ObservableCollection<BuildingConstruction> BuildingConstructions { get { return _buildingConstructions; } set { _buildingConstructions = value; OnPropertyChanged("BuildingConstructions"); } }
        public BuildingObject()
        {
            BuildingConstructions = new ObservableCollection<BuildingConstruction>();
        }
    }
}
