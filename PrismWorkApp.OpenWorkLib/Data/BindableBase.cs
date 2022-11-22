
using PrismWorkApp.OpenWorkLib.Core;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;


namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class BindableBase : INotifyPropertyChanged, IJornalable, IValidateable, IBindableBase, ICopingEnableable
    {

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event PropertyChangedEventHandler PropertyBeforeChanged = delegate { };


        public event ObjectStateChangeEventHandler ObjectChangedNotify;//Событие вызывается при изменении в данном объекте 
        public event ObjectStateChangeEventHandler ObjectChangeSaved; //Событие вызывается при сохранении изменений в данном объекте
        public event ObjectStateChangeEventHandler ObjectChangeUndo; //Событие вызывается при отмете изменений в данном объекте
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private Guid _storedId;
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        protected virtual bool BaseSetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {

            if (object.Equals(val, member)) return false;
           if(b_jornal_recording_flag)
                PropertyBeforeChanged(this, new PropertyChangedEventArgs(propertyName));
            member = val;
            //Type tp = Children[Children.Count - 1].GetType();
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        protected bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null, bool jornal_mode = false)
        {
            if (member != null)
                ValidateProperty(propertyName, val);
         //   PropertyChangesRegistrate(ref member, val, propertyName);
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
         private Guid _currentContextId;
        [NotMapped]
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set
            {
                b_jornal_recording_flag = false;
                SetProperty(ref _currentContextId, value);
                b_jornal_recording_flag = true;
            }
        }
        [NotMapped]
        public bool IsVisible
        {
            get { return visible; }
            set
            {
                b_jornal_recording_flag = false;
                SetProperty(ref visible, value);
                b_jornal_recording_flag = true;
            }
        }
    
        
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
            //PropertiesChangeJornal = new PropertiesChangeJornal();
            //PropertiesChangeJornal.ParentObject = this;
            //     31.10.2022   PropertiesChangeJornal.JornalChangedNotify += OnPropertyChanges;
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

        [NotMapped]
        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе (например копирования рефлексией) с данныv объектом по умолчанию 
        [NotMapped]
        public bool CopingEnable { get; set; } = true;
        

        public virtual void SetCopy<TSourse>(object pointer, Func<TSourse, bool> predicate)
            where TSourse : IEntityObject
        {
            Functions.CopyObjectReflectionNewInstances(this, pointer, predicate);
            Functions.SetAllIdToZero(pointer);
        }
        public virtual object Clone<TSourse>(Func<TSourse, bool> predicate) where TSourse : IEntityObject
        {
            if (!CopingEnable)
                return null;
            object new_object = Activator.CreateInstance(this.GetType());
            Functions.GetCopyEntitityObject(this, new_object, predicate);
            Functions.SetAllIdToZero(new_object);
            return new_object;
        }



    }


}

