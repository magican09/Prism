using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProjectDocument : bldDocument, IbldProjectDocument, IEntityObject
    {
        public virtual ObservableCollection<bldWork> bldWorks { get; set; }
        public virtual ObservableCollection<bldConstruction> bldConstructions { get; set; }
        public virtual ObservableCollection<bldObject> bldOjects { get; set; }
        public virtual bldProject bldProject { get; set; }
    }
}
