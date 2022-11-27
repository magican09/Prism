namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWorkArea : IRegisterable
    {
        string Axes { get; set; }
        string Levels { get; set; }
    }
}