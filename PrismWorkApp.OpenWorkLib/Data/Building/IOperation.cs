using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IOperation
    {
         DateTime StartTime { get; set; }
         DateTime EndTime { get; set; }
         decimal LaborVolume { get; set; }
         decimal LaborCost { get; set; }
    }
}
