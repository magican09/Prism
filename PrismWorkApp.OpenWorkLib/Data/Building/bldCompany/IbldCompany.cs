namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldCompany:IRegisterable
    {
        string Contacts { get; set; }
        string INN { get; set; }
        string OGRN { get; set; }
        public string Address { get; set; }
    }
}