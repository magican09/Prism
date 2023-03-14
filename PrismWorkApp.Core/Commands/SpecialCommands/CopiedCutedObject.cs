using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core.Commands
{
    public class CopiedCutedObject
    {
        public object FromObject { get; set; }
        public object Element  {get;set;}
        public CopyCutPaste ActionType { get; set; }
        public CopiedCutedObject(object from_object,object element, CopyCutPaste action_type)
        {
            FromObject = from_object;
            Element = element;
            ActionType = action_type;
        }
        public CopiedCutedObject()
        {

        }
    }
    public enum CopyCutPaste
    {
        COPIED,
        CUTED,
        PASTED
    }
}
