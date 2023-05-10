namespace PrismWorkApp.OpenWorkLib.Data

{
    public interface IbldProject : IRegisterable, ITemporal, IMeasurable, ILaborIntensiveable
    //, IHierarchicable<IbldProject,INameableOservableCollection<KeyValue> >
    {
         string Address { get; set; }
         bldObjectsGroup BuildingObjects { get; set; }
         bldParticipantsGroup Participants { get; set; }
         bldResponsibleEmployeesGroup ResponsibleEmployees { get; set; }
    }
}
