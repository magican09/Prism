using System;

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

        private bldMaterialsGroup _materials = new bldMaterialsGroup("МР:");
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

        private bldAOSRDocumentsGroup _aOSRDocuments = new bldAOSRDocumentsGroup("Акты АОСР");
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
            RestrictionPredicate = x => x.CopingEnable;// Определяет условтия я своего копировани в время глубокого копирования рефлексией 
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        public void SaveAOSRsToWord(string folderPath)
        {
            if (AOSRDocuments.Count > 1)
            {
                folderPath = System.IO.Path.Combine(folderPath, this.Name);
                System.IO.Directory.CreateDirectory(folderPath);
            }
            foreach (bldAOSRDocument aOSRDocument in AOSRDocuments)
            {
                
                aOSRDocument.SaveAOSRToWord(folderPath);
            }
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
                new AddPreviousWorkCommand(this,work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddNextWork(bldWork work)
        {
            AddNextWorkCommand Command =
                new AddNextWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        #endregion


    }
}
