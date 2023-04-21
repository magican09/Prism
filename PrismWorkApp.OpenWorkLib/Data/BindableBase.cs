using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class BindableBase : IBindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged = delegate { };
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated = delegate { };
        public event SaveChangesEventHandler SaveChanges;
        public event SaveChangesEventHandler SaveAllChanges;
        /// <summary>
        /// Вызывается для добалвения UnDoRedo команды в систему UnDoRedo
        /// </summary>
        /// <param name="command"></param>
        public void InvokeUnDoReDoCommandCreatedEvent(IUnDoRedoCommand command)
        {
            UnDoReDoCommandCreated.Invoke(this, new UnDoReDoCommandCreateEventsArgs(command));
        }
        private Guid _id;
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
                //return StructureLevel.Code;
            }
            set { SetProperty(ref _code, value); }
        }//Код
        private string _name;
        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #region NotyfyPropertyChaged
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
            if (member is IEntityObject entity_member)
            {
                if (entity_member is INameableObservableCollection nameble_collection_mamber)
                {
                    nameble_collection_mamber.Owner = this;
                }
                if (!entity_member.Parents.Contains(this)) entity_member.Parents.Add(this);
                if (!this.Children.Contains(entity_member)) this.Children.Add(entity_member);
                if (IsAutoRegistrateInUnDoReDo && UnDoReDoSystem != null && !UnDoReDoSystem.IsRegistered(entity_member))
                    UnDoReDoSystem.Register(entity_member, true, entity_member.IsDbBranch | this.IsDbBranch);
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
        [CreateNewWhenCopy]
        [NotJornaling]
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

        #region IJornaling


        private bool _isDbBranch = false;
        [NotMapped]
        [NotJornaling]
        public bool IsDbBranch
        {
            get { return _isDbBranch; }
            set { SetProperty(ref _isDbBranch, value); }
        }

        public EntityState _state;
        [NotMapped]
        [NotJornaling]
        public EntityState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }
        private IUnDoReDoSystem _unDoReDoSystem;
        [NotJornaling]
        [NotMapped]
        [CreateNewWhenCopy]
        public IUnDoReDoSystem UnDoReDoSystem
        {
            get { return _unDoReDoSystem; }
            set { _unDoReDoSystem = value; }
        }
        [NotJornaling]
        [NotMapped]
        public bool IsAutoRegistrateInUnDoReDo { get; set; } = false;
        private ObservableCollection<IUnDoRedoCommand> _changesJornal = new ObservableCollection<IUnDoRedoCommand>();
        [NotJornaling]
        [NotMapped]
        [CreateNewWhenCopy]
        public ObservableCollection<IUnDoRedoCommand> ChangesJornal
        {
            get { return _changesJornal; }
            set { _changesJornal = value; }
        }

        private bool b_jornal_recording_flag = false;
        public void JornalingOff()
        {
            if (b_jornal_recording_flag == true)
                b_jornal_recording_flag = false;
        }
        public void JornalingOn()
        {
            if (b_jornal_recording_flag == false && UnDoReDoSystem != null)
                b_jornal_recording_flag = true;
        }


        #endregion
        public BindableBase()
        {
        }
        [NotMapped]
        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе (например копирования рефлексией) с данныv объектом по умолчанию 

        #region  IHierarchical
        public ObservableCollection<IEntityObject> _parents = new ObservableCollection<IEntityObject>();
        [NavigateProperty]
        [NotMapped]
        [NotJornaling]
        public ObservableCollection<IEntityObject> Parents
        {
            get { return _parents; }
        }

        private ObservableCollection<IEntityObject> _children = new ObservableCollection<IEntityObject>();
        [NavigateProperty]
        [NotMapped]
        [NotJornaling]
        public ObservableCollection<IEntityObject> Children
        {
            get
            {
                return _children;
            }
            set { }
        }

        #endregion

        public virtual object Clone()
        {
            BindableBase new_object = (BindableBase)Activator.CreateInstance(this.GetType());
            // new_object =(BindableBase) this.MemberwiseClone();
            new_object.Id = Guid.Empty;
            var prop_infoes = new_object.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop_info in prop_infoes)
            {
                var prop_val = prop_info.GetValue(this);
                var new_object_prop_val = prop_info.GetValue(new_object);
                var member_info = this.GetType().GetMember(prop_info.Name);
                var create_new_atrb = prop_info.GetCustomAttribute<CreateNewWhenCopyAttribute>();
                var navigate_atrb = prop_info.GetCustomAttribute<NavigatePropertyAttribute>();

                if (!prop_info.PropertyType.FullName.Contains("System"))
                {
                    if (prop_val != null)
                    {
                        if (create_new_atrb == null && navigate_atrb == null) //Если объяет свойство не навигационный и  без указания на создания новго объекта   
                        {
                            if (prop_val is IList prop_val_list)
                            {
                                if (prop_val_list is ICloneable clonable_prop_val_list)
                                    new_object_prop_val = clonable_prop_val_list.Clone();
                                else
                                    foreach (object element in prop_val_list)
                                        (new_object_prop_val as IList).Add(element);
                                prop_info.SetValue(new_object, new_object_prop_val);

                            }
                            else
                                if (prop_info.SetMethod != null) prop_info.SetValue(new_object, prop_val);

                        }
                        if (create_new_atrb != null && navigate_atrb == null && prop_info.SetMethod != null) //Если стоит атрибут "создать новый при копировании"
                        {
                            prop_val = Activator.CreateInstance(prop_info.PropertyType);
                            prop_info.SetValue(new_object, prop_val);
                        }
                        if (navigate_atrb != null && prop_info.SetMethod != null) //Если свойство навигационное 
                            prop_info.SetValue(new_object, null);
                    }
                }
                else
                    if (create_new_atrb == null && prop_info.SetMethod != null)
                    prop_info.SetValue(new_object, prop_val);


            }
            return new_object;
        }


        //public virtual object Clone()
        //{
        //    BindableBase new_object = (BindableBase)Activator.CreateInstance(this.GetType());
        //    // new_object =(BindableBase) this.MemberwiseClone();
        //    new_object.Id = Guid.Empty;
        //    var prop_infoes = new_object.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
        //    ObservableCollection<PropertyInfo> CreateNewWhenCopyProps = new ObservableCollection<PropertyInfo>();
        //    ObservableCollection<PropertyInfo> OtherProps = new ObservableCollection<PropertyInfo>();
        //    foreach (PropertyInfo prop_info in prop_infoes)
        //    {
        //        var prop_val = prop_info.GetValue(this);
        //        var member_info = this.GetType().GetMember(prop_info.Name);
        //        object[] no_copy__attributes = member_info[0].GetCustomAttributes(typeof(CreateNewWhenCopyAttribute), false); //Проверяем нет ли у свойство атрибута против копирования
        //        if (no_copy__attributes.Length != 0)
        //        {
        //            CreateNewWhenCopyProps.Add(prop_info);
        //            prop_val = Activator.CreateInstance(prop_info.PropertyType);
        //            if (prop_info.SetMethod != null)
        //                prop_info.SetValue(new_object, prop_val);
        //            if (prop_val is IList)
        //            {

        //            }

        //        }
        //        else
        //            OtherProps.Add(prop_info);
        //    }

        //    foreach (PropertyInfo prop_info in prop_infoes)
        //    {
        //        var prop_val = prop_info.GetValue(this);
        //        var member_info = this.GetType().GetMember(prop_info.Name);
        //        object[] navigate__attributes = member_info[0].GetCustomAttributes(typeof(NavigatePropertyAttribute), false); //Проверяем нет ли у свойство атрибута против копирования

        //        if (prop_val != null && !prop_info.PropertyType.FullName.Contains("System"))
        //        {
        //            if (navigate__attributes.Length == 0) //Если объяет свойство не навигационный и без запрета накопирование  
        //            {
        //                if (prop_val is ICloneable clonable_prop_val)
        //                    prop_info.SetValue(new_object, clonable_prop_val.Clone());
        //                else
        //                    prop_info.SetValue(new_object, prop_val);
        //            }
        //            else  //Если свойство навигационное 
        //            {
        //                prop_val = null;
        //                prop_info.SetValue(new_object, prop_val);
        //            }

        //        }
        //        else
        //            if (prop_info.SetMethod != null) prop_info.SetValue(new_object, prop_val);
        //    }
        //    return new_object;
        //}
        // public Guid? CategoryId { get; set; }
        //  public EntityCategory? Category { get; set; }
    }


}

