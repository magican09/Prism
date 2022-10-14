using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public class PropertyStateRecord : IKeyable, INameable,IDateable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ContextId { get; set;}
        public string Name { get; set; }
        public DateTime Date { get ; set ; }
        public object Value { get; set; }
        public JornalRecordStatus Status { get; set; }
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
    }
}
