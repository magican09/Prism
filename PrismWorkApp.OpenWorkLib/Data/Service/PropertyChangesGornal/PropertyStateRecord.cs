using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class PropertyStateRecord 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid _сontextId;

        public Guid ContextId
        {
            get
            {
                if (ContextIdStructure != null)
                    return ContextIdStructure.ContextId; 
                else
                    return _сontextId;
            }
            set
            {
                    

                if (ContextIdStructure != null)
                {
                    ContextIdStructure.ContextId = value;
                }
                else
                    _сontextId = value;
            }
        }
        public ContextIdStructure ContextIdStructure{ get; set; }
        public string Name { get; set; }
        public DateTime Date { get ; set ; }
        public object Value { get; set; }
        public JornalRecordStatus Status { get; set; }
        public IJornalable ParentObject { get; set; }
        public PropertiesChangeJornal ParentJornal { get; set; }
        public PropertyStateRecord(object prop, JornalRecordStatus recordStatus, string name ="" )
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
        }
        public PropertyStateRecord(object prop, JornalRecordStatus recordStatus, string name, Guid currentContextId)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
        }
        public PropertyStateRecord(object prop, JornalRecordStatus recordStatus, string name, Guid currentContextId, IEntityObject parentObject)
        {
            Value = prop;
            Date = DateTime.Now;
            Status = recordStatus;
            Name = name;
            ContextId = currentContextId;
            ParentObject = parentObject;
        }
    }
}
