namespace PrismWorkApp.ProjectModel.Data.Interfaces
{
    public interface ILaboratoryReport
    {
        int Id { get; set; }
        string Name { get; set; }
        string WorkName { get; set; }
        string NormativeDocument { get; set; }

        string Code { get; set; }
    }
}
