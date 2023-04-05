using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProjectsGroup : NameableObservableCollection<bldProject>, IbldProjectsGroup
    {


        public bldProjectsGroup()
        {
            Name = "Проекты";
        }
        public bldProjectsGroup(string name) : this()
        {
            Name = name;
        }
        public bldProjectsGroup(List<bldProject> projects) : base(projects)
        {

        }

    }
}
