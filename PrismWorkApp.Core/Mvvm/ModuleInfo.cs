using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PrismWorkApp.Core.Mvvm
{
    public class ModuleInfo: DependencyObject
    {


        public static bool GetIsEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnableProperty);
        }

        public static void SetIsEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnableProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached("IsEnable", typeof(bool), typeof(ModuleInfo), new PropertyMetadata(false));


    }
}
