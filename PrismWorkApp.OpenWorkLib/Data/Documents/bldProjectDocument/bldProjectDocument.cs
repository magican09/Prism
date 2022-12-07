using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProjectDocument : bldDocument, IbldProjectDocument, IEntityObject
    {
        public virtual ObservableCollection<bldWork> bldWorks { get; set; }
    }
}
