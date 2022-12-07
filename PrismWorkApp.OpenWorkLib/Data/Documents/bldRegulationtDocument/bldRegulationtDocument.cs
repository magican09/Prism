using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldRegulationtDocument : bldDocument, IbldRegulationtDocument, IEntityObject
    {

        public virtual ObservableCollection<bldWork> bldWorks { get; set; }
    }
}
