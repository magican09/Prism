using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstructionsGroup : NameableObservableCollection<bldConstruction>, IbldConstructionsGroup, IEntityObject
    {
        public bldConstructionsGroup(List<bldConstruction> constructions) : base(constructions)
        {
            Name = "Список конструкции:";
        }
        public bldConstructionsGroup()
        {
            Name = "Список конструкции:";
        }
        public bldConstructionsGroup(string name)
        {
            Name = name;
        }
    }
}
