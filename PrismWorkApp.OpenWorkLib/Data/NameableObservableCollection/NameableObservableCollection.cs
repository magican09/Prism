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

    public class NameableObservableCollection<TEntity> : ObservableCollection<TEntity>, INameableObservableCollection, ICloneable, INotifyCollectionChanged, IEntityObject
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
            T old_member = member;
            member = val;
            if (member is IEntityObject entity_member)///Если какой либос свойство будет IEntityObject(IJornables)
            {
                if (entity_member is INameableObservableCollection nameble_collection_mamber)
                {
                    nameble_collection_mamber.Owner = this;
                }
                if (!entity_member.Parents.Contains(this)) entity_member.Parents.Add(this);
                if (!this.Children.Contains(entity_member)) this.Children.Add(entity_member);
                if (IsAutoRegistrateInUnDoReDo) UnDoReDoSystem?.RegisterAll(entity_member);
            }
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
        private Guid _storedId;
        private string _code;
        private string _name;
        [CreateNewWhenCopy]
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
         [CreateNewWhenCopy]
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
        public string Code
        {
            get
            {
                return _code;
            }
            set { SetProperty(ref _code, value); }
        }//Код
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion
        #region Validating
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };
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
        }
        public NameableObservableCollection(string name) : this()
        {
            Name = name;
        }
        public NameableObservableCollection(List<TEntity> list)
        {
            if (b_jornal_recording_flag)
            {

                AddListCommand<TEntity> Command = new AddListCommand<TEntity>(list, this);
                InvokeUnDoReDoCommandCreatedEvent(Command);
            }
            else
            {
                //var constructor = this.GetType().BaseType.GetConstructor(new Type[] { list.GetType() });
                //constructor.Invoke(new object[] { list });
                foreach (TEntity entity in list)
                    this.Add(entity);
            }
        }
        public NameableObservableCollection(IEnumerable<TEntity> entities) //: base(entities)
        {
            if (b_jornal_recording_flag)
            {

                AddListCommand<TEntity> Command = new AddListCommand<TEntity>(entities, this);
                InvokeUnDoReDoCommandCreatedEvent(Command);
            }
            else
            {

                foreach (TEntity entity in entities)
                {
                    this.Add(entity);
                }
            }
        }

        #endregion
        #region  IJornaling service
        private IUnDoReDoSystem _unDoReDoSystem;
        private ObservableCollection<IUnDoRedoCommand> _changesJornal = new ObservableCollection<IUnDoRedoCommand>();
        public event SaveChangesEventHandler SaveChanges;
        public event SaveChangesEventHandler SaveAllChanges;
        [NotJornaling]
        [NotMapped]
        public bool IsAutoRegistrateInUnDoReDo { get; set; } = false;
        [NotMapped]
        [CreateNewWhenCopy]
        public IUnDoReDoSystem UnDoReDoSystem
        {
            get { return _unDoReDoSystem; }
            set { _unDoReDoSystem = value; }
        }
         [NotJornaling]
        [CreateNewWhenCopy]
        public ObservableCollection<IUnDoRedoCommand> ChangesJornal
        {
            get { return _changesJornal; }
            set { _changesJornal = value; }
        }
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged = delegate { };
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated = delegate { };
        private bool b_jornal_recording_flag = false;
        public void JornalingOff()
        {
            if (b_jornal_recording_flag == true)
                b_jornal_recording_flag = false;

        }
        public void JornalingOn()
        {
            if (b_jornal_recording_flag == false && UnDoReDoSystem!=null)
                b_jornal_recording_flag = true;
        }
        #endregion
        public object Clone()
        {
            IList new_collection = (IList)Activator.CreateInstance(this.GetType());
            var prop_infoes = new_collection.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop_info in prop_infoes)
            {
                var prop_val = prop_info.GetValue(new_collection);
                var member_info = this.GetType().GetMember(prop_info.Name);
                var create_new_atrb = prop_info.GetCustomAttribute<CreateNewWhenCopyAttribute>();
                var navigate_atrb = prop_info.GetCustomAttribute<NavigatePropertyAttribute>();
                
                if (!prop_info.PropertyType.FullName.Contains("System") && prop_info.SetMethod != null)
                {
                    if (prop_val != null)
                    {
                        if (create_new_atrb == null && navigate_atrb  == null) //Если объяет свойство не навигационный и без запрета накопирование  
                        {
                            if (prop_val is ICloneable clonable_prop_val)
                                prop_info.SetValue(new_collection, clonable_prop_val.Clone());
                            else
                                prop_info.SetValue(new_collection, prop_val);

                        }
                        if (create_new_atrb != null   && navigate_atrb!=null) //Если стоит атрибут "создать новый при копировании"
                        {
                            prop_val = null;
                            prop_val = Activator.CreateInstance(prop_info.PropertyType);
                            prop_info.SetValue(new_collection, prop_val);
                        }
                        if (create_new_atrb != null) //Если свойство навигационное 
                        {
                            prop_val = null;
                            prop_info.SetValue(new_collection, prop_val);
                        }
                    }
                    else
                        if (create_new_atrb == null)
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
        [CreateNewWhenCopy]
        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе с данных объектом по умолчанию
        private IEntityObject _owner;
        [NotJornaling]
        [CreateNewWhenCopy]
        public IEntityObject Owner
        {
            get {
                if (_owner == null)//Если владельца списка нет - он сам свой владелец
                    return this;
                return _owner;
            }
            set {
               
                    if(this.Parents.Contains(Owner)) this.Parents.Remove(Owner);
                    _owner = value;
                    if (!this.Parents.Contains(_owner)) this.Parents.Add(_owner);
               
                }
        }
        #region  IHierarchical
        private ObservableCollection<IEntityObject> _children = new ObservableCollection<IEntityObject>();
        private ObservableCollection<IEntityObject> _parents = new ObservableCollection<IEntityObject>();
        [NavigateProperty]
        [NotJornaling]
        public ObservableCollection<IEntityObject> Parents
        {
            get { return _parents; }
            set
            {
                SetProperty(ref _parents, value);
            }
        }
        [NotMapped]
        [NavigateProperty]
        [NotJornaling]
        public ObservableCollection<IEntityObject> Children
        {
            get
            {
                //_children.Clear();
                //foreach (TEntity item in Items)
                //    _children.Add(item);
                return _children;
            }
            set { }
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
                if (!item.Parents.Contains(this)) item.Parents.Add(this);
                if (!this.Children.Contains(item)) this.Children.Add(item);
                base.SetItem(index, item);
            }
            if (IsAutoRegistrateInUnDoReDo) UnDoReDoSystem?.RegisterAll(item);
        }
        protected override void InsertItem(int index, TEntity item)
        {

            if (b_jornal_recording_flag)
            {
                InsertItemCommand<TEntity> Command = new InsertItemCommand<TEntity>(index, item, this);
                InvokeUnDoReDoCommandCreatedEvent(Command);
            }
            else
            {
                if (!item.Parents.Contains(this)) item.Parents.Add(this);
                if (!this.Children.Contains(item)) this.Children.Add(item);
                base.InsertItem(index, item);
            }
            if (IsAutoRegistrateInUnDoReDo) UnDoReDoSystem?.RegisterAll(item);
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
                TEntity item =Items[index];
                if (item.Parents.Contains(this)) item.Parents.Remove(this);
                if (this.Children.Contains(item)) this.Children.Remove(item); ;
                base.RemoveItem(index);
            }
        }


    }
}
