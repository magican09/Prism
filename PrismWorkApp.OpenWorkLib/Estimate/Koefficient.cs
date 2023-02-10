using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Estimate
{
    public class Koefficient //Коэффициенты 
    {
        public int Id { get; set; }
        public double Value_OZ { get; set; } //  Общий коэфицент
        public double Value_EM { get; set; } // Коэфицент к ЭМ
        public int Level { get; set; } // Уровень коэффицент
    }
}
