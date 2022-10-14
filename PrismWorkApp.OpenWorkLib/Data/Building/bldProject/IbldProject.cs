using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data

{
    public interface IbldProject: IRegisterable, ITemporal, IMeasurable, ILaborIntensiveable
            //, IHierarchicable<IbldProject,INameableOservableCollection<KeyValue> >
    {
        public string Address { get; set; }
        public bldObjectsGroup BuildingObjects { get; set; }
        public bldParticipantsGroup Participants { get; set; }
        public bldResponsibleEmployeesGroup ResponsibleEmployees { get; set; }
    }
}
