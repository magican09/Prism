using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldObject : BindableBase, IbldObject, IEntityObject, IJornalable, INameable
    {

        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }

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
        public virtual bldConstructionsGroup? Constructions
        {
            get { return _constructions; }
            set { SetProperty(ref _constructions, value); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
        private bldObjectsGroup _buildingObjects = new bldObjectsGroup();
        public virtual bldObjectsGroup BuildingObjects
        {
            get { return _buildingObjects; }
            set { SetProperty(ref _buildingObjects, value); }
        }
        #region Construcctions 
        public bldObject()
        {
            BuildingObjects.Owner = this;
            Constructions.Owner = this;
        }
        public bldObject(string name, string short_name)
        {
            Name = name;
            ShortName = short_name;
            BuildingObjects.Owner = this;
            Constructions.Owner = this;
        }
        #endregion

        private bldProject _bldProject;
        [NavigateProperty]
        public virtual bldProject? bldProject
        {
            get
            {
                if (_bldProject != null) return _bldProject;
                if (ParentObject != null) return ParentObject.bldProject;
                return null;
            }
            set { SetProperty(ref _bldProject, value); }
        }
        [NavigateProperty]
        public Guid? bldObjectId { get; set; }
        [NavigateProperty]
        public bldObject? ParentObject { get; set; }
        #region IClonable
        public override bool Equals(object? obj)
        {
            if (obj is IEntityObject)
            {
                if (((IEntityObject)obj).Id == this.Id && this.Id != Guid.Empty) return true;
                if (((IEntityObject)obj).StoredId == this.StoredId && this.StoredId != Guid.Empty) return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            if (this.Id != Guid.Empty) return this.Id.GetHashCode();
            if (this.StoredId != Guid.Empty) return this.Id.GetHashCode();
            return this.Id.GetHashCode();
        }
        public object Clone()
        {
            bldObject val = new bldObject();
            val = (bldObject)MemberwiseClone();
            return val;
        }
        #endregion

        private bldDocumentsGroup _documentation;
        public bldDocumentsGroup Documentation
        {
            get
            {
                if (_documentation != null) return _documentation;
                if (this.bldProject != null) return this.bldProject.Documentation;

                return null;
            }
            set { SetProperty(ref _documentation, value); }
        }

        private bldParticipantsGroup _participants;
        public bldParticipantsGroup Participants
        {
            get
            {
                if (_participants != null) return _participants;
                if (this.bldProject != null) return this.bldProject.Participants;
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
                if (this.bldProject != null) return bldProject.ResponsibleEmployees;

                return null;
            }
            set { SetProperty(ref _responsibleEmployees, value); }
        }
        public void SaveAOSRsToWord(string folderPath = null)
        {
            foreach (bldConstruction construction in Constructions)
            {
                string construction_folder_path = System.IO.Path.Combine(folderPath, construction.ShortName); ;
                System.IO.Directory.CreateDirectory(construction_folder_path);
                construction.SaveAOSRsToWord(construction_folder_path);
            }
            foreach (bldObject bld_object in BuildingObjects)
            {
                string bld_folder_path = System.IO.Path.Combine(folderPath, bld_object.ShortName); ;
                System.IO.Directory.CreateDirectory(bld_folder_path);
                bld_object.SaveAOSRsToWord(bld_folder_path);
            }

        }

        #region EditMethods
        public void RemoveConstruction(bldConstruction constr)
        {
            RemoveFromCollectionCommand<bldConstructionsGroup, bldConstruction> Command =
                new RemoveFromCollectionCommand<bldConstructionsGroup, bldConstruction>(Constructions, constr);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveBuildindObject(bldObject obj)
        {
            RemoveFromCollectionCommand<bldObjectsGroup, bldObject> Command =
                new RemoveFromCollectionCommand<bldObjectsGroup, bldObject>(BuildingObjects, obj);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void RemoveParticipant(bldParticipant participant)
        {
            RemoveFromCollectionCommand<bldParticipantsGroup, bldParticipant> Command =
                 new RemoveFromCollectionCommand<bldParticipantsGroup, bldParticipant>(Participants, participant);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        //public void RemoveResponsibleEmployee(bldResponsibleEmployee empl)
        //{
        //    RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
        //         new RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
        //    InvokeUnDoReDoCommandCreatedEvent(Command);
        //}
        public void AddBuildindObject(bldObject obj)
        {
            AddObjectToObjectCommand Command = new AddObjectToObjectCommand(this, obj);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddConstruction(bldConstruction construction)
        {
            AddConstructionToObjectCommand Command = new AddConstructionToObjectCommand(this, construction);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddParticipant(bldParticipant participant)
        {
            AddToCollectionCommand<bldParticipantsGroup, bldParticipant> Command =
                 new AddToCollectionCommand<bldParticipantsGroup, bldParticipant>(Participants, participant);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        //public void AddResponsibleEmployee(bldResponsibleEmployee empl)
        //{
        //    AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
        //         new AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
        //    InvokeUnDoReDoCommandCreatedEvent(Command);
        //}

        #endregion

    }
}
