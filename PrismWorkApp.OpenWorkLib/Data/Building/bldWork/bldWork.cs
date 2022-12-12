using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldWork : BindableBase, IbldWork, ICloneable, IEntityObject//, IJornalable
    {


        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _shortName;
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private decimal _quantity;
        public decimal Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }//Количесво 
        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }//Цена за ед. 
        private bldUnitOfMeasurement _unitOfMeasurement;
        public virtual bldUnitOfMeasurement UnitOfMeasurement
        {
            get { return _unitOfMeasurement; }
            set { SetProperty(ref _unitOfMeasurement, value); }
        }//Ед. изм
        private decimal _cost;
        public decimal Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }//Общая стоимость

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }//Дата начала
        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }//Дата окончания
        private DateTime? _netExecutionTime;
        public DateTime? NetExecutionTime
        {
            get { return _netExecutionTime; }
            set { SetProperty(ref _netExecutionTime, value); }
        }//Чистое время выполнения

        private decimal _laboriousness;
        public decimal Laboriousness
        {
            get { return _laboriousness; }
            set { SetProperty(ref _laboriousness, value); }
        }//Трудоемкость  чел.час/ед.изм
        private decimal _scopeOfWork;
        public decimal ScopeOfWork
        {
            get { return _scopeOfWork; }
            set { SetProperty(ref _scopeOfWork, value); }
        }

        private bldMaterialsGroup _materials = new bldMaterialsGroup("Использованные материалы (МР)");
        public virtual bldMaterialsGroup Materials
        {
            get { return _materials; }
            set { SetProperty(ref _materials, value); }
        }//Используемые материала


        private bldWorkArea _workArea;
        public virtual bldWorkArea WorkArea
        {
            get { return _workArea; }
            set { SetProperty(ref _workArea, value); }
        }
        private bool _isDone;
        public bool IsDone
        {
            get { return _isDone; }
            set { SetProperty(ref _isDone, value); }
        }
        private bldWorksGroup _previousWorks = new bldWorksGroup("Предыдущие работы");
        public bldWorksGroup PreviousWorks
        {
            get { return _previousWorks; }
            set { SetProperty(ref _previousWorks, value); }
        }
        private bldWorksGroup _nextWorks = new bldWorksGroup("Последующие работы");
        public bldWorksGroup NextWorks
        {
            get { return _nextWorks; }
            set { SetProperty(ref _nextWorks, value); }
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
        //private bldAOSRDocumentsGroup _aOSRDocuments = new bldAOSRDocumentsGroup("Акты АОСР");
        //public bldAOSRDocumentsGroup AOSRDocuments
        //{
        //    get { return _aOSRDocuments; }
        //    set { SetProperty(ref _aOSRDocuments, value); }
        //}
        private bldAOSRDocument _aOSRDocument;
        public bldAOSRDocument AOSRDocument
        {
            get { return _aOSRDocument; }
            set { SetProperty(ref _aOSRDocument, value); }
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

        private bldWorkExecutiveDocumentation _executiveDocumentation;
        public bldWorkExecutiveDocumentation? ExecutiveDocumentation
        {
            get
            {
                return _executiveDocumentation;
            }
            set { SetProperty(ref _executiveDocumentation, value); }
        }
        private bldParticipantsGroup _participants;
        public bldParticipantsGroup? Participants
        {
            get
            {
                if (_participants != null) return _participants;
                if (this.bldConstruction != null) return this.bldConstruction.Participants;
                return null;
            }
            set { SetProperty(ref _participants, value); }
        }

        [NavigateProperty]
        public bldConstruction bldConstruction { get; set; }

        public bldWork()
        {
            PreviousWorks.CopingEnable = false; //отключаем при копировании
            NextWorks.CopingEnable = false; //отключаем при копировании 
            AOSRDocument = new bldAOSRDocument();
            WorkArea = new bldWorkArea();
           //        RestrictionPredicate = x => x.CopingEnable;// Определяет условтия я своего копировани в время глубокого копирования рефлексией 
        }

        public object Clone()
        {
            bldWork new_work = (bldWork)this.MemberwiseClone();
            new_work.Id = Guid.Empty;
            var prop_infoes = new_work.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop_info in prop_infoes)
            {
                if (!prop_info.PropertyType.FullName.Contains("System"))
                {
                    var prop_val = prop_info.GetValue(new_work);

                    if (prop_val != null)
                    {
                        prop_val = null;
                        prop_val = Activator.CreateInstance(prop_info.PropertyType);
                        prop_info.SetValue(new_work, prop_val);
                    }
                }
            }
            return new_work;
        }
        private bldDocumentsGroup _documentation;
        [NotMapped]
        public bldDocumentsGroup Documentation
        {
            get
            {
                if (_documentation != null) return _documentation;
                if (bldConstruction != null) return bldConstruction.Documentation;
                return null;
            }
            set { SetProperty(ref _documentation, value); }
        }
        public void SaveAOSRsToWord(string folderPath)
        {
            //if (AOSRDocuments.Count > 1)
            //{
            //    folderPath = System.IO.Path.Combine(folderPath, this.Name);
            //    System.IO.Directory.CreateDirectory(folderPath);
            //}
            //foreach (bldAOSRDocument aOSRDocument in AOSRDocuments)
            //{

            //    aOSRDocument.SaveAOSRToWord(folderPath);
            //}
            if (AOSRDocument != null) AOSRDocument.SaveAOSRToWord(folderPath);
        }

        #region EditMethods
        public void RemovePreviousWork(bldWork work)
        {
            RemovePreviousWorkCommand Command = new RemovePreviousWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveNextWork(bldWork work)
        {
            RemoveNextWorkCommand Command = new RemoveNextWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddPreviousWork(bldWork work)
        {
            AddPreviousWorkCommand Command =
                 new AddPreviousWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddNextWork(bldWork work)
        {
            AddNextWorkCommand Command =
                new AddNextWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddMaterial(bldMaterial material)
        {
            AddMaterialCommand Command = new AddMaterialCommand(this, material);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveMaterial(bldMaterial material)
        {
            RemoveMaterialCommand Command = new RemoveMaterialCommand(this, material);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddProjectDocument(bldProjectDocument document)
        {
            AddToCollectionCommand<ICollection<bldProjectDocument>, bldProjectDocument> Command =
                new AddToCollectionCommand<ICollection<bldProjectDocument>, bldProjectDocument>(this.ProjectDocuments, document);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveProjectDocument(bldProjectDocument document)
        {
            RemoveFromCollectionCommand<ICollection<bldProjectDocument>, bldProjectDocument> Command =
               new RemoveFromCollectionCommand<ICollection<bldProjectDocument>, bldProjectDocument>(this.ProjectDocuments, document);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddLaboratoryReport(bldLaboratoryReport document)
        {
            AddLaboratoryReportCommand Command =
                 new AddLaboratoryReportCommand(this, document);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveLaboratoryReport(bldLaboratoryReport document)
        {
            RemoveLaboratoryReportCommand Command =
               new RemoveLaboratoryReportCommand(this, document);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddExecutiveScheme(bldExecutiveScheme scheme)
        {

            AddExecutiveSchemeCommand Command =
                new AddExecutiveSchemeCommand(this, scheme);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveExecutiveScheme(bldExecutiveScheme scheme)
        {
            RemoveExecutiveSchemeCommand Command =
                new RemoveExecutiveSchemeCommand(this, scheme);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        #endregion


    }
}
