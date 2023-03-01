using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchical
    {
        public IBindableBase Parent { get; set; }
        public ObservableCollection<IBindableBase> Children { get; set; }
    }
}
