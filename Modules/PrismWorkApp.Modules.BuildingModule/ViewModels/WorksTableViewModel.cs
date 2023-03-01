using Prism.Events;
using Prism.Regions;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.ProjectModel.Data.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class WorksTableViewModel : LocalBindableBase, INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string _title = "";
        private BuildingConstruction _selectedBuildingConstruction;
        public BuildingConstruction SelectedBuildingConstruction
        {
            get { return _selectedBuildingConstruction; }
            set { _selectedBuildingConstruction = value; LoadAllProjectCommand.RaiseCanExecuteChanged(); OnPropertyChanged("SelectedBuildingConstruction"); }
        }
        private Work _selectedWork;

        private readonly IEventAggregator _eventAggregator;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public Work SelectedWork { get { return _selectedWork; } set { _selectedWork = value; OnPropertyChanged("SelectedWork"); } }
        public WorksTableViewModel(IEventAggregator eventAggregator)
        {
            LoadAllProjectCommand = new NotifyCommand(LoadAllProjects, CanLoadAllProjects);
            CreateAOSRCommand = new NotifyCommand(CreateAOSR, CanCreateAOSR).ObservesProperty(() => SelectedWork);
            _eventAggregator = eventAggregator;
            //     _eventAggregator.GetEvent<ProjectSentEvent>().Subscribe(OnBuildingConstructionRecieved);
        }

        private void OnBuildingConstructionRecieved(BuildingConstruction buildingConstruction)
        {
            SelectedBuildingConstruction = buildingConstruction;
        }

        private bool CanLoadAllProjects()
        {
            return true;
        }
        private bool CanCreateAOSR()
        {
            return SelectedWork != null && SelectedBuildingConstruction != null;
        }
        private void CreateAOSR()
        {
            // ProjectService.SaveAOSRToWord(SelectedWork);

        }
        public NotifyCommand LoadAllProjectCommand { get; private set; }
        public NotifyCommand CreateAOSRCommand { get; private set; }
        private void LoadAllProjects()
        {

        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            BuildingConstruction buildingConstruction = (BuildingConstruction)navigationContext.Parameters["building_construction"];
            if (SelectedBuildingConstruction == null || SelectedBuildingConstruction?.FullName != buildingConstruction.FullName)
            {
                SelectedBuildingConstruction = buildingConstruction;

            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
