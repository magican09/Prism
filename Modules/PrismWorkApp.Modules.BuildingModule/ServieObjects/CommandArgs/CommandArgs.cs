using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class CommandArgs
    {
        public string  CommandName { get; set; }
        public IEntityObject Entity { get; set; }
        public IEntityObject Parent { get; set; }
        public Type Type { get; set; }
    }
}
