using System;

namespace PrismWorkApp.ProjectModel.Data.Interfaces
{
    public interface IProject
    {
        int Id { get; set; }
        string Name { get; set; }
        string ShortName { get; set; }
        string Address { get; set; }
        DateTime StartDate { get; set; }
        //     ObservableCollection<IProject> SubProjects { get; set; }

        //ObservableCollection <IWork> Works { get; set; }

    }
}
