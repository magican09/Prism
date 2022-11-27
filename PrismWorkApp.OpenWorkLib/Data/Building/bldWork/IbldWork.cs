namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWork : IRegisterable, IMeasurable, ITemporal, ILaborIntensiveable, IDateable
    {

        public bldMaterialsGroup Materials { get; set; }
        public bldWorkArea WorkArea { get; set; }
        public bool IsDone { get; set; }
        public bldWorksGroup PreviousWorks { get; set; }
        public bldWorksGroup NextWorks { get; set; }
        public bldLaboratoryReportsGroup LaboratoryReports { get; set; }
        public bldExecutiveSchemesGroup ExecutiveSchemes { get; set; }
        public bldAOSRDocumentsGroup AOSRDocuments { get; set; }
    }
}
