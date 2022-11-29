using System;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public interface IUnDoReDoSystem : INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
        public Guid Id { get; set; }
        bool AllUnDoIsDone();
        public bool IsSatcksEmpty();
        bool CanReDoExecute();
        bool CanUnDoExecute();
        bool ReDo(int levels);
        void UnRegister(IJornalable obj);
        void Register(IJornalable obj);
        bool UnDo(int levels);
        void UnDoAll();
        void ClearStacks();
        void AddUnDoReDo(IUnDoReDoSystem unDoReDo);



    }
}