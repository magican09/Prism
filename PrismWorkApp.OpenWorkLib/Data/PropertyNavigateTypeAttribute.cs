using System;
using System.ComponentModel.DataAnnotations;

namespace PrismWorkApp.OpenWorkLib.Data
{
    //public class NavigatePropertyAttribute : ValidationAttribute
    //{

    //    public override bool IsValid(object value)
    //    {
    //        /*  if (value == null)
    //           {
    //               ErrorMessage = "navigate_property_is_null";
    //               return false;
    //           }
    //           else
    //           {
    //               ErrorMessage = "navigate_property_is_not_null";
    //               return true;
    //           }

    //       */

    //        return false;
    //    }
    //}
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigatePropertyAttribute : Attribute
    {
        public NavigatePropertyAttribute()
        {

        }


    }
    [AttributeUsage(AttributeTargets.Property)]
    public class CreateNewWhenCopyAttribute : Attribute
    {
        public CreateNewWhenCopyAttribute()
        {

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
