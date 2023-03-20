using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchical
    {
        public BindableBase Parent { get; set; }
        public ObservableCollection<BindableBase> Children { get; set; }
    }
}
