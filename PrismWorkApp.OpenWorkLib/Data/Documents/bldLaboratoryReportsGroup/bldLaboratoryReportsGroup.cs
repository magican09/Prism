using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldLaboratoryReportsGroup: NameableObservableCollection<bldLaboratoryReport>, IbldLaboratoryReportsGroup, IEntityObject
    {
        public bldLaboratoryReportsGroup()
        {
            Name = "Лабораторные испытания";
        }

        public bldLaboratoryReportsGroup(string name)
        {
            Name = name;
        }
        public bldLaboratoryReportsGroup(List<bldLaboratoryReport> _list) : base(_list)
        {
            Name = "Лабораторные испытания";
        }
    }
}
