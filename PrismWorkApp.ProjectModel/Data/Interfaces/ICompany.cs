namespace PrismWorkApp.ProjectModel.Data.Interfaces
{
    public interface ICompany
    {
        int Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string OGRN { get; set; }
        string INN { get; set; }
        string Contacts { get; set; }
    }
}
