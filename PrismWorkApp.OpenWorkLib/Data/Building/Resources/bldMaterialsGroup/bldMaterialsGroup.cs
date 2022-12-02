﻿using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldMaterialsGroup : NameableObservableCollection<bldMaterial>, IbldMaterialsGroup, INotifyPropertyChanged, IEntityObject
    {

        public bldMaterialsGroup()
        {
            Name = "Материалы:";
        }
        public bldMaterialsGroup(string name)
        {
            Name = name;
        }
     
     
    }
}