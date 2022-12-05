using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.OpenWorkLib.Data
{
    //public class NameableObservableCollection<TEntity>:ObservableCollection<TEntity>,IList<TEntity> where TEntity: class,IEntityObject
    public class NameableObservableCollection<TEntity> : ObservableCollection<TEntity>, IEntityObject, IJornalable, INameableOservableCollection<TEntity> where TEntity : class, IEntityObject, IJornalable
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event PropertyBeforeChangeEventHandler PropertyBeforeChanged = delegate { };
        public event UnDoReDoCommandCreateEventHandler UnDoReDoCommandCreated = delegate { };
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
        private Guid _id = Guid.NewGuid();
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
        #region Changes Jornaling

        private bool b_jornal_recording_flag = true;
        private Guid _currentContextId;
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set { _currentContextId = value; }
        }
        private JornalRecordType status;
        public JornalRecordType Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                IsVisible = (status != JornalRecordType.REMOVED) ? true : false;
            }
        }
        //    public ObservableCollection<IJornalable> ParentObjects { get; set; }
        public  object ParentObject { get; set; }
        public ObservableCollection<IJornalable> ChildObjects { get; set; }
        public AdjustStatus AdjustedStatus { get; set; } = AdjustStatus.UNADJUSTED;
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
            IsVisible = Count > 0;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
          
            }
         }
        private object GetPropertyValue(string propName,object obj)
        {
            var obj_prop_val = obj.GetType().GetProperty(SortedPropertyName)?.GetValue(obj);

            return obj_prop_val;
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
        public virtual Func<IEntityObject, bool> RestrictionPredicate { get; set; } = x => true;//Предикат для ограничений при работе с данных объектом по умолчанию
        public bool IsPointerContainer { get; set; }
        public bool CopingEnable { get; set; } = true;




    }
}
