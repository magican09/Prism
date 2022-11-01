using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IJornalable
    {
        //  public PropertiesChangeJornal<PropertyStateRecord> PropertiesChangeJornal { get; set; }
        public void Save(object prop_id, Guid currentContextId);
        public void SaveAll(Guid currentContextId);
        //public void UnDo(Guid currentContextId);
        public void UnDo(PropertyStateRecord propertyState);
      
        public void UnDoAll(Guid currentContextId);
   //     public void JornalingOff();
     //   public void JornalingOn();
        public JornalRecordStatus Status { get; set; }
        public Guid CurrentContextId { get; set; }
        public PropertiesChangeJornal PropertiesChangeJornal { get; set; }
        public event ObjectStateChangeEventHandler ObjectChangedNotify;//Событие вызывается при изменении в данном объекте 
        public event ObjectStateChangeEventHandler ObjectChangeSaved; //Событие вызывается при сохранении изменений в данном объекте
        public event ObjectStateChangeEventHandler ObjectChangeUndo; //Событие вызывается при отмете изменений в данном объекте


        public ObservableCollection<IJornalable> ParentObjects { get; set; }
        public void AdjustAllParentsObjects();
        public void ClearChangesJornal();
        public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId);

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
