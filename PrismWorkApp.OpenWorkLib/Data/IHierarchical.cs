using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchical
    {
          ObservableCollection<IEntityObject> Parents { get;  }
          ObservableCollection<IEntityObject> Children { get;  }
    }
}
