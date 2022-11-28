using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IJornalable : IKeyable, ICuntextIdable
    {
        //public void Save(object prop_id, Guid currentContextId);
        //  public void SaveAll(Guid currentContextId);
        //   public void UnDo(PropertyStateRecord propertyState);
        //  public void UnDoLast(Guid currentContextId);
        //  public void UnDoAll(Guid currentContextId);
        public void JornalingOff();
        public void JornalingOn();
        ///     public JornalRecordType Status { get; set; }
        public Guid CurrentContextId { get; set; }
        public bool IsVisible { get; set; }
        // public UnDoReDoSystem UnDoReDoSystem { get; set; }
        //  public event PropertyChangedEventHandler PropertyBeforeChanged;
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged;
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated;
        //   public ObservableCollection<IJornalable> ParentObjects { get; set; }
        //  public ObservableCollection<IJornalable> ChildObjects { get; set; }

        //  public void AdjustObjectsStructure(UnDoReDoSystem changesJornal, IJornalable sourse = null);
        //   public void ResetObjectsStructure(IJornalable sourse = null);
        //   public void ClearChangesJornal();

        //  public bool IsUnDoReDoSystemIsEmpty(Guid currentContextId);
        //  public AdjustStatus AdjustedStatus { get; set; }

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
        REMOVED,
        COMPLEX_RECORD

    }

    public enum JornalRecordState
    {
        NONE,
        PICKED,
        DEACTIVATED,
        UNDO_COMPLETE,
        SAVED
    }
}
