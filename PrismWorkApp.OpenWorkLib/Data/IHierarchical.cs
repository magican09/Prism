using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchical
    {
        public ObservableCollection<IEntityObject> Parents { get;  }
        public ObservableCollection<IEntityObject> Children { get;  }
    }
}
