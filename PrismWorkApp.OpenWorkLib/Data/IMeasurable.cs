namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IMeasurable
    {
          decimal Quantity { get; set; }//Количесво 
          bldUnitOfMeasurement UnitOfMeasurement { get; set; }//Ед. изм

    }
}
