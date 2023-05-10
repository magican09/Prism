namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWork : IRegisterable, IMeasurable, ITemporal, ILaborIntensiveable, IDateable, ITask
    {

         bldMaterialsGroup Materials { get; }
         bldWorkArea WorkArea { get; set; }
         bool IsDone { get; set; }
         bldWorksGroup PreviousWorks { get; }
         bldWorksGroup NextWorks { get; }
         bldLaboratoryReportsGroup LaboratoryReports { get; }
         bldExecutiveSchemesGroup ExecutiveSchemes { get; }
        //  bldAOSRDocumentsGroup AOSRDocuments { get; set; }
         bldAOSRDocument AOSRDocument { get; set; }

    }
}
