using Prism.Events;
using Prism.Regions;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.ProjectModel.Data.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class AOSRDocumentsTableViewModel : LocalBindableBase, INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private readonly IEventAggregator _eventAggregator;
        private AOSRDocument _selectedAOSRDocument;
        public AOSRDocument SelectedAOSRDocument
        {
            get { return _selectedAOSRDocument; }
            set { _selectedAOSRDocument = value; OnPropertyChanged("SelectedAOSRDocument"); }
        }
        private ObservableCollection<AOSRDocument> _aOSRDocuments;
        public ObservableCollection<AOSRDocument> AOSRDocuments
        {
            get { return _aOSRDocuments; }
            set { _aOSRDocuments = value; OnPropertyChanged("AOSRDocuments"); }
        }
        private BuildingConstruction _selectedBuildingConstruction;
        public BuildingConstruction SelectedBuildingConstruction
        {
            get { return _selectedBuildingConstruction; }
            set { _selectedBuildingConstruction = value; OnPropertyChanged("SelectedBuildingConstruction"); }
        }
        public AOSRDocumentsTableViewModel()
        {
            CreateAOSRCommand = new NotifyCommand(CreateAOSR, CanCreateAOSR).ObservesProperty(() => SelectedAOSRDocument);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            BuildingConstruction buildingConstruction = (BuildingConstruction)navigationContext.Parameters["building_construction"];
            if (SelectedBuildingConstruction == null || SelectedBuildingConstruction?.FullName != buildingConstruction.FullName)
            {
                SelectedBuildingConstruction = buildingConstruction;
                AOSRDocuments = SelectedBuildingConstruction.AOSRDocuments;
            }
        }
        private void CreateAOSR()
        {
            // ProjectService.SaveAOSRToWord(SelectedAOSRDocument);

        }
        private bool CanCreateAOSR()
        {
            return SelectedAOSRDocument != null && SelectedBuildingConstruction != null;
        }
        public NotifyCommand CreateAOSRCommand { get; private set; }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }


    }
}
