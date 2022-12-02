using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResponsibleEmployee : Employee, IbldResponsibleEmployee, IEntityObject
    {
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


        public bldParticipant bldParticipant { get; set; }
    }
}
