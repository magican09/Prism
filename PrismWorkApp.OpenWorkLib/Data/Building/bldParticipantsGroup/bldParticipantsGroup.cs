using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipantsGroup : NameableObservableCollection<bldParticipant>, IbldParticipantsGroup, IEntityObject
    {
       
       public bldParticipantsGroup()
        {
            Name = "Участники строительства";
        }

        public object Clone()
        {
            return (bldParticipantsGroup)MemberwiseClone();
        }
        public bldParticipantsGroup(List<bldParticipant> participants):base(participants)
        {

        }
    }
}
