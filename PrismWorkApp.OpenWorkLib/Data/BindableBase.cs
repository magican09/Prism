using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;


namespace PrismWorkApp.OpenWorkLib.Data
{
    public abstract class BindableBase : IBindableBase, IEntityObject
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
                //return StructureLevel.Code;
            }
            set { SetProperty(ref _code, value); }
        }//Код
        private string _name;
        public string Name
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
            //Type tp = Children[Children.Count - 1].GetType();
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

        private IBindableBase _parent;
        [NotMapped]
        public IBindableBase Parent
        {
            get { return _parent; }
            set
            {

                SetProperty(ref _parent, value);
                if (_parent!=null && !_parent.Children.Contains(_parent)) _parent.Children.Add(this);
                //foreach (IBindableBase elm in Children)
                //    if(!Children.Contains(_parent)) elm.Parent = _parent;
                  
            }
        }
        private ObservableCollection<IBindableBase> _children = new ObservableCollection<IBindableBase>();
        [NotMapped]
        public ObservableCollection<IBindableBase> Children
        {
            get { return _children; }
            set { _children = value; }
        }

    }


}

