namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IMeasurable
    {
        public decimal Quantity { get; set; }//Количесво 
        public bldUnitOfMeasurement UnitOfMeasurement { get; set; }//Ед. изм
      
    }
}
