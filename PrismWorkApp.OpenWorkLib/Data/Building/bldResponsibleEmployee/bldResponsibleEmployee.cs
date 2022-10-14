using System;
using System.Collections.Generic;
using System.Text;

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
        private RoleOfResponsible _roleOfResposible;
        public virtual  RoleOfResponsible RoleOfResponsible
        {
            get { return _roleOfResposible; }
            set { SetProperty(ref _roleOfResposible, value); }
        }
        private bldDocument _docConfirmingTheAthority;
        public   bldDocument DocConfirmingTheAthority
        {
            get { return _docConfirmingTheAthority; }
            set { SetProperty(ref _docConfirmingTheAthority, value); }
        }
       
        private bldCompany _company;
        [NavigateProperty]
        public  bldCompany Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }
        [NavigateProperty]
        public bldProject bldProject { get; set; }
        [NavigateProperty]
        public bldParticipant bldParticipant { get; set;}
    }
}
