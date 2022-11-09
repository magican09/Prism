using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IPropertiesChangeJornal:INotifyPropertyChanged, ICuntextIdable
    {
        ObservableCollection<Guid> ContextIdHistory { get; set; }
        IJornalable ParentObject { get; set; }

        event PropertiesChangeJornalChangedEventHandler JornalChangedNotify;
    }
}