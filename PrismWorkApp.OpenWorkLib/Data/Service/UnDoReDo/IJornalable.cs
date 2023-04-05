using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IJornalable : IKeyable
    {
        public void JornalingOff();
        public void JornalingOn();
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged;
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated;
        public ObservableCollection<IUnDoRedoCommand> ChangesJornal { get; set; }
      //  public void SaveChanges();
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
