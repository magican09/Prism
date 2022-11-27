﻿using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public interface IUnDoReDoSystem : INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;

        bool AllUnDoIsDone();
        bool CanReDoExecute();
        bool CanUnDoExecute();
        void ReDo(int levels);
        void UnRegister(IJornalable obj);
        void Register(IJornalable obj);
        void UnDo(int levels);
        void UnDoAll();
        void ClearStacks();
    }
}