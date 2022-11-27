
using PrismWorkApp.ProjectModel.Data.Models;
using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.ProjectModel.Data.Interfaces
{
    public interface IMaterial
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        double Quantity { get; set; }
        string Measure { get; set; }
        DateTime Date { get; set; }
        ObservableCollection<Document> Documents { get; set; }



    }
}
