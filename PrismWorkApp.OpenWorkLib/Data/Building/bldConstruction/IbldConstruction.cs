namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstruction : ITemporal, IMeasurable, ILaborIntensiveable//,IHierarchicable<IbldObject,IbldConstructionsGroup>
    {
        public bldWorksGroup Works { get; set; }
        public bldConstructionsGroup Constructions { get; set; }
    }
}
