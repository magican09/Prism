using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldObject : IRegisterable, INameable, ITemporal, IMeasurable, ILaborIntensiveable,
                                 ICloneable, IEntityObject
    //IHierarchicable<IbldProject, INameableOservableCollection<KeyValue>>
    {
         string Address { get; set; }
         bldConstructionsGroup Constructions { get; set; }
         bldObjectsGroup BuildingObjects { get; set; }
    }
}
