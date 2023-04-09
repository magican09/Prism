using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public abstract class UnDoRedoCommandBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ObservableCollection<IJornalable> ChangedObjects { get; set; } = new ObservableCollection<IJornalable>();
       
        public IUnDoReDoSystem UnDoReDo_System { get; protected set; }

    }
}
