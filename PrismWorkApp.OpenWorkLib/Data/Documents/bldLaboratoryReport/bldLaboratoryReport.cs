namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldLaboratoryReport : bldDocument, IbldLaboratoryReport, IEntityObject, IbldDocument
    {
        public string LaboratoryReportPeeoperty { get; set; }
        public bldLaboratoryReport()
        {
            Code = "лаб_исп";
        }
        public bldLaboratoryReport(string name) : base(name)
        {
            Code = "лаб_исп";
        }
    }
}
