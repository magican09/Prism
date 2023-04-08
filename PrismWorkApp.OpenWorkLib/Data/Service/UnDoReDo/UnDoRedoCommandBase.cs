using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public abstract class UnDoRedoCommandBase
    {
        public ObservableCollection<IJornalable> ChangedObjects { get; set; } = new ObservableCollection<IJornalable>();
    }
}
