namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ICostable
    {
        public decimal UnitPrice { get; set; }//Цена за ед. 
        public decimal Cost { get; set; }//Общая стоимость
    }
}
