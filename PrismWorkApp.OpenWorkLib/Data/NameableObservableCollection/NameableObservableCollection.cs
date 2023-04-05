using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Data
{

    public class NameableObservableCollection<TEntity> : ObservableCollection<TEntity>, INameableObservableCollection<TEntity>, ICloneable, INotifyCollectionChanged, IEntityObject
        where TEntity : IEntityObject
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void InvokeUnDoReDoCommandCreatedEvent(IUnDoRedoCommand command)
        {
            UnDoReDoCommandCreated.Invoke(this, new UnDoReDoCommandCreateEventsArgs(command));
        }
        protected virtual bool BaseSetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {
           if (object.Equals(val, member)) return false;
            if (b_jornal_recording_flag)
            {
                PropertyBeforeChanged(this, new PropertyBeforeChangeEvantArgs(propertyName, member, val));
            }
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        protected bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null, bool jornal_mode = false)
        {
            if (member != null)
                ValidateProperty(propertyName, val);
            return BaseSetProperty<T>(ref member, val, propertyName);
        }
        #region Properties
        private Guid _id = Guid.NewGuid();
        [CreateNewWhenCopy]
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private Guid _storedId;
        [CreateNewWhenCopy]
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set { SetProperty(ref _code, value); }
        }//Код
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion
        #region Validating
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return null;
        }
        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }
        private void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this);
            context.MemberName = propertyName;
            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {

                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
        #region Constructors
        public NameableObservableCollection()
        {
            Id = Guid.NewGuid();
            CollectionChanged += OnCollectionChangedMethod;

        }
        public NameableObservableCollection(string name) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
            CollectionChanged += OnCollectionChangedMethod;
        }
        public NameableObservableCollection(List<TEntity> list) : base(list)
        {
            Id = Guid.NewGuid();
            CollectionChanged += OnCollectionChangedMethod;
        }
        public NameableObservableCollection(IList<TEntity> list) : base(list)
        {
            Id = Guid.NewGuid();
            CollectionChanged += OnCollectionChangedMethod;
        }
        public NameableObservableCollection(ICollection<TEntity> collection) : base(collection)
        {
            Id = Guid.NewGuid();
            CollectionChanged += OnCollectionChangedMethod;

        }
        public NameableObservableCollection(IEnumerable<TEntity> entities) : base(entities)
        {
            Id = Guid.NewGuid();
            CollectionChanged += OnCollectionChangedMethod;
        }
        #endregion
        #region  IJornaling service
        private ObservableCollection<IUnDoRedoCommand> _changesJornal = new ObservableCollection<IUnDoRedoCommand>();
        [NotJornaling]
        public ObservableCollection<IUnDoRedoCommand> ChangesJornal
        {
            get { return _changesJornal; }
            set { SetProperty(ref _changesJornal, value); }
        }
        private bool b_jornal_recording_flag = true;
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged = delegate { };
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated = delegate { };
    
        public void JornalingOff()
        {
            if (b_jornal_recording_flag == true)
                b_jornal_recording_flag = false;

        }
        public void JornalingOn()
        {
            if (b_jornal_recording_flag == false)
                b_jornal_recording_flag = true;
        }
        #endregion
        public Func<TEntity, bool> SortPridicate;
        public string SortedPropertyName;
    
        private void OnCollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
          
            if (e.Action == NotifyCollectionChangedAction.Add && !b_jornal_recording_flag)
            {
                foreach (IEntityObject added_element in e.NewItems)
                    added_element.Parents.Add(Owner);
            }

        }

        public object Clone()
        {
            IList new_collection = (IList)Activator.CreateInstance(this.GetType());
                 var prop_infoes = new_collection.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop_info in prop_infoes)
            {
                var prop_val = prop_info.GetValue(new_collection);
                var member_info = this.GetType().GetMember(prop_info.Name);
                object[] no_copy__attributes = member_info[0].GetCustomAttributes(typeof(CreateNewWhenCopyAttribute), false); //Проверяем нет ли у свойство атрибута против копирования
                object[] navigate__attributes = member_info[0].GetCustomAttributes(typeof(NavigatePropertyAttribute), false); //Проверяем нет ли у свойство атрибута против копирования

                if (!prop_info.PropertyType.FullName.Contains("System") && prop_info.SetMethod != null)
                {
                    if (prop_val != null)
                    {
                        if (no_copy__attributes.Length == 0 && navigate__attributes.Length == 0) //Если объяет свойство не навигационный и без запрета накопирование  
                        {
                            if (prop_val is ICloneable clonable_prop_val)
                                prop_info.SetValue(new_collection, clonable_prop_val.Clone());
                            else
                                prop_info.SetValue(new_collection, prop_val);

                        }
                        if (no_copy__attributes.Length > 0 && navigate__attributes.Length == 0) //Если стоит атрибут "создать новый при копировании"
                        {
                            prop_val = null;
                            prop_val = Activator.CreateInstance(prop_info.PropertyType);
                            prop_info.SetValue(new_collection, prop_val);
                        }
                        if (navigate__attributes.Length > 0) //Если свойство навигационное 
                        {
                            prop_val = null;
                            prop_info.SetValue(new_collection, prop_val);
                        }
                    }
                    else
                        if (no_copy__attributes.Length == 0)
                        prop_info.SetValue(new_collection, prop_val);
               }
            }

            foreach (TEntity element in this)
            {
                new_collection.Add(element);
            }
             ((IKeyable)new_collection).Id = Guid.Empty;
            return new_collection;
        }
         public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе с данных объектом по умолчанию
     
        private IEntityObject _owner;
        public IEntityObject Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        #region  IHierarchical
        public ObservableCollection<IEntityObject> _parents = new ObservableCollection<IEntityObject>();
        [NotMapped]
        [NavigateProperty]
        public ObservableCollection<IEntityObject> Parents
        {
            get { return _parents; }
            set
            {
                SetProperty(ref _parents, value);
            }
        }
        private ObservableCollection<IEntityObject> _children = new ObservableCollection<IEntityObject>();
        [NotMapped]
        [NavigateProperty]
        public ObservableCollection<IEntityObject> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        #endregion
 
        protected override void SetItem(int index, TEntity item)
        {
            if (b_jornal_recording_flag)
            {
                AddItemCommand<TEntity> Command = new AddItemCommand<TEntity>(item, this);
                InvokeUnDoReDoCommandCreatedEvent(Command);
            }
            else
            {
                base.SetItem(index, item);
            }
        }
        protected override void InsertItem(int index, TEntity item)
        {

            if (b_jornal_recording_flag)
            {
                InsertItemCommand<TEntity> Command = new InsertItemCommand<TEntity>(index,item, this);
                InvokeUnDoReDoCommandCreatedEvent(Command);
            }
            else
            {
                base.InsertItem(index, item);
            }

        }

        protected override void RemoveItem(int index)
        {
            if (b_jornal_recording_flag)
            {
                RemoveItemCommand<TEntity> Command = new RemoveItemCommand<TEntity>(this[index], this);
                InvokeUnDoReDoCommandCreatedEvent(Command);
            }
            else
            {
                base.RemoveItem(index);
            }
        }
    }
}
