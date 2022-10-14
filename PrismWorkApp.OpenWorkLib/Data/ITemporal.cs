using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ITemporal
    {
        public DateTime StartTime { get; set; }//Дата начала
        public DateTime EndTime { get; set; }//Дата окончания
        public DateTime NetExecutionTime { get; set; }//Чистое время выполнения
    }
}
