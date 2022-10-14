namespace PrismWorkApp.ProjectModel.Data.Interfaces
{
    public interface IEmployee
    {
        int Id { get; set; }
        string FullName { get; set; }
        int Number { get; set; }
        IEmployeePosition EmployeePosition { get; set; }

    }
}
