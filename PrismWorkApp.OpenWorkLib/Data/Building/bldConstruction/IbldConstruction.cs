﻿namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstruction : IRegisterable, ITemporal, IMeasurable, ILaborIntensiveable//,IHierarchicable<IbldObject,IbldConstructionsGroup>
    {
        public bldWorksGroup Works { get; set; }
        public bldConstructionsGroup Constructions { get; set; }
    }
}
