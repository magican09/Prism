using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ITemporal
    {
          DateTime StartTime { get; set; }//Дата начала
          DateTime? EndTime { get; set; }//Дата окончания
          DateTime? NetExecutionTime { get; set; }//Чистое время выполнения
    }
}
