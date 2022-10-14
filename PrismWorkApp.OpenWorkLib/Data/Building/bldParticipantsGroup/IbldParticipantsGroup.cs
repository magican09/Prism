using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldParticipantsGroup: INotifyCollectionChanged, IEnumerable<bldParticipant>, IList<bldParticipant>,INameable,
                                                                  ICollection<bldParticipant>,ICloneable
    {
       
    }
}
