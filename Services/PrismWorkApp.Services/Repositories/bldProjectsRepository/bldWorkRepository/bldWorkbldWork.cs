using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
    public class bldWorkbldWork
    {


        public Guid bldWorkId { get; set; }
        public bldWork bldWork { get; set; }
        public Guid PreviousWorkId { get; set;}
        public bldWork PreviousWork { get; set; } 
        public Guid NextWorkId { get; set; }
        public bldWork NextWork { get; set; }
    }
}
