﻿using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldAggregationDocumentsGroup : NameableObservableCollection<bldAggregationDocument>,  IEntityObject
    {
        public bldAggregationDocumentsGroup()
        {
            Name = "Ведомости";
        }

        public bldAggregationDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldAggregationDocumentsGroup(List<bldAggregationDocument> _list) : base(_list)
        {
            Name = "Ведомости";
        }
    }
}