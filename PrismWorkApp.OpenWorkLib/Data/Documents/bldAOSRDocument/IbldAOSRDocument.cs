using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldAOSRDocument : IbldDocument
    {
        bldResponsibleEmployeesGroup ResponsibleEmployees { get; set; }

          DateTime StartTime { get; set; }

          DateTime? EndTime { get; set; }

        bldWork bldWork { get; set; }
    }
}
