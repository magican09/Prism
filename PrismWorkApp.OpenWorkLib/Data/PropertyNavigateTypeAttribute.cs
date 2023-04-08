using System;
using System.ComponentModel.DataAnnotations;

namespace PrismWorkApp.OpenWorkLib.Data
{
    
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigatePropertyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            return true;
        }


    }
    [AttributeUsage(AttributeTargets.Property)]
    public class CreateNewWhenCopyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            return true;
        }


    }
    [AttributeUsage(AttributeTargets.Property)]
    public class NotJornalingAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            return true;
        }
    }
}
