namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldDocument : IRegisterable
    {
        public string FullName { get; set; }
        public int PagesNumber { get; set; }
        public string RegId { get; set; }
    }
}
