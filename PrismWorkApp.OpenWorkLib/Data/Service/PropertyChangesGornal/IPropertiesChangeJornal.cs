using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IPropertiesChangeJornal
    {
        ObservableCollection<Guid> ContextIdHistory { get; set; }
        IJornalable ParentObject { get; set; }

        event PropertiesChangeJornalChangedEventHandler JornalChangedNotify;
    }
}