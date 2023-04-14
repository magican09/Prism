using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IJornalable : IKeyable, IHierarchical
    {
        /// <summary>
        /// Включение жруналирования объекта
        /// </summary>
        public void JornalingOff();
        /// <summary>
        /// Выкдючение журналирования объектка
        /// </summary>
        public void JornalingOn();
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged;
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated;
      //  public event UnDoReDoCommandEventHandler UnDoReDoCommandCreated;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<IUnDoRedoCommand> AllChangesJornal { get; set; }
        /// <summary>
        /// Журнал хранящий историю изменявших объект команд
        /// </summary>
        public ObservableCollection<IUnDoRedoCommand> ChangesJornal { get; set; }
        //public ObservableCollection<IUnDoReDoSystem> UnDoReDoSystems { get; set; }
        /// <summary>
        /// UnDoReDoSystem сисестема в которой в данный момени зарегистрирован объект
        /// </summary>
        public IUnDoReDoSystem UnDoReDoSystem { get; set; }
        /// <summary>
        /// Флад включающий функцию авторегисрации добаляемых в тейкущй объект объектов
        /// </summary>
        public bool IsAutoRegistrateInUnDoReDo { get; set; }

        

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
