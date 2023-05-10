using System;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEmployee : IPerson, ICloneable
    {
        EmployeePosition Position { get; set; }
        decimal Salary { get; set; }
          bldCompany Company { get; set; }
    }
}