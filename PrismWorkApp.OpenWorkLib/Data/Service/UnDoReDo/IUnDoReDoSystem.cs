using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public interface IUnDoReDoSystem : INotifyPropertyChanged, IUnDoRedoCommand
    {
        public Stack<IUnDoRedoCommand> _UnDoCommands { get; set; }
        public  Stack<IUnDoRedoCommand> _ReDoCommands { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Уникальный Id системы
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Метод сигнализирующее о том, что все отмены изменений  выполнены ( "шагать назад некуда")
        /// </summary>
        /// <returns></returns>
        bool IsAllUnDoIsDone();
        /// <summary>
        /// Метод сигнализирующее о том, что в системе нет записанных "шагов" как "вперед" так и "назад". Оба стека пусты.
        /// </summary>
        /// <returns></returns>
        public bool IsSatcksEmpty();
        /// <summary>
        /// Метод ипользуемый для опредения есть ли в системе события которые можно отменить (сделать шаг назад).
        /// </summary>
        /// <returns></returns>
        bool CanUnDoExecute();
        /// <summary>
        /// Метод ипользуемый для опредения есть ли в системе события которые можно востановить (сделать шаг вперед).
        /// </summary>
        /// <returns>Возращающий true, если в системе есть события которе можно востановить вперед </returns>
        bool CanReDoExecute();
        /// <summary>
        /// Метод удаляет регистрацию объекта из ткущей системы .
        /// </summary>
        /// <param name="obj">Удаляемы из системы объект</param>
        void UnRegister(IJornalable obj);
        /// <summary>
        /// Метод удаляет регистрацию объекта и всего дерерва объектов по иерархии внурь.
        /// </summary>
        /// <param name="obj"></param>
      //  public void UnRegisterAll(IJornalable obj);
        /// <summary>
        /// Метод для регистрации объекта  реализуещго IJornalable в системе. В системе регистрируются как сам объект,
        /// так и все его IJornalable свойства на всю глубину цепочек объектов IJornalable, пока не встретит 
        /// уже зарегисрированный объект.
        /// </summary>
        /// <param name="obj"> Регистрируемый объект IJornalable</param>
        public void Register(IJornalable obj, bool enable_outo_registration = true, bool is_db_branch = false);
        /// <summary>
        /// Метод регистрирует все дерево объектов по иерахии внурь перескакивая и череp уже зарегисрированные, если 
        /// таковые встречаются.
        /// </summary>
        /// <param name="obj">Объект IJornalable, который будет зарегисрирована в сисиеме </param>
        /// <param name="first_itaration">Служебный флаг регистрации выхода из рекурсивной функции. Не изменять!</param>
       // public void RegisterAll(IJornalable obj, bool first_itaration = true);
        /// <summary>
        /// Метод отменяет ("откатывает назад") произвольное количество последних изменений
        /// </summary>
        /// <param name="levels">Количество отменяемых поледних шаго</param>
        /// <param name="without_redo">Можно отключить возможность вернуть вперед - по умолчанию возможность влючена</param>
        /// <returns></returns>
        bool UnDo(int levels, bool without_redo = false);
        /// <summary>
        /// Метод отменяет сделаный "откат назад"("шаг вперед")  произвольное количество шагов
        /// </summary>
        /// <param name="levels">Количество   поледних шагов "перед" </param>
        /// <param name="without_redo">Можно отключить возможность вернуть назад - по умолчанию возможность влючена</param>
        /// <returns></returns>
        bool ReDo(int levels, bool without_undo = false);
        /// <summary>
        ///Метод отменяет все изменения которые происходили в наблюдаемых объектах системы ("шагаем назад до упора")
        /// </summary>
        void UnDoAll();
        /// <summary>
        ///Метод очищает все стэки.( и то что хранит "шаги назад" и тот, что хранит "шаги назад"
        /// </summary>
        void ClearStacks();
        /// <summary>
        /// Метод добавляет одну систему UnDoReDoSystem  в другую в качетве команды IUnDoRedoCommand (так как UnDoReDoSystem
        /// реализует так же IUnDoRedoCommand).После этого добвленная система вомприниматеся как составная команда 
        ///  и выполняется целиков за один шаг UnDo или ReDo класса в который ее добавили 
        /// </summary>
        /// <param name="unDoReDo"> Добавляемая в качестве IUnDoRedoCommand UnDoReDoSystem система</param>
        void AddUnDoReDoSysAsCommand(IUnDoReDoSystem unDoReDo);

        /// <summary>
        /// Метод устанавливает передаваемую в аргументе систему в качетве дочерней для текущей системы.
        /// </summary>
        /// <param name="child_system">Сисема, которую надо уставновить в качестве доченей.</param>
        void SetUnDoReDoSystemAsChildren(IUnDoReDoSystem children_system);
        /// <summary>
        /// Метод удаляет передаваемую в аргументе систему в из коллекции дочерних  для текущей системы.
        /// </summary>
        /// <param name="child_system">Сисема, которую надо убрать из дочених систем.</param>
        void UnSetUnDoReDoSystemAsChildren(IUnDoReDoSystem children_system);
        /// <summary>
        /// Свойство содержит ссыку на родителькую систему, если текущая системв была в нее доавблаена при 
        /// помощи метода .SetUnDoReDoSystemAsChildren(current_undoredoSystem)
        /// </summary>
        IUnDoReDoSystem ParentUnDoReDo { get; set; }
        /// <summary>
        /// Коллекция хранит ссылки на все зарегистрированные в данной системе объекты IJornalable
        /// </summary>
        Dictionary<IJornalable, IUnDoReDoSystem> _RegistedModels { get; set; }
        /// <summary>
        /// Коллекция хранит ссылки на все зарегистрированые в данной системе дочерние системы
        /// </summary>
        ObservableCollection<IUnDoReDoSystem> ChildrenSystems { get;set;}
        /// <summary>
        /// Коллекция храниит все объекты, который зарегистрированы в дочерних системах.
        /// </summary>
      //   Dictionary<IJornalable, IUnDoReDoSystem> _ChildrenSystemRegistedModels { get; set; }
        /// <summary>
        /// Метод сохраняет изменения только в конкретном объекте в текущей системе (удаляет информацию об изменениях в системе и участвоваваших
        /// в изменнеии данного объектв других объектах) 
        /// </summary>
        /// <param name="obj"> Объкт,измененения котророго будут стеры из системы</param>
        /// <returns></returns>
         int Save(IJornalable obj);
        /// <summary>
        /// Метод сохораняем все изменения по дереву объектов внурь объекта в текущей системе(удаляет информацию об изменениях в системе) 
        /// то ксть метод .SaveChanges(obj) применяется ко всех объектам внутри obj
        /// </summary>
        /// <param name="obj">Объкт измененения котророго будет стеры из системы</param>
        /// <param name="first_itaration">Служебный флаг регистрации выхода из рекурсивной функции. Не изменять! </param>
        /// <returns>В проекте будет возращать количество объетов изменения котороых сохранили </returns>
        //int SaveAllChagesInCurrentSys(IJornalable obj, bool first_itaration = true);
        /// <summary>
        /// Сохраняет все изменения для объекта IJornable во всю глубины иерахии внутрь объекта во всех системах
        /// в котрой зарегистрирована объектв. И в дочерних, и в текущей и в родииельской.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //int SaveAllChages(IJornalable obj);
        //int SaveAllChages();
        public ObservableCollection<IJornalable> _AllChangedObjects { get; }
        public int SaveAll();
        public bool IsAnyChildSystemRegistered(IJornalable obj);
        public bool IsRegistered(IJornalable obj);
        public bool Contains(IJornalable obj);
        public int GetChangesNamber(IJornalable obj, bool first_itr = true);
        public IEnumerable<IUnDoRedoCommand> GetAllCommandsByObject(IJornalable obj, bool firs_itaration = true);
        public int Level { get; set; }
        public void UnDoAll(IJornalable obj);
      //  public int SystemHaveNotSavedObjectsMethodsNamber { get; set; }
        public event UnDoReDoSystemEventHandler SystemHaveNotSavedObjects;
        public bool HasAnyChangedObjectInAllSystems();
    }
}