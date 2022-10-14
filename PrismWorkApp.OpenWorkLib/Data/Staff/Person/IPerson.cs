using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public  interface IPerson:IRegisterable
    {
        string Fathername { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
