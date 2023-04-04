using Prism;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class UnDoReDoSystem : IUnDoReDoSystem, IUnDoRedoCommand,IActiveAware
    {
        public bool IsActive { get; set; }
        private bool _monitorCommandActivity;
        public event EventHandler IsActiveChanged;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event EventHandler CanExecuteChanged;

        private Stack<IUnDoRedoCommand> _UnDoCommands = new Stack<IUnDoRedoCommand>();
        private Stack<IUnDoRedoCommand> _ReDoCommands = new Stack<IUnDoRedoCommand>();
        private int _UnDoCounter = 0;
        private int _ReDoCounter = 0;
        public ObservableCollection<IJornalable> _RegistedModels { get; set; } = new ObservableCollection<IJornalable>();

       
        public Guid Id { get; set; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IUnDoReDoSystem ParentUnDoReDo { get; set; }
        public ObservableCollection<IUnDoReDoSystem> ChildrenSystems = new ObservableCollection<IUnDoReDoSystem>();
        public bool UnDo(int levels,bool without_redo = false)//bool without_redo = false возможено не понадобиться (отлючение реду)
        {
            if (_monitorCommandActivity && !IsActive) return false;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDo"));
            return (_UnDoCommands.Count > 0) ? true : false;
        }
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReDo"));
            return (_ReDoCommands.Count > 0) ? true : false;

        }
        public void UnDoAll()
        {

            while (_UnDoCommands.Count > 0)
            {
                UnDo(1);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnDoAll"));
        }
   
        public bool AllUnDoIsDone()
        {
            return _UnDoCommands.Count == 0;
        }
        public bool IsSatcksEmpty()
        {
            return _UnDoCommands.Count == 0 && _ReDoCommands.Count == 0;
        }
        public void ClearStacks()
        {
            _UnDoCommands.Clear();
            _ReDoCommands.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClearStacks"));
        }
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
        private Dictionary<UnDoReDoSystem,IList<IJornalable>> unregisted_from_parentSystem_objects = new Dictionary<UnDoReDoSystem, IList<IJornalable>>();
        public void SetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system)
        {
            //Если в системе регистриуюется дочерняя система, то объекты которые зарегирированы в дочерней системе
            //удаляем и родительской системы, что бы не дублировались комады undo_redo
            children_system.ParentUnDoReDo = this;
            ChildrenSystems.Add(children_system);
            unregisted_from_parentSystem_objects.Add(children_system as UnDoReDoSystem, new List<IJornalable>());
            foreach (IJornalable reg_obj in children_system._RegistedModels)
                if (_RegistedModels.Contains(reg_obj))
                {
                    this.UnRegister(reg_obj);
                    unregisted_from_parentSystem_objects[children_system as UnDoReDoSystem].Add(reg_obj);
                }
        }
        public void UnSetChildrenUnDoReDoSystem(IUnDoReDoSystem children_system)
        {
            //Если в системе   дочерней системе были объекты из родительской, то 
            //обратно регистриуем их в родительской системе
            children_system.ParentUnDoReDo = null;
           if(ChildrenSystems.Contains(children_system))
                ChildrenSystems.Remove(children_system);
            foreach (IJornalable reg_obj in children_system._RegistedModels)
            {
                if (unregisted_from_parentSystem_objects[children_system as UnDoReDoSystem].Contains(reg_obj))
                {
                    this.Register(reg_obj);
                    unregisted_from_parentSystem_objects[children_system as UnDoReDoSystem].Remove(reg_obj);
                }
            }
            unregisted_from_parentSystem_objects.Remove(children_system as UnDoReDoSystem);

        }
        public void Register(IJornalable obj)
        {
            if (obj == null) { throw new Exception("Попытка регистрации в системе UnDoReDo объекта со значением null"); }
            if (_RegistedModels.Contains(obj))
                return;
            else
            {
                _RegistedModels.Add(obj);
                if (ParentUnDoReDo != null && ParentUnDoReDo._RegistedModels.Contains(obj)) //Если регистрируем объект, которые уже был зарегистирован в родительсокй системе
                    ParentUnDoReDo.UnRegister(obj);//то удаляем его из родительской системы
            }
            var obj_props_infoes = obj.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length==0);
            foreach (PropertyInfo propertyInfo in obj_props_infoes)
            {
                var prop_val = propertyInfo.GetValue(obj);

                if (prop_val is INotifyCollectionChanged && prop_val is IJornalable jornable_prop)
                    if (!_RegistedModels.Contains(jornable_prop))
                        this.Register(jornable_prop);
            }
            obj.PropertyBeforeChanged += OnModelPropertyBeforeChanged;
            obj.UnDoReDoCommandCreated += OnObservedCommandCreated;
           
        }

        private void OnIsActivateChaged(object sender, EventArgs e)
        {
            if(sender is IActiveAware active_aware_object)
            {
                IsActive = active_aware_object.IsActive;
                IsActiveChanged?.Invoke(this, new EventArgs());
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnIsActivateChagedChanged"));

            }
        }

        public void UnRegister(IJornalable obj)
        {
            if (_RegistedModels.Contains(obj))
            {
                obj.PropertyBeforeChanged -= OnModelPropertyBeforeChanged;
                obj.UnDoReDoCommandCreated -= OnObservedCommandCreated;
                _RegistedModels.Remove(obj);

                if (obj is IUnDoReDoSystem child_system) //Если в системе регистриуюется дочерняя система, то объекты которые зарегирированы в дочерней системе
                {//регистриуем в родительской   системе
                    child_system.ParentUnDoReDo = null;
                    foreach (IJornalable reg_obj in child_system._RegistedModels)
                    {
                        if (!_RegistedModels.Contains(reg_obj)) this.Register(reg_obj);
                    }
                }
            }
        }
        public void AddUnDoReDo(IUnDoReDoSystem unDoReDo)
        {
            _UnDoCommands.Push((UnDoReDoSystem)unDoReDo);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddUnDoReDo"));
        }
        private void OnObservedCommandCreated(object sender, UnDoReDoCommandCreateEventsArgs e)
        {
            _UnDoCommands.Push(e.Command);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnCommandCreated"));
        }
        private void OnModelPropertyBeforeChanged(object sender, PropertyBeforeChangeEvantArgs e)
        {
            PropertySetCommand setCommand =
                new PropertySetCommand(sender as IJornalable, e.PropertyName, e.NewValue, e.LastValue);
            _UnDoCommands.Push(setCommand);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnModelPropertyBeforeChanged"));
        }

        public void Execute(object parameter = null) //ReDo all UnDoed 
        {
            ReDo(_UnDoCounter);
        }
        public void UnExecute()
        {
            UnDoAll();
        }

        public bool CanExecute(object parameter)
        {
            return _UnDoCommands.Count > 0 || _ReDoCommands.Count > 0;
        }

        /// <summary>
        /// UnDoReDoSystem contructor 
        /// </summary>
        /// <param name="caller_obj">Caller object</param>
        /// <param name="monitorCommandActivity">IAtivaAware monitoring On/Off flag</param>
        public UnDoReDoSystem(object caller_obj = null,bool monitorCommandActivity=false)
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
