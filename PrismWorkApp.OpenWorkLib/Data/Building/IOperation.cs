using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IOperation
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal LaborVolume { get; set; }
        public decimal LaborCost { get; set; }
    }
}
