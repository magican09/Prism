namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ICostable
    {
          decimal UnitPrice { get; set; }//Цена за ед. 
          decimal Cost { get; set; }//Общая стоимость
    }
}
