using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProjectsGroup:NameableObservableCollection<bldProject>,IbldProjectsGroup, IEntityObject
    {

       
        public bldProjectsGroup()
        {
            Name= "Проекты";
        }
        public bldProjectsGroup(string name)
        {
            Name = name;
        }
        public bldProjectsGroup(List<bldProject> projects):base(projects)
        {

        }

    }
}
