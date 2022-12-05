using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipant : BindableBase, IbldParticipant, IEntityObject
    {
        private string _name;
        [NotMapped]
        public string Name
        {
            get { return Role.Name; }
            set { } 
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
       
        private bldConstructionCompany _constructionCompany;
        public bldConstructionCompany ConstructionCompany
        {
            get { return _constructionCompany; }
            set { SetProperty(ref _constructionCompany, value); }
        }
 
       
        private bldParticipantRole _role; 
        public bldParticipantRole Role
        {
            get { return _role; }
            set
            {
                SetProperty(ref _role, value);
            }
        }
        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
        public bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }



        public bldObjectsGroup BuildingObjects { get; set; }
        public bldConstructionsGroup Constructions { get; set; }
        public bldWorksGroup Works { get; set; }
        [NavigateProperty]
        public bldProject bldProject { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        #region EditMethods
        public void RemoveResponsibleEmployee(bldResponsibleEmployee empl)
        {
            RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                 new RemoveFromCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        public void AddResponsibleEmployee(bldResponsibleEmployee empl)
        {
            AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                 new AddToCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees, empl);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }

        public void CleareResponsibleEmployees()
        {
            ClearCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee> Command =
                new ClearCollectionCommand<bldResponsibleEmployeesGroup, bldResponsibleEmployee>(ResponsibleEmployees);
            InvokeUnDoReDoCommandCreatedEvent(Command);
        }
        #endregion
    }
}
