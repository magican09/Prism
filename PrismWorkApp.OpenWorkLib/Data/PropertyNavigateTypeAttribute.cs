using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class NavigatePropertyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            /*  if (value == null)
               {
                   ErrorMessage = "navigate_property_is_null";
                   return false;
               }
               else
               {
                   ErrorMessage = "navigate_property_is_not_null";
                   return true;
               }

           */
           
            return false;
        }
    }
    public class NotJornalingAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            return false;
        }
    }
}
