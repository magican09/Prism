using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResponsibleEmployee :BindableBase,IbldResponsibleEmployee, IEntityObject
    {
        private string _name;
        [NotMapped]
        public string Name
        {
            get { return Role.Name; }
            set {  }
        }
        private Employee _employee;
        public Employee Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }
        private string _nRSId;
        public string NRSId
        {
            get { return _nRSId; }
            set { SetProperty(ref _nRSId, value); }
        }
        private bldResponsibleEmployeeRole _role;
        public bldResponsibleEmployeeRole Role
        {
            get { return _role; }
            set
            {
                SetProperty(ref _role, value);
            }
        }
        private bldDocument _docConfirmingTheAthority;
        public bldDocument DocConfirmingTheAthority
        {
            get { return _docConfirmingTheAthority; }
            set { SetProperty(ref _docConfirmingTheAthority, value); }
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
        public bldParticipant bldParticipant { get; set; }
    }
}
