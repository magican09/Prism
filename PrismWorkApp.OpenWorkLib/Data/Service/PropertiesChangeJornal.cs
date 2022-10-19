using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate void  PropertiesChangeJornalChangedEventHandler(object sender, PropertyStateRecord _propertyStateRecord);

    public class PropertiesChangeJornal : ObservableCollection<PropertyStateRecord>
    {
        public event PropertiesChangeJornalChangedEventHandler JornalChangedNotify;
        public PropertiesChangeJornal()
        {

            CollectionChanged += PropertiesChangeJornal_CollectionChanged;
        }

        private void PropertiesChangeJornal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
             if(e.Action== NotifyCollectionChangedAction.Add)
            {
                foreach (PropertyStateRecord stateRecord in e.NewItems)
                    if (JornalChangedNotify != null)
                        JornalChangedNotify(this, stateRecord);
                    else
                        ;
                
            }
           
           
        }
       public object ParentObject { get; set; }
        private  bool IsContainsRecord(Guid currentContextId)
        {
            return this.Count > 0;
        }
    }
}
