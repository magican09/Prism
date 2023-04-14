using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data.Service 
{
    public delegate void UnDoReDoSystemEventHandler(IUnDoReDoSystem sender, UnDoReDoSystemEventArgs e);
    public delegate void UnDoReDoCommandEventHandler(IJornalable sender, UnDoReDoCommandEventArgs e);

    public class UnDoReDoSystemEventArgs:EventArgs
    {
        // public IUnDoReDoSystem UnDoReDoSystem {get;set;}
        public IEnumerable<IJornalable>  ObjectsList { get; set; }
        public UnDoReDoSystemEventArgs(IList<IJornalable> objects)
        {
         
            ObjectsList = objects;
        }
    }
    public class UnDoReDoCommandEventArgs : EventArgs
    {
        // public IUnDoReDoSystem UnDoReDoSystem {get;set;}
        public  IUnDoRedoCommand  Command { get; set; }
        public UnDoReDoCommandEventArgs(IUnDoRedoCommand command)
        {

            Command = command;
        }
    }
}
