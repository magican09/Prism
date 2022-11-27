namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldMaterial : IRegisterable, IMeasurable
    {
        public bldDocumentsGroup Documents { get; set; }

    }
}
