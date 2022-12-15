using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldMaterialCertificate : bldDocument, IbldMaterialCertificate, IEntityObject,IDateable,ITemporal
    {
        private string _materialName;
        public string MaterialName
        {
            get { return _materialName; }
            set { SetProperty(ref _materialName, value); }
        }
        
        private string _geometryParameters;
        public string GeometryParameters
        {
            get { return _geometryParameters; }
            set { SetProperty(ref _geometryParameters, value); }
        }
        private decimal _materialQuantity;
        public decimal MaterialQuantity
        {
            get { return _materialQuantity; }
            set { SetProperty(ref _materialQuantity, value); }
        }//Количесво 
        private string _unitsOfMeasure;
        public string UnitsOfMeasure
        {
            get { return _unitsOfMeasure; }
            set { SetProperty(ref _unitsOfMeasure, value); }
        }
        private string _controlingParament;
        public string ControlingParament
        {
            get { return _controlingParament; }
            set { SetProperty(ref _controlingParament, value); }
        }
        private string _regulationDocumentsName;
        public string RegulationDocumentsName
        {
            get { return _regulationDocumentsName; }
            set { SetProperty(ref _regulationDocumentsName, value); }
        }
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }//Дата начала
        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }//Дата окончания
        private DateTime? _netExecutionTime;
        public DateTime? NetExecutionTime
        {
            get { return _netExecutionTime; }
            set { SetProperty(ref _netExecutionTime, value); }
        }//Чистое время выполнения
    }
}
