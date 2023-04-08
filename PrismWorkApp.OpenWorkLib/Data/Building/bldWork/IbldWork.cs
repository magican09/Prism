namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWork : IRegisterable, IMeasurable, ITemporal, ILaborIntensiveable, IDateable, ITask
    {

        public bldMaterialsGroup Materials { get; }
        public bldWorkArea WorkArea { get; set; }
        public bool IsDone { get; set; }
        public bldWorksGroup PreviousWorks { get; }
        public bldWorksGroup NextWorks { get; }
        public bldLaboratoryReportsGroup LaboratoryReports { get; }
        public bldExecutiveSchemesGroup ExecutiveSchemes { get; }
        // public bldAOSRDocumentsGroup AOSRDocuments { get; set; }
        public bldAOSRDocument AOSRDocument { get; set; }

    }
}
