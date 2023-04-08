using Prism;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    /// <summary>
    /// Класс  для реализации системы UnDoReDo. 
    /// Для регистраи переменной в системе применять: на два уровня иерахии вниз UnDoReDoSysytem.Register(IJornable)
    ///                                               на всю глубину иерархии объектов UnDoReDoSysytem.RegisterAll(IJornable)
    ///Чтобы удалить отслеживаемую переменную :на два уровня иерахии вниз UnDoReDoSysytem.UnRegister(IJornable)
    ///                                               на всю глубину иерархии объектов UnDoReDoSysytem.UnRegisterAll(IJornable)
    /// </summary>
    public class UnDoReDoSystem : IUnDoReDoSystem, IActiveAware
    {
        private Stack<IUnDoRedoCommand> _UnDoCommands = new Stack<IUnDoRedoCommand>();
        private Stack<IUnDoRedoCommand> _ReDoCommands = new Stack<IUnDoRedoCommand>();
        private bool _monitorCommandActivity = false;
        private int _UnDoCounter = 0;
        private int _ReDoCounter = 0;

        public Guid Id { get; set; }
        #region IActiveAware 
        public bool IsActive { get; set; } ///Реализация интресфейса IActiveAware
        public event EventHandler IsActiveChanged;
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
        public event EventHandler CanExecuteChanged;

        public IUnDoReDoSystem ParentUnDoReDo { get; set; }///Хранит ссылку на родительскиую систему, если она есть
        public Dictionary<IJornalable, IUnDoReDoSystem> _RegistedModels { get; set; } = new Dictionary<IJornalable, IUnDoReDoSystem>();//Все зарегистрированые в системе объекты 
        public Dictionary<IJornalable, IUnDoReDoSystem> _ChildrenSystemRegistedModels { get; set; } = new Dictionary<IJornalable, IUnDoReDoSystem>();//Все зарегисрированные в дочерних системах объекты
        public ObservableCollection<IUnDoReDoSystem> ChildrenSystems = new ObservableCollection<IUnDoReDoSystem>();//Дочерние системы
        public ObservableCollection<IJornalable> ChangedObjects { get; set; } = new ObservableCollection<IJornalable>();//Все объекты в которых произошли  изменения
        public ObservableCollection<IUnDoRedoCommand> AllExecutedCommands { get; set; } = new ObservableCollection<IUnDoRedoCommand>();//Все созданных команды по всем измененеиям во всез зарегистированных объектах
        #region Main Methods 
        /// <summary>
        /// Метод отменяет ("откатывает назад") произвольное количество последних изменений
        /// </summary>
        /// <param name="levels">Количество отменяемых поледних шаго</param>
        /// <param name="without_redo">Можно отключить возможность вернуть вперед - по умолчанию возможность влючена</param>
        /// <returns></returns>
        public bool UnDo(int levels, bool without_redo = false)//bool without_redo = false возможено не понадобиться (отключение реду)
        {
            if (_monitorCommandActivity && !IsActive) return false;//Релизация IActiveAware (если функция включена и системв в активнос состоянии

            for (int ii = 0; ii < levels; ii++)
            {
                if (_UnDoCommands.Count > 0)
                {
                    IUnDoRedoCommand command = _UnDoCommands.Pop();
                    command.UnExecute();
                    AllExecutedCommands.Remove(command);
                    var objcs_for_remove = ChangedObjects.Where(cho => !_UnDoCommands.Where(cm => cm.ChangedObjects.Contains(cho)).Any()).ToList();
                    foreach (IJornalable j_obj in objcs_for_remove)
                        ChangedObjects.Remove(j_obj);
                    if (!without_redo)
                    {
                        _ReDoCommands.Push(command);
                        _ReDoCounter--;
                    }
                    _UnDoCounter++;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDo"));
            return (_UnDoCommands.Count > 0) ? true : false;
        }
        /// <summary>
        /// Метод отменяет сделаный "откат назад"("шаг вперед")  произвольное количество шагов
        /// </summary>
        /// <param name="levels">Количество   поледних шагов "перед" </param>
        /// <param name="without_redo">Можно отключить возможность вернуть назад - по умолчанию возможность влючена</param>
        /// <returns></returns>

        public bool ReDo(int levels, bool without_undo = false) //bool without_undo = false возможено не понадобиться (отлючение анду)
        {
            if (_monitorCommandActivity && !IsActive) return false;
            for (int ii = 0; ii < levels; ii++)
            {
                if (_ReDoCommands.Count > 0)
                {
                    IUnDoRedoCommand command = _ReDoCommands.Pop();
                    command.Execute();
                    AllExecutedCommands.Add(command);
                    //foreach (IJornalable obj in command.ChangedObjects)
                    //    obj.ChangesJornal.Add(command);
                    if (!without_undo)
                    {
                        _UnDoCommands.Push(command);
                        _UnDoCounter--;
                    }
                    _ReDoCounter++;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReDo"));
            return (_ReDoCommands.Count > 0) ? true : false;

        }

        /// <summary>
        ///Метод отменяет все изменения которые происходили в наблюдаемых объектах системы ("шагаем назад до упора")
        /// </summary>
        public void UnDoAll()
        {
            while (_UnDoCommands.Count > 0)
            {
                UnDo(1);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDoAll"));
        }
        /// <summary>
        ///Метод очищает все стэки.( и то что хранит "шаги назад" и тот, что хранит "шаги назад"
        /// </summary>
        public void ClearStacks()
        {

            _UnDoCommands.Clear();
            _ReDoCommands.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClearStacks"));
        }
        #endregion
        /// <summary>
        /// Метод сигнализирующее о том, что все отмены изменений  выполнены ( "шагать назад некуда")
        /// </summary>
        /// <returns>Возращае true, если стэк изменений пусты (записанных шагов нет)</returns>
        public bool IsAllUnDoIsDone()
        {
            return _UnDoCommands.Count == 0;
        }
        /// <summary>
        /// Метод сигнализирующее о том, что в системе нет записанных "шагов" как "вперед" так и "назад". Оба стека пусты.
        /// </summary>
        /// <returns>Возращае true, если оба стэка изменений пусты (записанных шаго нет</returns>
        public bool IsSatcksEmpty()
        {
            return _UnDoCommands.Count == 0 && _ReDoCommands.Count == 0;
        }
        /// <summary>
        /// Метод ипользуемый для опредения есть ли в системе события которые можно отменить (сделать шаг назад).
        /// </summary>
        /// <returns> Возращающий true, если в системе есть события которе можно откатить назад </returns>
        public bool CanUnDoExecute()
        {
            if (_monitorCommandActivity && !IsActive)
            {
                return false;
            }
            return _UnDoCommands.Count > 0;
        }
        public bool CanReDoExecute()
        {
            if (_monitorCommandActivity && !IsActive)
            {
                return false;
            }
            return _ReDoCommands.Count > 0;
        }

        #region IActivateAware
        /// <summary>
        /// Метод реализованного интрейфейса IAvtivaAware, который вызывается  при изменнении состояния активности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsActivateChaged(object sender, EventArgs e)
        {
            if (sender is IActiveAware active_aware_object)
            {
                IsActive = active_aware_object.IsActive;
                IsActiveChanged?.Invoke(this, new EventArgs());
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnIsActivateChagedChanged"));
            }
        }
        #endregion
        #region Registration 
        /// <summary>
        /// Метод для регистрации объекта  реализуещго IJornalable в системе. В системе регистрируются как сам объект,
        /// так и все его IJornalable свойства на всю глубину цепочек объектов IJornalable, пока не встретит 
        /// уже зарегисрированный объект.
        /// </summary>
        /// <param name="obj"> Регистрируемый объект IJornalable</param>
        public void Register(IJornalable obj)
        {
            if (obj == null) { throw new Exception("Попытка регистрации в системе UnDoReDo объекта со значением null"); }
            if (_RegistedModels.ContainsKey(obj) || _ChildrenSystemRegistedModels.ContainsKey(obj))//Если объект зарегисприван в данной или в дочерней системе - выходим
                return;
            else
            {
                if (ParentUnDoReDo != null)
                {
                    if (ParentUnDoReDo._ChildrenSystemRegistedModels.ContainsKey(obj)) //Если вы данный момент объект уже используется в другой системе
                        throw new Exception($"Объект занять другой стистемой UnDoReDo {obj.ToString()}");
                    if (ParentUnDoReDo._RegistedModels.ContainsKey(obj)) //Если регистрируем объект, которые уже был зарегистирован в родительской системе
                    {
                        ParentUnDoReDo.UnRegister(obj);//то удаляем его из родительской системы
                        ParentUnDoReDo._ChildrenSystemRegistedModels.Add(obj, this);
                    }
                }
                _RegistedModels.Add(obj, this);
            }
            ///Пробегаемся по свойствам и регистрируем свойства, который тоже IJornalable
            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                    this.Register(jornable_prop);
            }
            obj.JornalingOff(); //На всякий случай выключаем журналирование в объекте
            obj.PropertyBeforeChanged += OnModelPropertyBeforeChanged;//Событие возникающее в региструемом объекте перед изменением свойства
            obj.UnDoReDoCommandCreated += OnObservedCommandCreated;//Событие возникающее в региструемом коллекции  при применение любой команды IUnDoReDoCommand
            obj.UnDoReDoSystems.Add(this);//Добавляем систему в коллекцию наблюдающи за объектом систем
            obj.JornalingOn();

        }
        private IJornalable firstRegistredObject = null;//Переменная для хранения объекта с которого мы воли в рекурсивную фунцию
        /// <summary>
        /// Метод регистрирует все дерево объектов по иерахии внурь перескакивая и черех уже зарегисрированные, если 
        /// таковые встречаются.
        /// </summary>
        /// <param name="obj">Объект IJornalable, который будет зарегисрирована в сисиеме </param>
        /// <param name="first_itaration">Служебный флаг регистрации выхода из рекурсивной функции. Не изменять!</param>
        public void RegisterAll(IJornalable obj, bool first_itaration = true)
        {
            if (!first_itaration && obj == firstRegistredObject) return;
            if (first_itaration) firstRegistredObject = obj;
            foreach (IJornalable child in obj.Children)
                this.RegisterAll(child, false);
            this.Register(obj);
            if (obj == firstRegistredObject) firstRegistredObject = null;
        }

        /// <summary>
        /// Метод удаляет регистрацию объекта .
        /// </summary>
        /// <param name="obj">Удаляемы из системы объект</param>
        public void UnRegister(IJornalable obj)
        {
            if (_RegistedModels.ContainsKey(obj))
            {
                obj.JornalingOff();
                obj.PropertyBeforeChanged -= OnModelPropertyBeforeChanged;
                obj.UnDoReDoCommandCreated -= OnObservedCommandCreated;
                obj.UnDoReDoSystems.Remove(this);
                _RegistedModels.Remove(obj);
                if (ParentUnDoReDo != null)
                {
                    //if (ParentUnDoReDo._ChildrenSystemRegistedModels.ContainsKey(obj))
                    //    ParentUnDoReDo._ChildrenSystemRegistedModels.Remove(obj);
                    if (!ParentUnDoReDo._RegistedModels.ContainsKey(obj)) //Если регистрируем объект, которые уже был зарегистирован в родительской системе
                    {
                        ParentUnDoReDo.Register(obj);//то удаляем его из родительской системы
                        ParentUnDoReDo._ChildrenSystemRegistedModels.Remove(obj);
                    }
                }
                ///Пробегаемся по свойствам и регистрируем свойства, который тоже IJornalable
                var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
                foreach (PropertyInfo propertyInfo in props_infoes)
                {
                    var prop_val = propertyInfo.GetValue(obj);
                    var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                    if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                        this.UnRegister(jornable_prop);
                }

                ///Пока не понятно зачем все это...
                //if (obj is IUnDoReDoSystem child_system) //Если в системе регистриуюется дочерняя система, то объекты которые зарегирированы в дочерней системе
                //{//регистриуем в родительской   системе
                //    child_system.ParentUnDoReDo = null;
                //    foreach (IJornalable reg_obj in child_system._RegistedModels.Keys)
                //    {
                //        if (!_RegistedModels.ContainsKey(reg_obj)) 
                //            this.Register(reg_obj);
                //        child_system.UnRegister(reg_obj);
                //    }
                //}
                obj.JornalingOn();
            }
        }
        private IJornalable firstUnRegisteredObject = null;//Переменная для хранения объекта с которого мы воли в рекурсивную фунцию
        /// <summary>
        /// Метод удаляет регистрацию объекта и всего дерерва объектов по иерархии внурь.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="first_itaration">Служебный флаг регистрации выхода из рекурсивной функции. Не изменять!</param>
        public void UnRegisterAll(IJornalable obj, bool first_itaration = true)
        {
            if (!first_itaration && obj == firstUnRegisteredObject) return;
            if (first_itaration) firstUnRegisteredObject = obj;
            foreach (IJornalable child in obj.Children)
            {
                this.UnRegisterAll(child, false);
            }
            this.UnRegister(obj);
            if (obj == firstUnRegisteredObject) firstUnRegisteredObject = null;
        }

        //private Dictionary<IUnDoReDoSystem,IJornalable> unregisted_for_child_object = new Dictionary<IUnDoReDoSystem, IJornalable>)();
        /// <summary>
        /// Метод устанавливает передаваемую в аргументе систему в качетве дочерней для текущей системы.
        /// </summary>
        /// <param name="child_system">Сисема, которую надо уставновить в качестве доченей.</param>
        public void SetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system)
        {
            ///Если в системе регистриуюется дочерняя система, то объекты которые зарегирированы в дочерней системе
            ///удаляем регистрацию в родительской системе, что бы не дублировались записи об изменениях в двух системах
            if (!ChildrenSystems.Contains(children_system))
            {
                if (children_system.ParentUnDoReDo != null && children_system.ParentUnDoReDo != this)
                    children_system.UnSetChildrenUnDoReDoSystem(children_system.ParentUnDoReDo);
                foreach (IJornalable reg_obj in children_system._RegistedModels.Keys)   ///Проходим по всем зарегистрированным в дочернией
                    if (_RegistedModels.ContainsKey(reg_obj))                           ///системе объектам и если если объекты, которые зарегистрирована в родительской системе
                        this.UnRegisterAll(reg_obj);                                    ///удаляем регисраци в родительской системе
                children_system.ParentUnDoReDo = this;
                ChildrenSystems.Add(children_system);                                   ///Доавляем дочернюю систему в коллекцию дочерних систему родтельской системы

            }
        }
        /// <summary>
        /// Метод удаляет передаваемую в аргументе систему в из коллекции дочерних  для текущей системы.
        /// </summary>
        /// <param name="child_system">Сисема, которую надо убрать из дочених систем.</param>
        public void UnSetChildrenUnDoReDoSystem(IUnDoReDoSystem child_system)
        {
            ///Если в дочерней системе были объекты , то 
            /// регистриуем их в родительской системе
            if (ChildrenSystems.Contains(child_system))
            {

                foreach (IJornalable reg_obj in child_system._RegistedModels.Keys)///Перерегистрируем все объекты бывшей дочерней сисемы
                {                                                                 /// в текущей
                    child_system.UnRegisterAll(reg_obj);
                    //this._ChildrenSystemRegistedModels.Remove(reg_obj);
                    this.RegisterAll(reg_obj);
                }
                child_system.ParentUnDoReDo = null;
                ChildrenSystems.Remove(child_system);

            }
        }
        #endregion

        #region  Changes Invoke Metjods
        /// <summary>
        /// Метод обработчик собылтий IJornalable.UnDoReDoCommandCreated зарегистрированных в системе объектов.
        /// Вызывается после выполнения команды IUnDoRedoCommand изменений внутри наблюдаемого объекта 
        /// </summary>
        /// <param name="sender">Наблюдаемы объект в которо происходят изменнения</param>
        /// <param name="e">Передаваемая комагда и ее название обернутые в класс UnDoReDoCommandCreateEventsArgs</param>
        private void OnObservedCommandCreated(object sender, UnDoReDoCommandCreateEventsArgs e)
        {
            _ReDoCommands.Clear();///Сбрасывае все шаги "вперед" перед продложением  новой "ветки изменией" 
            _ReDoCounter = 0;
            IJornalable changed_obj = sender as IJornalable;
            IUnDoRedoCommand command = e.Command;
            _UnDoCommands.Push(command);
            AllExecutedCommands.Add(command);

            foreach (IJornalable ch_obj in command.ChangedObjects)
                if (!ChangedObjects.Contains(ch_obj)) ChangedObjects.Add(ch_obj);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnCommandCreated"));
        }
        /// <summary>
        /// Метод обработчик собылтий IJornalable.PropertyBeforeChanged зарегистрированных в системе объектов.
        /// Вызыаетя непосредственно перед изменением свойва наблюдаемого объекта
        /// </summary>
        /// <param name="sender">Наблюдаемы объект в которо происходят изменнения</param>
        /// <param name="e">Передаваемый из зарегисрированного объекта параметры обернутые в класс PropertyBeforeChangeEvantArgs</param>
        private void OnModelPropertyBeforeChanged(object sender, PropertyBeforeChangeEvantArgs e)
        {
            _ReDoCommands.Clear();///Сбрасывае все шаги "вперед" перед продложением  новой "ветки изменией" 
            _ReDoCounter = 0;
            IJornalable changed_obj = sender as IJornalable;
            PropertySetCommand command =
                new PropertySetCommand(changed_obj, e.PropertyName, e.NewValue, e.LastValue);//Созданем команду изменения свойства объекта
            _UnDoCommands.Push(command);
            AllExecutedCommands.Add(command);

            foreach (IJornalable ch_obj in command.ChangedObjects)
                if (!ChangedObjects.Contains(ch_obj)) ChangedObjects.Add(ch_obj);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnModelPropertyBeforeChanged"));
        }

        /// <summary>
        /// Метод добавляет одну систему UnDoReDoSystem  в другую в качетве команды IUnDoRedoCommand (так как UnDoReDoSystem
        /// реализует так же IUnDoRedoCommand).После этого добвленная система вомприниматеся как составная команда 
        ///  и выполняется целиков за один шаг UnDo или ReDo класса в который ее добавили 
        /// </summary>
        /// <param name="unDoReDo"> Добавляемая в качестве IUnDoRedoCommand UnDoReDoSystem система</param>
        public void AddUnDoReDo(IUnDoReDoSystem unDoReDo)
        {
            _UnDoCommands.Push((UnDoReDoSystem)unDoReDo);
            AllExecutedCommands.Add(unDoReDo);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddUnDoReDo"));
        }
        /// <summary>
        /// Метод сохраняет изменения объекта в системе (удаляет информацию об изменениях в системе) 
        /// </summary>
        /// <param name="obj"> Объкт измененения котророго будет стеры из системы</param>
        /// <returns></returns>
        public int SaveChages(IJornalable obj)
        {
            IJornalable saved_obj = obj;
            if (this.ChangedObjects.Contains(saved_obj))
            {///Получаем из IJornalable все команды которые были к нему применены - хранятся в его свойстве ChangesJornal
                List<IUnDoRedoCommand> obj_chg_commands = new List<IUnDoRedoCommand>(saved_obj.ChangesJornal);
                foreach (IUnDoRedoCommand command in obj_chg_commands)
                {///В каждой команда находим все объекты, которые эти команды затронули (изменили), кроме текущего объекта
                    List<IJornalable> chgd_objects = command.ChangedObjects.Where(ob => ob != saved_obj).ToList();
                    foreach (IJornalable chgd_obj in chgd_objects)
                    {
                        chgd_obj.ChangesJornal.Remove(command);///Удаляем в найденных объектах информацию о найденных командах
                    }
                    saved_obj.ChangesJornal.Remove(command);//Удаляем команды в текущем объекте
                }
                this.ChangedObjects.Remove(saved_obj);///Удаляем объект из журнала системы, где хараняться наблюдаемые объекты 
                                                      ///в которых были зафиксированы изменения
            }

            return 1;
        }

        private IJornalable firstSavedObject = null; //Переменная для хранения объекта с которого мы воли в рекурсивную фунцию
        /// <summary>
        /// Метод сохораняем все изменения по дереву объектов внурь объекта  (удаляет информацию об изменениях в системе) 
        /// </summary>
        /// <param name="obj">Объкт измененения котророго будет стеры из системы</param>
        /// <param name="first_itaration">Служебный флаг регистрации выхода из рекурсивной функции. Не изменять! </param>
        /// <returns>В проекте будет возращать количество объетов изменения котороых сохранили </returns>
        public int SaveAllChages(IJornalable obj, bool first_itaration = true)
        {

            if (!first_itaration && obj == firstSavedObject) return 0;
            if (first_itaration) firstSavedObject = obj;
            foreach (IJornalable child in obj.Children)
            {
                this.SaveAllChages(child, false);
            }
            this.SaveChages(obj);
            if (obj == firstSavedObject) firstSavedObject = null;
            return 1;
        }
        public int SaveAllChages()
        {
            foreach (IJornalable obj in _RegistedModels.Keys)
            {
                this.SaveAllChages(obj);
            }
            foreach (IUnDoReDoSystem child_unDoReDo in ChildrenSystems)
                    child_unDoReDo.SaveAllChages();
            return 1;
        }
        #endregion

        #region IUnDoRedoCommand Implamentaton

        #region Command Implementation
        /// <summary>
        /// Один из методов реализации IUnDoRedoCommand
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter = null) //ReDo all UnDoed 
        {
            ReDo(_UnDoCounter);
        }
        /// <summary>
        /// Один из методов реализации IUnDoRedoCommand
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _UnDoCommands.Count > 0 || _ReDoCommands.Count > 0;
        }
        #endregion
        /// <summary>
        /// Один из методов реализации IUnDoRedoCommand
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public void UnExecute()
        {
            UnDoAll();
        }
        public string Name { get; set; }
        #endregion


        /// <summary>
        /// Коструктор UnDoReDoSystem   
        /// </summary>
        /// <param name="caller_obj">Объект в коромо создана система UnDoReDoSystem и реализующий IActiveAware.
        /// Если передать кострукторы этот объект, то его состояние будет управлять состояние UnDoReDoSystem</param>
        /// <param name="monitorCommandActivity">Влючение IAtivaAware (monitoring On/Off) flag</param>
        public UnDoReDoSystem(object caller_obj = null, bool monitorCommandActivity = false)
        {
            Id = Guid.NewGuid();
            if (caller_obj is IActiveAware activeAware_caller_obj)
            {
                _monitorCommandActivity = monitorCommandActivity;
                activeAware_caller_obj.IsActiveChanged += OnIsActivateChaged;
            }
        }

    }
}