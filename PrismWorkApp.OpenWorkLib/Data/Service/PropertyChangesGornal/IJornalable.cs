using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IJornalable : IKeyable, ICuntextIdable
    {
        public void Save(object prop_id, Guid currentContextId);
        public void SaveAll(Guid currentContextId);
        public void UnDo(PropertyStateRecord propertyState);
        public void UnDoLast(Guid currentContextId);
        public void UnDoAll(Guid currentContextId);
        public void JornalingOff();
        public void JornalingOn();
        public JornalRecordType Status { get; set; }
        public Guid CurrentContextId { get; set; }
        public bool IsVisible { get; set; }
        public PropertiesChangeJornal PropertiesChangeJornal { get; set; }

        public event ObjectStateChangeEventHandler ObjectChangedNotify;//Событие вызывается при изменении в данном объекте 
        public event ObjectStateChangeEventHandler ObjectChangeSaved; //Событие вызывается при сохранении изменений в данном объекте
        public event ObjectStateChangeEventHandler ObjectChangeUndo; //Событие вызывается при отмете изменений в данном объекте
        public event PropertyChangedEventHandler PropertyBeforeChanged;
        public ObservableCollection<IJornalable> ParentObjects { get; set; }
        public ObservableCollection<IJornalable> ChildObjects { get; set; }

        public void AdjustObjectsStructure(PropertiesChangeJornal changesJornal, IJornalable sourse = null);
        public void ResetObjectsStructure(IJornalable sourse = null);
        public void ClearChangesJornal();

        public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId);
        public AdjustStatus AdjustedStatus { get; set; }

    }
    public enum AdjustStatus
    {

        UNADJUSTED
        , IN_PROCESS,
        ADJUSTED,
        NONE
    }

    public enum JornalRecordType
    {
        CREATED,
        MODIFIED,
        DELETED,
        UNJOURNALED,
        ADDED,
        REMOVED
       
    }

    public enum JornalRecordState
    {
        UNPICKED,
        PICKED,
        DEACTIVATED,
        UNDO_COMPLETE,
        SAVED
    }
}
