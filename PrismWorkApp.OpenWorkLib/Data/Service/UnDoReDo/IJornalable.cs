using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IJornalable : IKeyable, ICuntextIdable
    {
        public void JornalingOff();
        public void JornalingOn();
  //      public bool IsVisible { get; set; }
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged;
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated;
   
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
