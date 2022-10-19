using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IJornalable
    {
        //  public PropertiesChangeJornal<PropertyStateRecord> PropertiesChangeJornal { get; set; }
        public void Save(object prop_id, Guid currentContextId);
        public void SaveAll(Guid currentContextId);
        public void UnDo(Guid currentContextId);
        public void UnDoAll(Guid currentContextId);
        public void JornalingOff();
        public void JornalingOn();
        public JornalRecordStatus Status { get; set; }
        public bool IsVisible { get; set; }
        public Guid CurrentContextId { get; set; }
        public PropertiesChangeJornal PropertiesChangeJornal { get; set; }
        public event PropertiesChangeJornalChangedEventHandler ObjectChangedNotify;


        //public void  SetParentObject(IJornalable obj);

    }


    public enum JornalRecordStatus
    {
        CREATED,
        MODIFIED,
        DELETED,
        UNJOURNALED,
        ADDED,
        REMOVED
    }
}
