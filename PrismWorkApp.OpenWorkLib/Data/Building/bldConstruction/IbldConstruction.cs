namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstruction : ITemporal, IMeasurable, ILaborIntensiveable//,IHierarchicable<IbldObject,IbldConstructionsGroup>
    {
         bldWorksGroup Works { get; set; }
         bldConstructionsGroup Constructions { get; set; }
    }
}
