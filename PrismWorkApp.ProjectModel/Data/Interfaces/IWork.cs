

using PrismWorkApp.ProjectModel.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.ProjectModel.Data.Interfaces
{
   public  interface IWork 
    {
         int Id { get; set; }
         string Name { get; set; }
        
        string Code { get; set; }
         double Quantity { get; set; }
        string Address { get; set; }
         DateTime StartDate { get; set; }
         DateTime EndDate { get; set; }
        string Level { get; set; } //Отметка проведения работ
        string Axes { get; set; } //Оси проведения работ 
        ObservableCollection<IWork> NextWorks { get; set; }//Последующие работы 
        string ProjectCode { get; set; }
        // ObservableCollection <IMaterial> Materials { get; set; }
        // ObservableCollection <ILaboratoryReport> Laboratories { get; set; }
         ObservableCollection<IMaterial> Materials { get; set; }
         ObservableCollection<Document> LaboratoryReports { get; set; }
         ObservableCollection<Document> ExecutiveSchemes { get; set; } //Испольнительные схемы

    }
}
