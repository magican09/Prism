using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldWorksGroup : NameableObservableCollection<bldWork>, IbldWorksGroup
    {

        private bool _isDone;
        public bool IsDone
        {
            get { return _isDone; }
            set { SetProperty(ref _isDone, value); }
        }

        public bldWorksGroup()
        {
            Name = "Ведомость работ:";
        }

        public bldWorksGroup(string name)
        {
            Name = name;
        }
        public bldWorksGroup(List<bldWork> works_list) : base(works_list)
        {
            Name = "Ведомость работ:";
        }
        public bldWorksGroup(IEnumerable<bldWork> works_list) : base(works_list)
        {
            Name = "Ведомость работ:";
        }
    }
}
