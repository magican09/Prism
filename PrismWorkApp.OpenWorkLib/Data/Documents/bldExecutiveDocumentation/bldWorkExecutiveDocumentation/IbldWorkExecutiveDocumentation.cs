namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWorkExecutiveDocumentation
    {
        bldAOSRDocumentsGroup AOSRDocuments { get; set; }
        bldExecutiveSchemesGroup ExecutiveSchemes { get; set; }
        bldLaboratoryReportsGroup LaboratoryReports { get; set; }
        string Name { get; set; }
        bldProjectDocumentsGroup ProjectDocuments { get; set; }
        bldRegulationtDocumentsGroup RegulationDocuments { get; set; }
    }
}