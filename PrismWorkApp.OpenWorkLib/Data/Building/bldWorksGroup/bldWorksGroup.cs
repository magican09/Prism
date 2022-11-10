using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public class bldWorksGroup : NameableObservableCollection<bldWork>, IbldWorksGroup, IEntityObject
    {
       
        private bool _isDone;
        public bool IsDone
        {
            get {  return _isDone; }
            set {  _isDone= value; OnPropertyChanged("IsDone"); }
        }
       
        public bldWorksGroup()
        {
            Name = "Ведомость работ:";
        }

        public bldWorksGroup( string name)
        {
            Name = name;
        }
        public bldWorksGroup(List<bldWork> works_list):base(works_list)
        {
            Name = "Ведомость работ:";
        }

    }
}
