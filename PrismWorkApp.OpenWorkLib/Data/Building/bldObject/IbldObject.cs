using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldObject: IRegisterable, ITemporal, IMeasurable, ILaborIntensiveable,
                                 ICloneable,IEntityObject
                            //IHierarchicable<IbldProject, INameableOservableCollection<KeyValue>>
    {
        public string Address { get; set; }
        public   bldConstructionsGroup Constructions { get; set; }
        public  bldObjectsGroup BuildingObjects { get;  set; }
    }
}
