using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldUnitOfMeasurementsGroup:NameableObservableCollection<bldUnitOfMeasurement>
    {
        public bldUnitOfMeasurementsGroup()
        {
            Name = "Ед.изм.:";
        }

        public bldUnitOfMeasurementsGroup(string name)
        {
            Name = name;
        }
        public bldUnitOfMeasurementsGroup(List<bldUnitOfMeasurement>  _list) : base( _list)
        {
            Name = "Ед.изм.:";
        }
        public bldUnitOfMeasurementsGroup(IEnumerable<bldUnitOfMeasurement>  _list) : base( _list)
        {
            Name = "Ед.изм.:";
        }
    }
}
