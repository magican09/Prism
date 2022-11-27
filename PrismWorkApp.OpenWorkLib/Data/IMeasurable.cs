namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IMeasurable
    {
        public decimal Quantity { get; set; }//Количесво 
        public decimal UnitPrice { get; set; }//Цена за ед. 
        public bldUnitOfMeasurement UnitOfMeasurement { get; set; }//Ед. изм
        public decimal Cost { get; set; }//Общая стоимость
    }
}
