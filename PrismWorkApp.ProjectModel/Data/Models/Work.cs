
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
    public class Work :oldNode /*INotifyPropertyChanged,*/ 
    {
     /*   public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    */
        public int Id { get; set; }
        private string _code;
     
        public string Code { get { return _code; } set { _code = value; OnPropertyChanged("Code"); } }
        private string _name;
     
        public string Name { get { return _name; }
            set { _name = value; OnPropertyChanged("Name");NodeName = value; } }
        private string _fullname;

        public string FullName { get { return Name; }
            set { Name = value;
                OnPropertyChanged("FullName"); } } //Временно

        private double _quantity;
        public double Quantity { get { return _quantity; } set { _quantity = value; OnPropertyChanged("Quantity"); } }
     
        private string _measure;
        public string Measure { get { return _measure; } set { _measure = value; OnPropertyChanged("Measure"); } }
      
        private string _address;
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged("Address"); } }
     
        private DateTime _startDate;
        public DateTime StartDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("StartDate"); } }
      
        private DateTime _endDate;
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("EndDate"); } }
   
        private string _level;
        public string Level { get { return _level; } set { _level = value; OnPropertyChanged("Level"); } } //Отметка проведения работ
      
        private string _axes;
        public string Axes { get { return _axes; } set { _axes = value; OnPropertyChanged("Axes"); } } //Оси проведения работ 

        private ObservableCollection<AOSRDocument> _aOSRDocuments;
        public ObservableCollection<AOSRDocument> AOSRDocuments { get { return _aOSRDocuments; }
            set { _aOSRDocuments = value; OnPropertyChanged("AOSRDocuments"); } }
        private BuildingConstruction _buildingConstruction;
        public BuildingConstruction BuildingConstruction { get { return _buildingConstruction; } 
            set { _buildingConstruction = value; OnPropertyChanged("BuildingConstruction"); } }

        private ObservableCollection<Work> _nextWorks;
        public ObservableCollection<Work> NextWorks { get { return _nextWorks; } set { _nextWorks = value; OnPropertyChanged("NextWorks"); } }//Последующие работы 

        private ObservableCollection<Work> _previousWorks;
        public ObservableCollection<Work> PreviousWorks { get { return _previousWorks; } 
            set { _previousWorks = value; OnPropertyChanged("PreviousWorks"); } }//Последующие работы 
        private string _projectCode;
        public string ProjectCode { get { return _projectCode; } set { _projectCode = value; OnPropertyChanged("ProjectCode"); } }
      
        private ObservableCollection<Material> _materials;
        public ObservableCollection<Material> Materials { get { return _materials; } set { _materials = value; OnPropertyChanged("Materials"); } }
      
        private ObservableCollection<Document> _laboratoryReports;
        public ObservableCollection<Document> LaboratoryReports
        { get { return _laboratoryReports; } set { _laboratoryReports = value; OnPropertyChanged("LaboratoryReports"); } }
     
        private ObservableCollection<Document> _executiveSchemes;
        public ObservableCollection<Document> ExecutiveSchemes
        { get { return _executiveSchemes; } set { _executiveSchemes = value; OnPropertyChanged("ExecutiveSchemes"); } } //Испольнительные схемы
      
        private string _regulations;
        public string Regulations { get { return _regulations; } set { _regulations = value; OnPropertyChanged("Regulations"); } }//Нормативыне документы ГОСТы, СНИПы
      
        public Work(string name = "")
        {
            Materials = new ObservableCollection<Material>();
            LaboratoryReports = new ObservableCollection<Document>();
            ExecutiveSchemes = new ObservableCollection<Document>();
            NextWorks = new ObservableCollection<Work>();
            PreviousWorks = new ObservableCollection<Work>();
            AOSRDocuments = new ObservableCollection<AOSRDocument>();
            Name = name;
        }


    }
}
