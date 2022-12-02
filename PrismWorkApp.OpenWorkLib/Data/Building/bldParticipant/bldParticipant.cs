using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipant : BindableBase, IbldParticipant, IEntityObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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
    }
}
