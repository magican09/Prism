using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.OpenWorkLib.Services;
using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProject : BindableBase, IbldProject, IEntityObject//, IJornalable
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

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }//Дата начала
        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }//Дата окончания
        private DateTime _netExecutionTime;
        public DateTime NetExecutionTime
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
        public bldUnitOfMeasurement UnitOfMeasurement
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


        private bldObjectsGroup _buildingObjects;
        public bldObjectsGroup? BuildingObjects
        {
            get { return _buildingObjects; }
            set { SetProperty(ref _buildingObjects, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
        private bldParticipantsGroup _participants = new bldParticipantsGroup();

        public bldParticipantsGroup? Participants
        {
            get { return _participants; }
            set { SetProperty(ref _participants, value); }
        }

       private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
       public bldResponsibleEmployeesGroup? ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }

        public object Clone()
        {
            // return CloningService.Clone<bldProject>(this);
            bldProject project = (bldProject)MemberwiseClone();
            project.BuildingObjects = (bldObjectsGroup)BuildingObjects.Clone();
            project.Participants = (bldParticipantsGroup)Participants.Clone();
            project.ResponsibleEmployees = (bldResponsibleEmployeesGroup)ResponsibleEmployees.Clone();
            return project;
        }

        public bldProject()
        {
        }

        #region EditMethods
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
        public void RemoveResponsibleEmployee(bldResponsibleEmployee empl)
        {
            RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                 new RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddBuildindObject(bldObject obj)
        {
            obj.bldProject = this;
            AddToCollectionCommand<bldObjectsGroup, bldObject> Command =
                new AddToCollectionCommand<bldObjectsGroup, bldObject>(BuildingObjects, obj);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddParticipant(bldParticipant participant)
        {
            AddToCollectionCommand<bldParticipantsGroup, bldParticipant> Command =
                 new AddToCollectionCommand<bldParticipantsGroup, bldParticipant>(Participants, participant);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddResponsibleEmployee(bldResponsibleEmployee empl)
        {
            AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                 new AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        #endregion
    }
}
