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
        private RoleOfResponsible _roleOfResposible;
        public virtual RoleOfResponsible RoleOfResponsible
        {
            get { return _roleOfResposible; }
            set
            {

                switch (value)
                {
                    case RoleOfResponsible.CUSTOMER:
                        RoleFullName = "Представитель Заказчика";
                        RoleName = "Заказчик (застройщик)";

                        break;
                    case RoleOfResponsible.GENERAL_CONTRACTOR:
                        RoleFullName = "Представитель генподрядной организации";
                        RoleName = "Генподрядчик";

                        break;
                    case RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER:
                        RoleFullName = "Технадзор от генподрядчика";
                        RoleName = "Технадзор";
                        break;
                    case RoleOfResponsible.WORK_PERFORMER:
                        RoleFullName = "Представитель подрядной организации организации";
                        RoleName = "Подрядчик";
                        break;
                    case RoleOfResponsible.AUTHOR_SUPERVISION:
                        RoleFullName = "Представитель проектной организации";
                        RoleName = "Авторский надзор";
                        break;
                    case RoleOfResponsible.OTHER:
                        RoleFullName = "Иное лицо участвующей в процессе строительства.";
                        RoleName = "Иное лицо";
                        break;
                    default:
                        RoleFullName = "Роль не определена";
                        RoleName = "Не определено";
                        break;
                }
                SetProperty(ref _roleOfResposible, value);
            }
        }
        private string _roleName;
        [NotMapped]
        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }
        private string _roleFullName;
        [NotMapped]
        public string RoleFullName
        {
            get { return _roleFullName; }
            set { _roleFullName = value; }
        }

        private bldDocument _docConfirmingTheAthority;
        public bldDocument DocConfirmingTheAthority
        {
            get { return _docConfirmingTheAthority; }
            set { SetProperty(ref _docConfirmingTheAthority, value); }
        }
        [NavigateProperty]
        public bldProject bldProject { get; set; }
    //    public bldProjectsGroup bldProjects { get; set; }
        [NavigateProperty]
        public bldParticipant bldParticipant { get; set; }
     //   [NavigateProperty]
      //  public Guid bldParticipantId { get; set; }
    }
}
