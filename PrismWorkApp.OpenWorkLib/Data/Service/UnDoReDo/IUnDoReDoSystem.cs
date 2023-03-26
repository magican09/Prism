using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoReDoSystem : INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
        public Guid Id { get; set; }
        bool AllUnDoIsDone();
        public bool IsSatcksEmpty();
        bool CanReDoExecute();
        bool CanUnDoExecute();
        void UnRegister(IJornalable obj);
        void Register(IJornalable obj);
        bool UnDo(int levels, bool without_redo = false);
        bool ReDo(int levels, bool without_undo = false);
        void UnDoAll();
        void ClearStacks();
        void AddUnDoReDo(IUnDoReDoSystem unDoReDo);
        public void SetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system);
        public void UnSetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system);
        public IUnDoReDoSystem ParentUnDoReDo { get; set; }
        public ObservableCollection<IJornalable> _RegistedModels { get; set; }


    }
}