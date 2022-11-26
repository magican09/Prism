using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public delegate void  ObservedCommandExecuteEvenHandler(object sender, ObservedCommandExecuteEventsArgs e);

    public class ObservedCommandExecuteEventsArgs
    {
        public string CommandName { get; set; }
        public IUnDoRedoCommand Command { get; set; }
        public ObservedCommandExecuteEventsArgs(IUnDoRedoCommand command,string command_name ="")
        {
            Command = command;
            CommandName = command_name;
        }
    }
}
