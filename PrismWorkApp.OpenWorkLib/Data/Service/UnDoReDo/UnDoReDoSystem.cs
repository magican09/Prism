using Prism;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        public event UnDoReDoSystemEventHandler SystemHaveNotSavedObjects;
        public Stack<IUnDoRedoCommand> _UnDoCommands { get; set; } = new Stack<IUnDoRedoCommand>();
        public Stack<IUnDoRedoCommand> _ReDoCommands { get; set; } = new Stack<IUnDoRedoCommand>();
        private bool _monitorCommandActivity = false;
        private int _UnDoCounter = 0;
        private int _ReDoCounter = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        private int _level;
        public int Level
        {
            get
            {
                if (this.ParentUnDoReDo != null)
                    _level = this.ParentUnDoReDo.Level + 1;
                else
                    _level = 0;
                return _level;
            }
            set
            {
                _level = value;
            }
        }
        private int _index;

        public int Index
        {
            get
            {
                if (UnDoReDo_System != null &&
                   UnDoReDo_System._UnDoCommands.Where(cm => cm.Id == this.Id).FirstOrDefault() != null)
                    _index = UnDoReDo_System._UnDoCommands.ToList().IndexOf(
                       UnDoReDo_System._UnDoCommands.Where(cm => cm.Id == this.Id).FirstOrDefault());
                else _index = -1;
                return _index;
            }
            set { _index = value; }
        }

        //public int Index 
        //{
        //    get
        //    {
        //        if (UnDoReDo_System != null)
        //            return UnDoReDo_System._UnDoCommands.ToList().IndexOf(this);
        //        else return -1;
        //    }
        //     }
        public Guid Id { get; set; }
        #region IActiveAware 
        public bool IsActive { get; set; } ///Реализация интресфейса IActiveAware
        public event EventHandler IsActiveChanged;
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Свойство содержит ссыку на родителькую систему, если текущая системв была в нее доавблаена при 
        /// помощи метода .SetUnDoReDoSystemAsChildren(current_undoredoSystem)
        /// </summary>
        private IUnDoReDoSystem _parentUnDoReDo;
        /// <summary>
        /// Хранит ссылку на родительскиую систему, если она есть
        /// </summary>
        public IUnDoReDoSystem ParentUnDoReDo
        {
            get { return _parentUnDoReDo; }
            set
            {
                _parentUnDoReDo = value;

            }
        }//Хранит ссылку на родительскиую систему, если она есть
        /// <summary>
        /// Коллекция хранит все зарегистрированные в данной системе объекты IJornalable
        /// </summary>
        public ObservableCollection<IJornalable> _AllChangedObjects
        {
            get
            {
                IEnumerable<IJornalable> all_models = new ObservableCollection<IJornalable>(ChangedObjects);
                foreach (IUnDoReDoSystem system in ChildrenSystems)
                {
                    all_models = all_models.Union(system.ChangedObjects);
                }
                return new ObservableCollection<IJornalable>(all_models);
            }
        } //Все зарегистрированые в системе и в дочерних системах объекты объекты 
        /// <summary>
        /// Коллекция хранит все зарегистрированные в данной системе объекты IJornalable
        /// </summary>
        public Dictionary<IJornalable, IUnDoReDoSystem> _RegistedModels { get; set; } = new Dictionary<IJornalable, IUnDoReDoSystem>();//Все зарегистрированые в системе объекты 
        /// <summary>
        /// Коллекция храниит все объекты, который зарегистрированы в дочерних системах.
        /// </summary>
      //  public Dictionary<IJornalable, IUnDoReDoSystem> _ChildrenSystemRegistedModels { get; set; } = new Dictionary<IJornalable, IUnDoReDoSystem>();//Все зарегисрированные в дочерних системах объекты
        /// <summary>
        /// Коллекция хранит ссылки на все зарегистрированые в данной системе дочерние системы
        /// </summary>
        public ObservableCollection<IUnDoReDoSystem> ChildrenSystems { get; set; } = new ObservableCollection<IUnDoReDoSystem>();//Дочерние системы

        /// <summary>
        /// Коллекция хранит ссылки на объекты на которыйе  в системе имеются зарегистрированные измененеия
        /// </summary>
        public ObservableCollection<IJornalable> ChangedObjects { get; set; } = new ObservableCollection<IJornalable>();//Все объекты в которых произошли  изменения

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
                    if (!without_redo)
                    {
                        _ReDoCommands.Push(command);
                        _ReDoCounter--;
                    }
                    _UnDoCounter++;
                }
            }
            OnPropertyChanged("UnDo");
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
                    if (!without_undo)
                    {
                        _UnDoCommands.Push(command);
                        _UnDoCounter--;
                    }
                    _ReDoCounter++;
                }
            }
            OnPropertyChanged("ReDo");
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
            OnPropertyChanged("UnDoAll");

        }
        private IEnumerable<IUnDoRedoCommand> all_commands;
        /// <summary>
        /// Метод возвращает все команды изменений(в порядук убывания даты создания) объекта в данной и в дочерних системах 
        /// </summary>
        /// <param name="obj">Объкт  IJornalable</param>
        /// <param name="firs_itaration"></param>
        /// <returns></returns>
        public IEnumerable<IUnDoRedoCommand> GetAllCommandsByObject(IJornalable obj, bool firs_itaration = true)
        {
            if (firs_itaration) all_commands = new List<IUnDoRedoCommand>();
            var all_object_systems = this.ChildrenSystems.Where(s => s._UnDoCommands.Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == obj.StoredId).Any()).Any());
            foreach (IUnDoReDoSystem unDoReDo in all_object_systems)//Если объект зарегисрирован в дочених системах...
            {
                
                
                all_commands = all_commands.Union(unDoReDo.GetAllCommandsByObject(obj, false));
            }
            all_commands = all_commands.Union(this._UnDoCommands.Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == obj.StoredId).Any()));
            return all_commands.OrderByDescending(cm => cm.Date);
        }
        public bool HasAnyChangedObjectInAllSystems()
        {
            bool b_have_change = false;
            foreach (IUnDoReDoSystem system in this.ChildrenSystems)
                if (system.ChangedObjects.Count > 0) { b_have_change = true; break; }
            return b_have_change || this.ChangedObjects.Count > 0;
        }

        public void UnDoAll(IJornalable obj)//Пока не реализован ... надо смотреть...
        {
            List<IJornalable> all_changed_sub_objects = new List<IJornalable>(obj.GetAllChangedObjects());

            //for(int ii=0;ii<all_changed_sub_objects.Count;ii++)
            foreach (IJornalable current_obj in all_changed_sub_objects)
            {
                var object_all_commands = this.GetAllCommandsByObject(current_obj);
                foreach (IUnDoRedoCommand command in object_all_commands)
                {
                    command.UnExecute();
                    command.UnDoReDo_System._UnDoCommands.Remove(command);
                    current_obj.ChangesJornal.Remove(command);
                    var ather_objects = command.ChangedObjects.Where(ob => ob.StoredId != current_obj.StoredId);
                    foreach (IJornalable ather_object in ather_objects)
                    {
                        ather_object.ChangesJornal.Remove(command);
                        if (ather_object.ChangesJornal.Count == 0 &&
                            command.UnDoReDo_System.ChangedObjects.Where(ob => ob.StoredId == ather_object.StoredId).Any())
                            command.UnDoReDo_System.ChangedObjects.Remove(ather_object);
                    }

                }
            }
            OnPropertyChanged("UnDoAll");

        }
        /// <summary>
        ///Метод очищает все стэки.( и то что хранит "шаги назад" и тот, что хранит "шаги назад"
        /// </summary>
        public void ClearStacks()
        {

            _UnDoCommands.Clear();
            _ReDoCommands.Clear();
            OnPropertyChanged("ClearStacks");
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
        /// <summary>
        /// Метод ипользуемый для опредения есть ли в системе события которые можно востановить (сделать шаг вперед).
        /// </summary>
        /// <returns>Возращающий true, если в системе есть события которе можно востановить вперед </returns>
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
                OnPropertyChanged("OnIsActivateChagedChanged");
            }
        }
        #endregion
        #region Comman Metods 
        /// <summary>
        /// Метод возращает true,если объект зарегисрирован хотя бы о одной из дочерних систем.
        /// </summary>
        /// <param name="obj">бъект типа IJornalable</param>
        /// <returns></returns>
        public bool IsAnyChildSystemRegistered(IJornalable obj)
        {
            bool is_in_child_system = ChildrenSystems?.Where(s => s._RegistedModels.ContainsKey(obj)).FirstOrDefault() != null;

            return is_in_child_system;
        }
        /// <summary>
        /// Метод возращает true, если объект зарегисрирован в данной или в дочерних системах
        /// </summary>
        /// <param name="obj">Объект типа IJornalable</param>
        /// <returns></returns>
        public bool IsRegistered(IJornalable obj)
        {
            return this._RegistedModels.ContainsKey(obj) || this.IsAnyChildSystemRegistered(obj);
        }
        public bool Contains(IJornalable obj)
        {
            return this._RegistedModels.ContainsKey(obj) || this.IsAnyChildSystemRegistered(obj);
        }
        #endregion
        #region Registration 

        public void Register(IJornalable obj)
        {
            this.Register(obj, true, true);
        }
        /// <summary>
        /// Метод для регистрации объекта  реализуещго IJornalable в системе. В системе регистрируются как сам объект,
        /// так и все его IJornalable свойства на всю глубину цепочек объектов IJornalable, пока не встретит 
        /// уже зарегисрированный объект.
        /// </summary>
        /// <param name="obj"> Регистрируемый объект IJornalable</param>
        /// <param name="enable_outo_registration"> Влючает функую авторегистрации в объекте..</param>
        public void Register(IJornalable obj, bool enable_outo_registration = true, bool is_db_branch = false)
        {

            if (obj == null) { throw new Exception("Попытка регистрации в системе UnDoReDo объекта со значением null"); }
            //if (this.IsRegistered(obj))//Если объект зарегисприван в данной или в дочерней системе - выходим
            //obj.UnDoReDoSystem.UnRegister(obj);//Если объект и меет регисрацию в другой системе - удаляем эту регисрацию.. это противорчечи   циклу  ниже, но посмотрим..
          
            if (this._RegistedModels.ContainsKey(obj) && obj.UnDoReDoSystem!=null &&  obj.UnDoReDoSystem.Id == this.Id)//Если объект зарегисприван в данной  системе - выходим
                return;
           // if (this._RegistedModels.ContainsKey(obj) && (obj.UnDoReDoSystem==null || obj.UnDoReDoSystem.StoredId != this.StoredId)) //Если объект был ранее зарегистрирован в этой системеме, но зарегистрирована в другой 
            //   { obj.UnDoReDoSystem = this; return; }                                         // просто меняем текущую систему объектв на эту..
            //var all_object_systems = this.ChildrenSystems.Where(s => s._UnDoCommands.Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == obj.StoredId).Any()).Any()).ToList();
            //if (obj.UnDoReDoSystem != null && obj.UnDoReDoSystem.StoredId != this.StoredId && all_object_systems.Count == 0)
            //    obj.UnDoReDoSystem.UnRegister(obj);//Если объект и меет регисрацию в другой системе - удаляем эту регисрацию.. это противорчечи   циклу  ниже, но посмотрим..
            //                                       //throw new Exception("Попытка регистрации в системе  объекта с уже выполенной регистрацией в другой системе.!\n" +
            //                                       //  "Уберите регисрацию объекта в дрной системе UnDoReDoSystem и после регистрируйте его в этой системе.");

            //foreach (IUnDoReDoSystem unDoReDo in all_object_systems)//Если объект зарегисрирован в дочених системах...
            //     unDoReDo.UnRegister(obj); //Удаляем регисрацию в дочерних системах, и регисрируем объект в текущей

            //if (obj.UnDoReDoSystem != null && obj.UnDoReDoSystem.StoredId != this.StoredId) //Если объект зарегисрирован в другой системе..
            //{
            //    var all_chages_in_obj = new List<IJornalable>(obj.GetAllChangedObjects()); //Получаем внутренние объкеты объкта, в которых были несохраненые изменения
            //                                                                               ////Выполнятеся когда пытаются зарегистрировать объект имеющий изменения в дочерней системе... 
            //    if (all_chages_in_obj.Count > 0 && this.SystemHaveNotSavedObjects != null) //Если в обработчик наличия в регистриуемом объекте несохранненых подобъектов влючен - вызывем его...
            //        this.SystemHaveNotSavedObjects?.Invoke(this, new UnDoReDoSystemEventArgs(all_chages_in_obj));
            //    else if (all_chages_in_obj.Count > 0) //Если обработчика нет  - то автоматически сохраняем все изменнеия в объекте.
            //        foreach (IJornalable non_save_object in all_chages_in_obj)
            //            obj.UnDoReDoSystem.Save(non_save_object);

            //    //obj.UnDoReDoSystem.UnRegister(obj);//то удаляем его из родительской системы
            //    is_db_branch = obj.IsDbBranch;
            //}

            //if (ParentUnDoReDo != null) //Если существует родительская система..
            //{
            //    if (ParentUnDoReDo.IsAnyChildSystemRegistered(obj)) //Если вы данный момент объект уже используется в другой, смежной дочерней системе..
            //        throw new Exception($"Объект занять другой стистемой UnDoReDo {obj.ToString()}");
            //    if (ParentUnDoReDo.IsRegistered(obj)) //Если  объект зарегистрирован в родительской системе.. 
            //    {
            //        var all_chages_in_obj =  new List<IJornalable>(obj.GetAllChangedObjects()); //Получаем внутренние объкеты объкта, в которых были несохраненые изменения
            //       ////Выполнятеся когда пытаются зарегистрировать объект имеющий изменения в дочерней системе... 
            //        if (all_chages_in_obj.Count > 0 && this.SystemHaveNotSavedObjects != null) //Если в обработчик наличия в регистриуемом объекте несохранненых подобъектов влючен - вызывем его...
            //            this.SystemHaveNotSavedObjects?.Invoke(this, new UnDoReDoSystemEventArgs(all_chages_in_obj));
            //        else if (all_chages_in_obj.Count > 0) //Если обработчика нет  - то автоматически сохраняем все изменнеия в объекте.
            //            foreach (IJornalable non_save_object in all_chages_in_obj)
            //                this.ParentUnDoReDo.Save(non_save_object);

            //        ParentUnDoReDo.UnRegister(obj);//то удаляем его из родительской системы
            //    }
            //}
            obj.JornalingOff(); //На всякий случай выключаем журналирование в объекте

            if (obj.UnDoReDoSystem==null||obj.UnDoReDoSystem.Id != this.Id)//Если объект был в друной системе - выписываемся из нее...
            {
                // obj.UnDoReDoSystem?.UnRegister(obj);
                obj.UnDoReDoSystem = this;//Устанавливаем текущую систему в объекте .
            }

            if (obj is IList list_obj) //Если регистрируемый элемент сам  является коллекцией
                foreach (IJornalable element in list_obj)
                    this.Register(element, enable_outo_registration, is_db_branch);
            obj.PropertyBeforeChanged += OnModelPropertyBeforeChanged;//Событие возникающее в региструемом объекте перед изменением свойства
            obj.UnDoReDoCommandCreated += OnObservedCommandCreated;//Событие возникающее в региструемом коллекции  при применение любой команды IUnDoReDoCommand
            obj.IsAutoRegistrateInUnDoReDo = enable_outo_registration;
             if(!_RegistedModels.Where(el=>el.Key.StoredId==obj.StoredId).Any()) 
                _RegistedModels.Add(obj, this);
            if (obj.State == EntityState.Detached)
                obj.State = EntityState.Unchanged;
            obj.IsDbBranch = is_db_branch;
           
            ///Пробегаемся по свойствам и рекурсивно регистрируем все свойства, которые тоже IJornalable
            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                    this.Register(jornable_prop, enable_outo_registration, is_db_branch);

            }

            obj.JornalingOn();
            OnPropertyChanged("Register");

        }
        private IJornalable firstRegistredObject = null;//Переменная для хранения объекта с которого мы воли в рекурсивную фунцию
        /// <summary>
        /// Метод удаляет регистрацию объекта и всего дерерва объектов по иерархии внурь.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="first_itaration">Служебный флаг регистрации выхода из рекурсивной функции. Не изменять!</param>
        public void UnRegister(IJornalable obj)
        {

            if (obj == null) { throw new Exception("Попытка удаления регистрации в системе UnDoReDo объекта со значением null"); }
            if (!this.IsRegistered(obj))//Если объект не зарегисприван в данной или в дочерней системе - выходим
                return;
            if (this.IsAnyChildSystemRegistered(obj)) //Если существует  дорених системах 
            {
                ///Удаляем в ней регистрацию объекта рекурсивно...
                foreach (IUnDoReDoSystem system in this.ChildrenSystems.Where(s => s._RegistedModels.ContainsKey(obj)).ToList())
                    system.UnRegister(obj);
            }
            ///Пробегаемся по свойствам и рекурсивно удаляем регистрацию всех свойств, которые тоже IJornalable
            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attrb = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attrb == null)//Если свойтво IJornable и не помчено атрибутом 
                {
                    this.UnRegister(jornable_prop);
                }
            }
            obj.JornalingOff(); //На всякий случай выключаем журналирование в объекте
            obj.UnDoReDoSystem = null;//Удаляем текущую систему в объекте .
            obj.PropertyBeforeChanged -= OnModelPropertyBeforeChanged;//Событие возникающее в региструемом объекте перед изменением свойства
            obj.UnDoReDoCommandCreated -= OnObservedCommandCreated;//Событие возникающее в региструемом коллекции  при применение любой команды IUnDoReDoCommand
            obj.IsAutoRegistrateInUnDoReDo = false;
            _RegistedModels.Remove(obj);
            obj.JornalingOn();
            if (obj is IList list_obj) //Если регистрируемый элемент сам является коллекцией
                foreach (IJornalable element in list_obj)
                    this.UnRegister(element);
            OnPropertyChanged("UnRegister");

        }

        /// <summary>
        /// Метод устанавливает передаваемую в аргументе систему в качетве дочерней для текущей системы.
        /// </summary>
        /// <param name="child_system">Сисема, которую надо уставновить в качестве доченей.</param>
        public void SetUnDoReDoSystemAsChildren(IUnDoReDoSystem child_system)
        {
            if (this.Id == child_system.Id) return;
            ///Если в системе регистриуюется дочерняя система, то объекты которые зарегирированы в дочерней системе
            ///удаляем регистрацию в родительской системе, что бы не дублировались записи об изменениях в двух системах
            if (!ChildrenSystems.Where(s=>s.Id == child_system.Id).Any())
            {
                //if (this._UnDoCommands.Count > 0)//Если в родительской к
                //{
                //  List<IJornalable> chnaged_objs=  new List<IJornalable>(ChangedObjects.Where(ob => ob != null)).ToList();
                //    SystemHaveNotSavedObjects?.Invoke(this, new UnDoReDoSystemEventArgs(chnaged_objs));
                //}
                if (child_system.ParentUnDoReDo != null && child_system.ParentUnDoReDo.Id != this.Id)///Если регистрируемая система уже имела родительскую систему
                    child_system.UnSetUnDoReDoSystemAsChildren(child_system.ParentUnDoReDo);//удяляем регисрацию из родительской системы

                //foreach (IJornalable reg_obj in child_system._RegistedModels.Keys)   ///Проходим по всем зарегистрированным в дочернией
                //    if (_RegistedModels.ContainsKey(reg_obj))                           ///системе объектам и если находим  объекты, которые зарегистрирована в родительской системе
                //        this.UnRegister(reg_obj);                                    ///удаляем регисрацию в родительской системе

                child_system.ParentUnDoReDo = this;
                child_system.SystemHaveNotSavedObjects += this.SystemHaveNotSavedObjects;
                child_system.PropertyChanged += this.PropertyChanged;
                ChildrenSystems.Add(child_system);                                   ///Доавляем дочернюю систему в коллекцию дочерних систему родтельской системы

            }
            OnPropertyChanged("SetUnDoReDoSystemAsChildren");

        }

        /// <summary>
        /// Метод удаляет передаваемую в аргументе систему в из коллекции дочерних  для текущей системы.
        /// </summary>
        /// <param name="child_system">Сисема, которую надо убрать из дочених систем.</param>
        public void UnSetUnDoReDoSystemAsChildren(IUnDoReDoSystem child_system)
        {
            ///Если в дочерней системе были объекты , то 
            /// регистриуем их в родительской системе
            if (ChildrenSystems.Where(s=>s.Id == child_system.Id).Any())
            {
                foreach (IJornalable reg_obj in child_system._RegistedModels.Keys)///Перерегистрируем все объекты бывшей дочерней сисемы
                {                                                                 /// в текущей
                    //child_system.UnRegister(reg_obj);
                    this.Register(reg_obj);
                    reg_obj.UnDoReDoSystem = this;
                }
                child_system.ParentUnDoReDo = null;
                child_system.SystemHaveNotSavedObjects -= this.SystemHaveNotSavedObjects;
                child_system.PropertyChanged -= this.PropertyChanged;
                ChildrenSystems.Remove(child_system);
            }
            OnPropertyChanged("UnSetUnDoReDoSystemAsChildren");

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
            IJornalable changed_obj = sender as IJornalable;
            if (changed_obj.UnDoReDoSystem.Id == this.Id)
            {
                _ReDoCommands.Clear();///Сбрасывае все шаги "вперед" перед продложением  новой "ветки изменией" 
                _ReDoCounter = 0;
                IUnDoRedoCommand command = e.Command;
                _UnDoCommands.Push(command);
                foreach (IJornalable ch_obj in command.ChangedObjects)
                    if (!ChangedObjects.Where(ob => ob.StoredId == ch_obj.StoredId).Any()) ChangedObjects.Add(ch_obj);

                OnPropertyChanged("OnCommandCreated");
            }
        }
        /// <summary>
        /// Метод обработчик собылтий IJornalable.PropertyBeforeChanged зарегистрированных в системе объектов.
        /// Вызыаетя непосредственно перед изменением свойва наблюдаемого объекта
        /// </summary>
        /// <param name="sender">Наблюдаемы объект в которо происходят изменнения</param>
        /// <param name="e">Передаваемый из зарегисрированного объекта параметры обернутые в класс PropertyBeforeChangeEvantArgs</param>
        private void OnModelPropertyBeforeChanged(object sender, PropertyBeforeChangeEvantArgs e)
        {
            IJornalable changed_obj = sender as IJornalable;
            PropertyInfo prop_info = changed_obj.GetType().GetProperty(e.PropertyName);
            var prop_attrb = prop_info?.GetCustomAttribute<NotJornalingAttribute>();
            if (prop_info != null && prop_attrb == null && changed_obj.UnDoReDoSystem.Id == this.Id)
            {
                _ReDoCommands.Clear();///Сбрасывае все шаги "вперед" перед продложением  новой "ветки изменией" 
                _ReDoCounter = 0;

                PropertySetCommand command =
                    new PropertySetCommand(changed_obj, e.PropertyName, e.NewValue, e.LastValue);//Созданем команду изменения свойства объекта
                _UnDoCommands.Push(command);
                foreach (IJornalable ch_obj in command.ChangedObjects)
                    if (!ChangedObjects.Where(ob=>ob.StoredId==ch_obj.StoredId).Any()) ChangedObjects.Add(ch_obj);

                OnPropertyChanged("OnModelPropertyBeforeChanged");
            }
        }

        public void SetUnDoReDo_System(IUnDoReDoSystem unDoReDoSystem)
        {
            this.UnDoReDo_System = unDoReDoSystem;
        }
        /// <summary>
        /// Метод добавляет одну систему UnDoReDoSystem  в другую в качетве команды IUnDoRedoCommand (так как UnDoReDoSystem
        /// реализует так же IUnDoRedoCommand).После этого добвленная система вомприниматеся как составная команда 
        ///  и выполняется целиков за один шаг UnDo или ReDo класса в который ее добавили 
        /// </summary>
        /// <param name="unDoReDo"> Добавляемая в качестве IUnDoRedoCommand UnDoReDoSystem система</param>
        public void AddUnDoReDoSysAsCommand(IUnDoReDoSystem unDoReDo)
        {
            unDoReDo.Date = DateTime.Now;
            unDoReDo.SetUnDoReDo_System(this);
            _UnDoCommands.Push((UnDoReDoSystem)unDoReDo);
            foreach (IJornalable element in unDoReDo.ChangedObjects)
                if (!this.ChangedObjects.Where(ob=>ob.StoredId==element.StoredId).Any())
                    this.ChangedObjects.Add(element);
            OnPropertyChanged("AddUnDoReDo");

        }
        /// <summary>
        /// Метод сохраняет изменения только в конкретном объекте в текущей и дочерних системах (удаляет информацию об изменениях в системе и участвоваваших
        /// в изменнеии данного других объектах) 
        /// </summary>
        /// <param name="obj"> Объкт,измененения котророго будут стеры из системы</param>
        /// <returns></returns>
        public int Save(IJornalable obj)
        {
            IJornalable saved_obj = obj;
            var all_objects_systems = ChildrenSystems.Where(s => s._UnDoCommands.Union(s._ReDoCommands).Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == saved_obj.StoredId).Any()).Any());
            foreach (IUnDoReDoSystem unDoReDo in all_objects_systems)//Если объект зарегисрирован в дочених системах...
            {
                unDoReDo.Save(saved_obj);//Сохраняем объект во всех дочерних системах
            }
            var commands_list = this._UnDoCommands.Union(_ReDoCommands).Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == saved_obj.StoredId).Any()).ToList(); // Все команды текущей системы в которых содержится сохраняемый объект
            foreach (IUnDoRedoCommand command in commands_list)///Проходим по командам 
            {
                List<IJornalable> chgd_objects = command.ChangedObjects.Where(ob => ob.StoredId != saved_obj.StoredId).ToList();//Находим в командах все объекты с которыми он был связан
                foreach (IJornalable chgd_obj in chgd_objects)
                {
                    chgd_obj.ChangesJornal.Remove(command);///Удаляем в найденных объектах информацию о найденных командах
                    if (!chgd_obj.ChangesJornal.Where(cm => cm.UnDoReDo_System == this).Any())//Если в текущей системе больше нет 
                        this.ChangedObjects.Remove(chgd_obj);
                }
                saved_obj.ChangesJornal.Remove(command);//Удаляем команду в текущем объекте
                this._UnDoCommands.Remove(command); //Удаляем команду из   стека UnDo ...
                if (command is IUnDoReDoSystem 
                    undoredo_command) undoredo_command.SaveAll();
                this._ReDoCommands.Clear();///Очищаем стек  ReDo
            }
            if (this.ChangedObjects.Where(ob => ob.StoredId == saved_obj.StoredId).Any())
                this.ChangedObjects.Remove(saved_obj);///Удаляем объект из журнала системы, где хараняться наблюдаемые объекты 
                                                      ///в которых были зафиксированы изменения
            if (obj is IList list_obj) //Если регистрируемый элемент является коллекцией
                foreach (IJornalable element in list_obj)
                    this.Save(element);
            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);

            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                {
                    this.Save(jornable_prop);
                }
            }

            OnPropertyChanged("Save");
            return 1;
        }

        ///// <summary>
        ///// Мето проходт по всем зарегисрированным объектам и сохраняет все изменения в них
        ///// </summary>
        ///// <returns></returns>
        public int SaveAll()
        {

            foreach (IJornalable obj in this._AllChangedObjects)
            {
                this.Save(obj);
            }
            OnPropertyChanged("SaveAll");

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

        public IUnDoReDoSystem UnDoReDo_System { get; protected set; }
        #endregion

        public void AdjustChangedProperty(
            bool b_jornal_recording_flag,
            string propertyName,
            ref object member,
            object val,
            PropertyBeforeChangeEventHandler PropertyBeforeChanged, IEntityObject entity)
        {
            if (b_jornal_recording_flag)
                PropertyBeforeChanged(this, new PropertyBeforeChangeEvantArgs(propertyName, member, val));

            if (member is IEntityObject entity_member)
            {
                if (entity_member is INameableObservableCollection nameble_collection_mamber)
                {
                    nameble_collection_mamber.Owner = entity;
                }
                if (!entity_member.Parents.Where(ob => ob.StoredId == entity.StoredId).Any()) entity_member.Parents.Add(entity);
                if (!entity.Children.Where(ob => ob.StoredId == entity_member.StoredId).Any()) entity.Children.Add(entity_member);
                if (entity.IsAutoRegistrateInUnDoReDo) entity.UnDoReDoSystem?.Register(entity_member, true);
            }
        }
        /// <summary>
        /// Метод возращает количство объектов внутри объекта, у которых были зарегистрированные в системе иземенения 
        /// и которые не сохранены в БД
        /// </summary>
        /// <param name="obj">Проверяемый объект </param>
        /// <returns></returns>
        public int GetUnChangesObjectsNamber(IJornalable obj, bool first_itr = true)
        {
            IJornalable saved_obj = obj;
            int changes_number = 0;
            var all_objects_systems = ChildrenSystems.Where(s => s._UnDoCommands.Union(s._ReDoCommands).Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == saved_obj.StoredId).Any()).Any());
            foreach (IUnDoReDoSystem unDoReDo in all_objects_systems)//Если объект зарегисрирован в дочених системах...
            {
                changes_number += unDoReDo.GetUnChangesObjectsNamber(saved_obj);//Сохраняем объект во всех дочерних системах
            }
            if (obj is IList list_obj) //Если регистрируемый элемент является коллекцией
                foreach (IJornalable element in list_obj)
                    changes_number += this.GetUnChangesObjectsNamber(element);

            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                {
                    changes_number += this.GetUnChangesObjectsNamber(jornable_prop);
                }
            }
            if(saved_obj.IsDbBranch&& saved_obj.State!= EntityState.Unchanged )
                 changes_number ++;
            return changes_number;
        }

        /// <summary>
        /// Метод возращает количство объектов внутри объекта, у которых были зарегистрированные в системе изменения 
        /// </summary>
        /// <param name="obj">Проверяемый объект </param>
        /// <returns></returns>
        public int GetChangesNamber(IJornalable obj, bool first_itr = true)
        {
            IJornalable saved_obj = obj;
            int changes_number = 0;
            var all_objects_systems = ChildrenSystems.Where(s => s._UnDoCommands.Union(s._ReDoCommands).Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == saved_obj.StoredId).Any()).Any());
            foreach (IUnDoReDoSystem unDoReDo in all_objects_systems)//Если объект зарегисрирован в дочених системах...
            {
                changes_number += unDoReDo.GetChangesNamber(saved_obj);//Сохраняем объект во всех дочерних системах
            }
            if (obj is IList list_obj) //Если регистрируемый элемент является коллекцией
                foreach (IJornalable element in list_obj)
                    changes_number += this.GetChangesNamber(element);

            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                {
                    changes_number += this.GetChangesNamber(jornable_prop);
                }
            }
            changes_number += saved_obj.ChangesJornal.Count;
            return changes_number;
        }
        private object first_object;
        /// <summary>
        /// Метод возращает количство объектов внутри объекта, у которых были зарегистрированные в системе изменения 
        /// </summary>
        /// <param name="obj">Проверяемый объект </param>
        /// <param name="first_itr">Служебный параметрт рекурсии. Не  определять!</param>
        /// <returns></returns>
        public int GetChangesNamber_1(IJornalable obj, bool first_itr = true)
        {
            int i_changes_namber = 0;
            if (first_itr) first_object = obj;
            if (!first_itr && obj == first_object)
            {
                first_object = null;
                return 0;
            }
            var all_objects_systems = ChildrenSystems.Where(s => s._UnDoCommands.Union(s._ReDoCommands).Where(cm => cm.ChangedObjects.Where(ob => ob.StoredId == obj.StoredId).Any()).Any());
            foreach (IUnDoReDoSystem unDoReDo in all_objects_systems)//Если объект зарегисрирован в дочених системах...
            {
                i_changes_namber += unDoReDo.GetChangesNamber(obj, false);
            }

            var props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);

            if (obj is IList list_obj) //Если регистрируемый элемент является коллекцией
                foreach (IJornalable element in list_obj)
                    i_changes_namber += this.GetChangesNamber(element, false);
            foreach (PropertyInfo propertyInfo in props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);
                var attr = propertyInfo.GetCustomAttribute<NotJornalingAttribute>();//Проверяем не помченно ли свойтво атрибутом [NotJornalin]
                if (prop_val is IJornalable jornable_prop && attr == null)//Если свойтво IJornable и не помчено атрибутом 
                {
                    i_changes_namber += this.GetChangesNamber(jornable_prop, false);
                }
            }



            i_changes_namber += obj.ChangesJornal.Count();

            return i_changes_namber;
        }

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
            //  this.SystemHaveNotSavedObjects += OnSystemHaveNotSavedObject;

        }

        private void OnSystemHaveNotSavedObject(IUnDoReDoSystem sender, UnDoReDoSystemEventArgs e)
        {

        }
    }
}