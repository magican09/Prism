using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstruction : BindableBase, IbldConstruction, ICloneable
    {

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
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
        private bldConstructionsGroup _constructions = new bldConstructionsGroup();
        public virtual bldConstructionsGroup Constructions
        {
            get { return _constructions; }
            set { SetProperty(ref _constructions, value); }
        }

        private bldWorksGroup _works = new bldWorksGroup();
        public virtual bldWorksGroup Works
        {
            get { return _works; }
            set { SetProperty(ref _works, value); }
        }

        private bldParticipantsGroup _participants;
        public bldParticipantsGroup? Participants
        {
            get
            {
                if (_participants != null) return _participants;
                if (ParentConstruction != null) return ParentConstruction.Participants;
                if (this.bldObject != null) return this.bldObject.Participants;
                return null;
            }
            set { SetProperty(ref _participants, value); }
        }
        private bldResponsibleEmployeesGroup? _responsibleEmployees;
        public bldResponsibleEmployeesGroup? ResponsibleEmployees
        {
            get
            {
                if (_responsibleEmployees != null) return _responsibleEmployees;
                if (this.bldObject != null) return bldObject.ResponsibleEmployees;
                return null;
            }
            set { SetProperty(ref _responsibleEmployees, value); }
        }
        private bldObject _bldObject;
        [NavigateProperty]
        public virtual bldObject? bldObject
        {
            get
            {
                if (_bldObject != null) return _bldObject;
                if (ParentConstruction != null) return ParentConstruction.bldObject;
                return null;
            }
            set { SetProperty(ref _bldObject, value); }
        }
        [NavigateProperty]
        public Guid? bldConstructionId { get; set; }
        [NavigateProperty]
        public bldConstruction? ParentConstruction { get; set; }
        private bldProject? _bldProject;
        public bldProject? bldProject
        {
            get
            {
                if (_bldProject != null) return _bldProject;
                if (bldObject != null) return bldObject.bldProject;
                if (ParentConstruction != null) return ParentConstruction.bldProject;
                return null;
            }
            set { SetProperty(ref _bldProject, value); }
        }
        private bldDocumentsGroup _documentation;
        public bldDocumentsGroup Documentation
        {
            get
            {
                if (_documentation != null) return _documentation;
                if (bldObject != null) return bldObject.Documentation;
                if (ParentConstruction != null) return ParentConstruction.Documentation;
                return null;
            }
            set { SetProperty(ref _documentation, value); }
        }
        #region Constructions
        public bldConstruction()
        {
           // Works.ParentObject = this;
            Works.Parent = this;

        }
        public bldConstruction(string name, string short_name)
        {
            Name = name;
            ShortName = short_name;
        }
        #endregion
        public object Clone()
        {
            throw new NotImplementedException();
        }
        public void SaveAOSRsToWord(string folderPath = null)
        {
            foreach (bldWork work in Works)
            {
                work.SaveAOSRsToWord(folderPath);
            }
            foreach (bldConstruction construction in Constructions)
            {
                string construction_folder_path = System.IO.Path.Combine(folderPath, construction.ShortName); ;
                System.IO.Directory.CreateDirectory(construction_folder_path);
                construction.SaveAOSRsToWord(construction_folder_path);
            }
        }
        #region EditMethods
        public void AddWork(bldWork work)
        {
            AddWorkToConstructionCommand Command = new AddWorkToConstructionCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddWorkGroup(ObservableCollection<bldWork> work)
        {
            AddWorkGroupToConstructionCommand Command = new AddWorkGroupToConstructionCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        public void AddConstruction(bldConstruction construction)
        {
            AddConstructionToConstructionCommand Command = new AddConstructionToConstructionCommand(this, construction);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        public void RemoveWork(bldWork work)
        {
            RemoveWorkCommand Command = new RemoveWorkCommand(this, work);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        //public void RemoveResponsibleEmployee(bldResponsibleEmployee empl)
        //{
        //    RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
        //         new RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
        //    InvokeUnDoReDoCommandCreatedEvent(Command);
        //}

        public void RemoveConstruction(bldConstruction constr)
        {
            RemoveFromCollectionCommand<bldConstructionsGroup, bldConstruction> Command =
                new RemoveFromCollectionCommand<bldConstructionsGroup, bldConstruction>(Constructions, constr);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        #endregion


    }
}
