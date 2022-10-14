using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldAOSRDocument:IbldDocument
    {
        bldResponsibleEmployeesGroup ResponsibleEmployees { get; set; }
    
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        bldWork bldWork { get; set; }
    }
}
