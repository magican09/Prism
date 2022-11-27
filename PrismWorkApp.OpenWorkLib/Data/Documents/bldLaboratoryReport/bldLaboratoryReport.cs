namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldLaboratoryReport : bldDocument, IbldLaboratoryReport, IEntityObject
    {
        public string LaboratoryReportPeeoperty { get; set; }
        public bldLaboratoryReport()
        {

        }
        public bldLaboratoryReport(string name) : base(name)
        {

        }
    }
}
