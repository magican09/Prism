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
    public abstract class BindableBase : IBindableBase, IEntityObject, ICloneable
    {

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged = delegate { };
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated = delegate { };
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

        #region Changes Jornaling

        private bool b_jornal_recording_flag = true;
        private bool visible = true;
        //private Guid _currentContextId;
        //[NotMapped]
        //public Guid CurrentContextId
        //{
        //    get { return _currentContextId; }
        //    set
        //    {
        //        b_jornal_recording_flag = false;
        //        SetProperty(ref _currentContextId, value);
        //        b_jornal_recording_flag = true;
        //    }
        //}
        //[NotMapped]
        //public bool IsVisible
        //{
        //    get { return visible; }
        //    set
        //    {
        //        b_jornal_recording_flag = false;
        //        SetProperty(ref visible, value);
        //        b_jornal_recording_flag = true;
        //    }
        //}

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
        public BindableBase()
        {
        }

        [NotMapped]
        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе (например копирования рефлексией) с данныv объектом по умолчанию 
        [NotMapped]
        public bool CopingEnable { get; set; } = true;

        public Guid? ParentId { get;set;}
        private BindableBase? _parent;
       // [NotMapped]
        [NavigateProperty]
        public BindableBase? Parent
        {
            get { return _parent; }
            set
            {
                SetProperty(ref _parent, value);
             ///   if (_parent != null && !_parent.Children.Contains(_parent)) _parent.Children.Add(this);
                //foreach (IBindableBase elm in Children)
                //    if(!Children.Contains(_parent)) elm.Parent = _parent;
            }
        }
        private ObservableCollection<BindableBase> _children = new ObservableCollection<BindableBase>();
       // [NotMapped]
        [NavigateProperty]
        public ObservableCollection<BindableBase> Children
        {
            get { return _children; }
            set { _children = value; }
        }

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
                object[] no_copy__attributes = member_info[0].GetCustomAttributes(typeof(CreateNewWhenCopyAttribute), false); //Проверяем нет ли у свойство атрибута против копирования
                object[] navigate__attributes = member_info[0].GetCustomAttributes(typeof(NavigatePropertyAttribute), false); //Проверяем нет ли у свойство атрибута против копирования


                if (!prop_info.PropertyType.FullName.Contains("System"))
                {
                    if (prop_val != null)
                    {
                        if (no_copy__attributes.Length == 0 && navigate__attributes.Length == 0) //Если объяет свойство не навигационный и без запрета накопирование  
                        {
                            if (prop_val is ICloneable clonable_prop_val && prop_info.SetMethod != null)
                                prop_info.SetValue(new_object, clonable_prop_val.Clone());
                            else if (prop_val is IList prop_val_list)
                            {
                                foreach (object element in prop_val_list)
                                {
                                    (new_object_prop_val as IList).Add(element);
                                }
                            }
                            else
                               if (prop_info.SetMethod != null) prop_info.SetValue(new_object, prop_val);

                        }
                        if (no_copy__attributes.Length != 0 && navigate__attributes.Length == 0 && prop_info.SetMethod != null) //Если стоит атрибут "создать новый при копировании"
                        {
                            prop_val = Activator.CreateInstance(prop_info.PropertyType);
                            prop_info.SetValue(new_object, prop_val);
                        }
                        if (navigate__attributes.Length != 0 && prop_info.SetMethod != null) //Если свойство навигационное 
                        {
                            prop_info.SetValue(new_object, null);
                        }
                    }
                }
                else
                    if (no_copy__attributes.Length == 0 && prop_info.SetMethod != null)
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

