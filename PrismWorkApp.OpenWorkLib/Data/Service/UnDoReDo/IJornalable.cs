using System;
using System.Collections.ObjectModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IJornalable : IKeyable, IHierarchical
    {
        public Guid StoredId { get; set; }
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
      
        /// <summary>
        /// Cостояние объектв в системе UNDoRedu
        /// </summary>
        public EntityState  State { get; set; }
        /// <summary>
        /// Является ли объект частью дерева объектов сохраняемых в базу даннных
        /// </summary>
        public bool IsDbBranch { get; set; }
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
    public enum EntityState
    {
        //
        // Summary:
        //     The entity is not being tracked by the context.
        Detached = 0,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database. Its property
        //     values have not changed from the values in the database.
        Unchanged = 1,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database. It has
        //     been marked for deletion from the database.
        Deleted = 2,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database. Some or
        //     all of its property values have been modified.
        Modified = 3,
        //
        // Summary:
        //     The entity is being tracked by the context but does not yet exist in the database.
        Added = 4,
        Removed =5,
        Created=6
       

    }
    public  enum AdjustStatus
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
