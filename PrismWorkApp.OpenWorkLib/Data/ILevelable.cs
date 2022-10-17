using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface ILevelable
    {
     //   public StructureLevel StructureLevel { get; set; }
        public string Code { get; set; }
        public void UpdateStructure();
        public void ClearStructureLevel();
    }
}
