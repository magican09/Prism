using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldParticipant : IRegisterable, ICloneable
    {
        // bldConstructionCompany Company { get; set; }
     //   public bldConstructionCompanyGroup ConstructionCompanies { get; set; }
        bldResponsibleEmployeesGroup ResponsibleEmployees { get; set; }
        ParticipantRole Role { get; set; }
        public string RoleName { get; set; }
        public string RoleFullName { get; set; }
    }
}