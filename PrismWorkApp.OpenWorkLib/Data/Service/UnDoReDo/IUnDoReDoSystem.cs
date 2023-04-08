using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoReDoSystem : INotifyPropertyChanged, IUnDoRedoCommand
    {
        event PropertyChangedEventHandler PropertyChanged;
        public Guid Id { get; set; }
        bool IsAllUnDoIsDone();
        public bool IsSatcksEmpty();
        bool CanReDoExecute();
        bool CanUnDoExecute();
        void UnRegister(IJornalable obj);
        public void UnRegisterAll(IJornalable obj, bool first_itaration = true);
        void Register(IJornalable obj);
        public void RegisterAll(IJornalable obj, bool first_itaration = true);
       bool UnDo(int levels, bool without_redo = false);
        bool ReDo(int levels, bool without_undo = false);
        void UnDoAll();
        void ClearStacks();
        void AddUnDoReDo(IUnDoReDoSystem unDoReDo);
        public void SetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system);
        public void UnSetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system);
        public IUnDoReDoSystem ParentUnDoReDo { get; set; }
        public Dictionary<IJornalable, IUnDoReDoSystem> _RegistedModels { get; set; }
        public Dictionary<IJornalable, IUnDoReDoSystem> _ChildrenSystemRegistedModels { get; set; } 
        public int SaveChages(IJornalable obj);
        public int SaveAllChages(IJornalable obj, bool first_itaration = true);



    }
}