using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldParticipantsGroup : NameableObservableCollection<bldParticipant>, IbldParticipantsGroup, IEntityObject
    {

        public bldParticipantsGroup()
        {
            Name = "Участники строительства";
        }
        public bldParticipantsGroup(string name)
        {
            Name = name;
        }
        public object Clone()
        {
            return (bldParticipantsGroup)MemberwiseClone();
        }
        public bldParticipantsGroup(List<bldParticipant> participants) : base(participants)
        {

        }
    }
}
