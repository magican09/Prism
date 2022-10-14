using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ILaborIntensiveable
    {
        public decimal Laboriousness { get; set; }
        public decimal ScopeOfWork { get; set; }
    }
}
