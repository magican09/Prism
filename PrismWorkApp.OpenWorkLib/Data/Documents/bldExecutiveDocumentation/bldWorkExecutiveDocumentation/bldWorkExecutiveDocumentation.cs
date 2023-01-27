using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public class bldWorkExecutiveDocumentation : BindableBase, INameable, IbldWorkExecutiveDocumentation
    {
        private string _name ="Исполнительная документация";
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public bldWorkExecutiveDocumentation()
        {
            Name = "Исполнительная документация";
        }

        public bldWorkExecutiveDocumentation(string name)
        {
            Name = name;
        }
        private bldLaboratoryReportsGroup _laboratoryReports = new bldLaboratoryReportsGroup("Лабораторные испытания");
        public bldLaboratoryReportsGroup LaboratoryReports
        {
            get { return _laboratoryReports; }
            set { SetProperty(ref _laboratoryReports, value); }
        }
        private bldExecutiveSchemesGroup _executiveSchemes = new bldExecutiveSchemesGroup("Исполнительные схемы");
        public bldExecutiveSchemesGroup ExecutiveSchemes
        {
            get { return _executiveSchemes; }
            set { SetProperty(ref _executiveSchemes, value); }
        }
        private bldAOSRDocumentsGroup _aOSRDocuments = new bldAOSRDocumentsGroup();
        public bldAOSRDocumentsGroup AOSRDocuments
        {
            get { return _aOSRDocuments; }
            set { SetProperty(ref _aOSRDocuments, value); }
        }
        private bldProjectDocumentsGroup _projectDocuments = new bldProjectDocumentsGroup("Рабочая документация");
        public bldProjectDocumentsGroup ProjectDocuments
        {
            get { return _projectDocuments; }
            set { SetProperty(ref _projectDocuments, value); }
        }
        private bldRegulationtDocumentsGroup _regulationDocuments = new bldRegulationtDocumentsGroup("Нормативная документация");
        public bldRegulationtDocumentsGroup RegulationDocuments
        {
            get { return _regulationDocuments; }
            set { SetProperty(ref _regulationDocuments, value); }
        }
    }
}
