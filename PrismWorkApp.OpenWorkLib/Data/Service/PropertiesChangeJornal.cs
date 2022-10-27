using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate void  PropertiesChangeJornalChangedEventHandler(object sender, PropertyStateRecord _propertyStateRecord);
    public delegate void ObjectStateChangeEventHandler(object sender, ObjectStateChangedEventArgs e);

    public class PropertiesChangeJornal : ObservableCollection<PropertyStateRecord>
    {
        public event PropertiesChangeJornalChangedEventHandler JornalChangedNotify;
        public ObservableCollection<Guid> ContextIdHistory { get; set; } = new ObservableCollection<Guid>();

        public PropertiesChangeJornal()
        {

            CollectionChanged += PropertiesChangeJornal_CollectionChanged;
        }

        private void PropertiesChangeJornal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
             if(e.Action== NotifyCollectionChangedAction.Add)
            {
                foreach (PropertyStateRecord stateRecord in e.NewItems)
                {
                    stateRecord.ParentJornal = this;
                    if (JornalChangedNotify != null)
                    {
                        /*  if (this.Where(pr=>pr.ContextId== stateRecord.ContextId).Count()==1)//Если изменеия в текущем контексе первые..
                         {
                             PropertyStateRecord st_record = new PropertyStateRecord(stateRecord.Value,stateRecord.Status, stateRecord.Name, ParentObject.CurrentContextId);

                             ParentObject.PropertiesChangeJornal.Add(st_record);
                         }    */
                     //   if (!ContextIdHistory.Contains(stateRecord.ContextId))
                       //     ContextIdHistory.Add(stateRecord.ContextId);


                        JornalChangedNotify(this, stateRecord);

                    }
                    else
                        ;
                }
            }
           
           
        }
        public IJornalable ParentObject { get; set; }
        private  bool IsContainsRecord(Guid currentContextId)
        {
            return this.Count > 0;
        }
    }
    public class ObjectStateChangedEventArgs
    {
       public  string ObjectName { get; set; }
       public IJornalable Object { get; set; }
       public PropertyStateRecord PropertyStateRecord { get; set; }
        public ObjectStateChangedEventArgs()
        {

        }
        public ObjectStateChangedEventArgs(string name, IJornalable obj, PropertyStateRecord propertyStateRecord)
        {
            ObjectName = name;
            Object = obj;
            PropertyStateRecord = propertyStateRecord;
        }
    }


}
