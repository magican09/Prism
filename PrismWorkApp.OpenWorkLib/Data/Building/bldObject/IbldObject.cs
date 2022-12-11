using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldObject : IRegisterable,INameable, ITemporal, IMeasurable, ILaborIntensiveable,
                                 ICloneable, IEntityObject
    //IHierarchicable<IbldProject, INameableOservableCollection<KeyValue>>
    {
        public string Address { get; set; }
        public bldConstructionsGroup Constructions { get; set; }
        public bldObjectsGroup BuildingObjects { get; set; }
    }
}
