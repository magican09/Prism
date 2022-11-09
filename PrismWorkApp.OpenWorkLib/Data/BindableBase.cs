
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

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        protected virtual bool BaseSetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = "")
        {

            if (object.Equals(val, member)) return false;
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
        private JornalRecordStatus status;
        private Guid _currentContextId;
        [NotMapped]
        public Guid CurrentContextId
        {
            get { return _currentContextId; }
            set
            {
                var prop_info_coll = this.GetType().GetProperties(); //Устанавливается при инициализации в LocalBindableBase  и его потомков
                b_jornal_recording_flag = false;
                foreach (PropertyInfo propertyInfo in prop_info_coll)
                {
                    var prop_val = propertyInfo.GetValue(this);
                    if (prop_val is IList && prop_val is IJornalable) //Если в свойствах есть коллекции, то устанавливаем их CurrentContextId 
                    {
                        (prop_val as IJornalable).CurrentContextId = CurrentContextId;
                    }
                }
                b_jornal_recording_flag = true;
                if (PropertiesChangeJornal!=null && !PropertiesChangeJornal.ContextIdHistory.Contains(value))
                    PropertiesChangeJornal.ContextIdHistory.Add(value);
                _currentContextId = value;

            }
        }
        public PropertiesChangeJornal _propertiesChangeJornal;
        [NotMapped]
        public PropertiesChangeJornal PropertiesChangeJornal
        {
            get { return _propertiesChangeJornal; }
            set { _propertiesChangeJornal = value; }
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
        [NotMapped]
        public ObservableCollection<IJornalable> ParentObjects { get; set; }
        [NotMapped]
        public ObservableCollection<IJornalable> ChildObjects { get; set; }
        [NotMapped]
        public JornalRecordStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                IsVisible = (status != JornalRecordStatus.REMOVED) ? true : false;
            }
        }
        [NotMapped]
        public AdjustStatus AdjustedStatus { get; set; } = AdjustStatus.UNADJUSTED;

        public virtual void AllPropertiesModifyedChangeNotificate()
        {
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo pr_info in properties)
            {
                OnPropertyChanged(pr_info.Name);
            }
        }
        public bool IsPropertiesChangeJornalIsEmpty(Guid currentContextId)
        {
            if (PropertiesChangeJornal!=null && PropertiesChangeJornal.Count == 0)
                return true;
            else
                return false;
        }
        public void OnChildObjectChanges(object sender, ObjectStateChangedEventArgs e)
        {
            if (e != null && !PropertiesChangeJornal.Contains(e.PropertyStateRecord))
            {
                PropertiesChangeJornal.Add(e.PropertyStateRecord);
            }
            ObjectChangedNotify?.Invoke(this, e);
        }
        public void OnChildObjectChangeSaved(object sender, ObjectStateChangedEventArgs e)
        {
            if (e != null && PropertiesChangeJornal.Contains(e.PropertyStateRecord))
            {
                PropertiesChangeJornal.Remove(e.PropertyStateRecord);
                ObjectChangeSaved?.Invoke(this, e);
            }
            ObjectChangedNotify?.Invoke(this, null);
        }
        public void OnChildObjectChangeUndo(object sender, ObjectStateChangedEventArgs e)
        {
            if (PropertiesChangeJornal.Contains(e.PropertyStateRecord))
            {
                PropertiesChangeJornal.Remove(e.PropertyStateRecord);
                ObjectChangeUndo?.Invoke(this, e);
            }
            ObjectChangedNotify?.Invoke(this, null);
        }

        public void JornalingOff()
        {
            if (b_jornal_recording_flag == true)
            {
                b_jornal_recording_flag = false;

                /*  var prop_info_coll = this.GetType().GetProperties();
                  foreach (PropertyInfo propertyInfo in prop_info_coll)
                  {
                      var prop_val = propertyInfo.GetValue(this);
                      if (prop_val is IList && prop_val is IJornalable)
                      {
                          (prop_val as IJornalable).JornalingOff();

                      }
                  }*/
            }
        }
        public void JornalingOn()
        {
            if (b_jornal_recording_flag == false)
            {
                b_jornal_recording_flag = true;
                /*   var prop_info_coll = this.GetType().GetProperties();
                   foreach (PropertyInfo propertyInfo in prop_info_coll)
                   {
                       var prop_val = propertyInfo.GetValue(this);
                       if (prop_val is IList && prop_val is IJornalable)
                       {
                           (prop_val as IJornalable).JornalingOn();
                       }
                   }*/
            }
        }
        public void ClearChangesJornal()
        {
            var prop_infos = this.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in prop_infos)
            {
                var prop_val = propertyInfo.GetValue(this);

                if (prop_val is IJornalable && (prop_val as IJornalable).PropertiesChangeJornal.Count != 0)
                {
                    MethodInfo unDoAllMethod = propertyInfo.PropertyType.GetMethod("ClearChangesJornal");
                    object res = unDoAllMethod.Invoke(prop_val, new object[] { });
                }
            }
            ParentObjects?.Clear();
            ChildObjects?.Clear();
            PropertiesChangeJornal.Clear();
            PropertiesChangeJornal.ContextIdHistory.Clear();
        }

        public virtual void UnDo(PropertyStateRecord propertyState)
        {
            if (propertyState != null)
            {
                b_jornal_recording_flag = false; //Отключаем ведение жрунала, так как собираемся переустановить текущее значение без журналирования
                if (propertyState.ParentObject.Id == (this as IEntityObject).Id)//Изменения были в дочернем объекте текущего отбъекта
                {
                    var prop_info = this.GetType().GetProperty(propertyState.Name); //Достаем с помощью рефлексии данные о свойстве из текущего объекта
                    var prop_val = prop_info.GetValue(this);

                    if (prop_info.GetIndexParameters().Length == 0)//Если свойство не является индек..
                    {
                        prop_info.SetValue(this, propertyState.Value); //Присваиваем свойству текущего объекта сохраненное в журнале значение
                    }
                    PropertiesChangeJornal.Remove(propertyState); //Удаляем из журнала сохраненное изменение
                    ObjectChangeUndo?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));

                }
                else
                {
                    propertyState.ParentObject.UnDo(propertyState);
                    PropertiesChangeJornal.Remove(propertyState);
                    ObjectChangeUndo?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                }
                b_jornal_recording_flag = true; //Включаем журналирование

            }
        }
        public virtual void UnDoLast(Guid currentContextId)
        {
             PropertyStateRecord last_record =
                         PropertiesChangeJornal.Where(pr => pr.ContextId == currentContextId).OrderByDescending(pr => pr.Date).LastOrDefault();
            UnDo(last_record);
        }

        public virtual void UnDoAll(Guid currentContextId)
        {
            List<PropertyStateRecord> records =
              PropertiesChangeJornal.Where(pr => pr.ContextId == currentContextId).OrderByDescending(pr => pr.Date).ToList();
            foreach (PropertyStateRecord record in records)
                UnDo(record); //Отменяем измеенения всех переменных, которые не коллекции
            PropertiesChangeJornal.ContextIdHistory.Remove(currentContextId);
        }
        public virtual void Save(object prop_id, Guid currentContextId)
        {
            /* List<PropertyStateRecord> propertyStateRecords =
                            PropertiesChangeJornal.Where(p => ((IKeyable)p.Value).Id == (Guid)prop_id && p.ContextId == currentContextId).ToList(); // Находим все записи о объекте в журнале
             */
            //Сохранение текущего состояние свойства заключается в том, что мы просто удаляем все предыдущие изменнеия из журнала изменений
            if (prop_id != null)
            {
                PropertyStateRecord propertyState;
                if (prop_id is string)//Если пытаемся сохранить изменения в простом свойстве - нам передали имя свойства.
                {
                    propertyState = PropertiesChangeJornal.Where(p => p.Name == prop_id.ToString() && p.ContextId == currentContextId).OrderBy(pr => pr.Date).LastOrDefault();
                    if (propertyState != null)
                    {
                        if (propertyState.Value is IEntityObject)
                            propertyState.ParentObject.SaveAll(currentContextId);
                        PropertiesChangeJornal.Remove(propertyState);
                        ObjectChangeSaved?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));
                    }
                }
                else
                {
                    //Ищем в журнале запись с изменениями сохраняемого свойства
                    propertyState = PropertiesChangeJornal.Where(p => p.Id == (Guid)prop_id).OrderBy(pr => pr.Date).LastOrDefault(); //

                    if (propertyState.ParentObject.Id != (this as IEntityObject).Id)//Изменения были в дочернем объекте текущего отбъекта
                        propertyState.ParentObject.SaveAll(currentContextId);
                    PropertiesChangeJornal.Remove(propertyState);
                    ObjectChangeSaved?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyState));//Оповещаем подписантов на этом собитие о том, что сохранили изменение

                }
                List<PropertyStateRecord> recordsFoDelete =//Удаляем из журнала предыдущие состояния свойства сделанные их текущего контекста окна. 
                                        PropertiesChangeJornal.Where(p => p.Name == prop_id.ToString() && p.ContextId == currentContextId).ToList();

                foreach (PropertyStateRecord record in recordsFoDelete)
                {
                    PropertiesChangeJornal.Remove(record);
                    ObjectChangeSaved?.Invoke(this, new ObjectStateChangedEventArgs("", this, record));//Оповещаем подписантов на этом собитие о том, что сохранили изменение
                }
            }
        }
        public virtual void SaveAll(Guid currentContextId)
        {
            List<string> uniq_property_names = //Получаем имена свойство, которые подвергались изменениям
                   PropertiesChangeJornal.GroupBy(g => g.Name).Select(x => x.First()).Select(jr => jr.Name).ToList();
            foreach (string prop_name in uniq_property_names)//Сохраняем последние изменения для каждого свойства
                Save(prop_name, currentContextId);

            /*   List<Guid> uniq_property_ids = //Получаем id элементов, которые подвергались изменениям
               PropertiesChangeJornal.Where(r => r.Value != null).GroupBy(g => g.ContextId).Select(x => x.First()).Select(jr => ((IEntityObject)jr.Value).Id).ToList();
               foreach (Guid prop_id in uniq_property_ids)
                   Save(prop_id, currentContextId);*/


            PropertiesChangeJornal.ContextIdHistory.Remove(currentContextId);

        }

        private void PropertyChangesRegistrate<T>(ref T member, T newVal, string propertyName)
        {
            var navigate_props_infos = Functions.GetNavigateProperties(this); //Получаем список навигационных свойств текущего объкта

            if (b_jornal_recording_flag &&
                navigate_props_infos.Where(pri => pri.Name == propertyName).FirstOrDefault() == null) //Регистрация сделанных изменений в журнал изменений
            {
                if (member is IJornalable || newVal is IJornalable)
                {
                    if (member != null && member is IJornalable && newVal != null) // Если свойству присваивают другой объект и они оба не null
                    {
                        IJornalable property_member = member as IJornalable;
                        IJornalable property_new_val = newVal as IJornalable;

                        if (property_member.ParentObjects == null) property_member.ParentObjects = new ObservableCollection<IJornalable>();

                        if (property_member.ParentObjects.Contains(this))
                            property_member.ParentObjects.Remove(this); //Удаляемся из объектса  свойств 
                        property_member.ObjectChangedNotify -= OnChildObjectChanges;  //Отписвыаемся от изменений в объете который был в качестве свойства 
                        property_member.ObjectChangeSaved -= OnChildObjectChangeSaved;  //Отписвыаемся от изменений  
                        property_member.ObjectChangeUndo -= OnChildObjectChangeSaved;  //Отписвыаемся от изменений 

                        if (property_new_val.ParentObjects == null) property_new_val.ParentObjects = new ObservableCollection<IJornalable>();
                        if (!property_new_val.ParentObjects.Contains(this))
                            property_new_val.ParentObjects.Add(this); //Добавляем текущий объекта в качестве родительского для нового значения 

                        property_new_val.ObjectChangedNotify += OnChildObjectChanges; //Добавляем обработски изменений в новомо объекте 
                        property_new_val.ObjectChangeSaved += OnChildObjectChangeSaved; //Добавляем обработски изменений в новомо объекте 
                        property_new_val.ObjectChangeUndo += OnChildObjectChangeSaved; //Добавляем обработски изменений в новомо объекте 
                    }
                    if (member == null && newVal != null && newVal is IJornalable) // Если свойству присваивают другой объект и  он не  null
                    {
                        IJornalable property_new_val = newVal as IJornalable;
                        if (property_new_val.ParentObjects == null) property_new_val.ParentObjects = new ObservableCollection<IJornalable>();
                        if (!property_new_val.ParentObjects.Contains(this))
                            property_new_val.ParentObjects.Add(this); //Добавляем текущий объекта в качестве родительского для нового значения 

                        property_new_val.ObjectChangedNotify += OnChildObjectChanges; //Добавляем обработски изменений в новомо объекте 
                        property_new_val.ObjectChangeSaved += OnChildObjectChangeSaved; //Добавляем обработски изменений в новомо объекте 
                        property_new_val.ObjectChangeUndo += OnChildObjectChangeSaved; //Добавляем обработски изменений в новомо объекте 
                    }
                    if (member != null && member is IJornalable && newVal == null) // Если свойству присваивают другой объект и они оба не null
                    {
                        IJornalable property_member = member as IJornalable;

                        property_member.ParentObjects.Remove(this); //Удаляемся из объектса  свойств 
                        property_member.ObjectChangedNotify -= OnChildObjectChanges;  //Отписвыаемся от изменений в объете который был в качестве свойства 
                        property_member.ObjectChangeSaved -= OnChildObjectChangeSaved;  //Отписвыаемся от изменений  
                        property_member.ObjectChangeUndo -= OnChildObjectChangeSaved;  //Отписвыаемся от изменений 
                    }
                }
                if (CurrentContextId != Guid.Empty)
                {
                    List<Guid> AnatherWindowsIds = PropertiesChangeJornal.ContextIdHistory.Where(el => el != CurrentContextId).ToList(); //Ищем в журнале Id других окон в цепочке изменений
                    foreach (Guid prev_wnd_is in AnatherWindowsIds) //Если существуют другие контекты из которых производителись изменения свойство данного бъекта, то сохраняем текущее значение свойства в журнале
                    {
                        PropertyStateRecord propertyStateRecord_1 = new PropertyStateRecord(member, JornalRecordStatus.MODIFIED, propertyName, prev_wnd_is);
                        propertyStateRecord_1.ParentObject = (IEntityObject)this;
                        PropertiesChangeJornal.Add(propertyStateRecord_1);
                        ObjectChangedNotify?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyStateRecord_1));
                    }
                    PropertyStateRecord propertyStateRecord = new PropertyStateRecord(member, JornalRecordStatus.MODIFIED, propertyName, CurrentContextId);
                    propertyStateRecord.ParentObject = (IEntityObject)this;
                    PropertiesChangeJornal.Add(propertyStateRecord);
                    ObjectChangedNotify?.Invoke(this, new ObjectStateChangedEventArgs("", this, propertyStateRecord));
                }
            }
        }
        public void AdjustObjectsStructure(PropertiesChangeJornal changesJornal, IJornalable sourse = null)
        {
            if (sourse == null) sourse = this;
            sourse.PropertiesChangeJornal = changesJornal;
            var _props_propinfos = sourse.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            var navigate_props_infoes = Functions.GetNavigateProperties(this);
            var all_props_propinfos =
               _props_propinfos.Where(pri => !navigate_props_infoes.Contains(pri) && pri.GetValue(sourse) is IJornalable);

            foreach (PropertyInfo prop_info in all_props_propinfos)
            //all_props_propinfos.Where(pri=>(pri.GetValue(sourse) as IJornalable).AdjustedStatus==AdjustStatus.UNADJUSTED))
            {
                var prop_value = prop_info.GetValue(sourse);
                IJornalable jornl_prop_value = prop_value as IJornalable;

                if (jornl_prop_value != null && jornl_prop_value.AdjustedStatus != AdjustStatus.IN_PROCESS &&
                   jornl_prop_value.AdjustedStatus != AdjustStatus.ADJUSTED)
                {           //Примем что "детей настраивает родитель"
                    if (jornl_prop_value.ParentObjects == null)
                        jornl_prop_value.ParentObjects = new ObservableCollection<IJornalable>();

                    if (!jornl_prop_value.ParentObjects.Contains(sourse))
                        jornl_prop_value.ParentObjects.Add(sourse);

                    if (this.ChildObjects == null) this.ChildObjects = new ObservableCollection<IJornalable>();

                    if (!this.ChildObjects.Contains(jornl_prop_value))
                        this.ChildObjects.Add(jornl_prop_value);

                    jornl_prop_value.ObjectChangedNotify += OnChildObjectChanges;
                    jornl_prop_value.ObjectChangeSaved += OnChildObjectChangeSaved;
                    jornl_prop_value.ObjectChangeUndo += OnChildObjectChangeUndo;
                    jornl_prop_value.AdjustedStatus = AdjustStatus.IN_PROCESS;
                }
            }
            foreach (PropertyInfo prop_info in all_props_propinfos)
            {
                var prop_value = prop_info.GetValue(sourse);
                IJornalable jornl_prop_value = prop_value as IJornalable;
                if (jornl_prop_value != null && jornl_prop_value.AdjustedStatus == AdjustStatus.IN_PROCESS)
                {           //Примем что "детей настраивает родитель"
                    jornl_prop_value.AdjustedStatus = AdjustStatus.ADJUSTED;
                    if ((prop_value is IList))
                        jornl_prop_value.AdjustObjectsStructure(PropertiesChangeJornal);
                    else
                        AdjustObjectsStructure(PropertiesChangeJornal, jornl_prop_value);

                }
            }
        }
        public void ResetObjectsStructure(IJornalable sourse = null)
        {
            if (sourse == null)
            {
                sourse = this;
                sourse.ParentObjects?.Clear();
                sourse.ChildObjects?.Clear();
                sourse.ObjectChangedNotify -= OnChildObjectChanges;
                sourse.ObjectChangeSaved -= OnChildObjectChangeSaved;
                sourse.ObjectChangeUndo -= OnChildObjectChangeUndo;
            }
            var _props_propinfos = sourse.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0);
            var navigate_props_infoes = Functions.GetNavigateProperties(this);
            var all_props_propinfos =
               _props_propinfos.Where(pri => !navigate_props_infoes.Contains(pri) && pri.GetValue(sourse) is IJornalable);

            foreach (PropertyInfo prop_info in all_props_propinfos)
            {
                var prop_value = prop_info.GetValue(sourse);
                IJornalable jornl_prop_value = prop_value as IJornalable;

                if (jornl_prop_value != null && jornl_prop_value.AdjustedStatus != AdjustStatus.IN_PROCESS &&
                     jornl_prop_value.AdjustedStatus != AdjustStatus.NONE)
                {
                    jornl_prop_value.ParentObjects?.Clear();
                    jornl_prop_value.ChildObjects?.Clear();

                    jornl_prop_value.ObjectChangedNotify -= OnChildObjectChanges;
                    jornl_prop_value.ObjectChangeSaved -= OnChildObjectChangeSaved;
                    jornl_prop_value.ObjectChangeUndo -= OnChildObjectChangeUndo;
                    jornl_prop_value.AdjustedStatus = AdjustStatus.IN_PROCESS;
                }
            }
            foreach (PropertyInfo prop_info in all_props_propinfos)
            {
                var prop_value = prop_info.GetValue(sourse);
                IJornalable jornl_prop_value = prop_value as IJornalable;
                if (jornl_prop_value != null && jornl_prop_value.AdjustedStatus == AdjustStatus.IN_PROCESS)
                {           //Примем что "детей настраивает родитель"
                    jornl_prop_value.AdjustedStatus = AdjustStatus.NONE;
                    if ((prop_value is IList))
                        jornl_prop_value.ResetObjectsStructure();
                    else
                        ResetObjectsStructure(jornl_prop_value);

                }
            }
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

