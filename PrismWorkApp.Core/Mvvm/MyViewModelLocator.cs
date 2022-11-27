using System;
using System.Windows;

namespace PrismWorkApp.Core.Mvvm
{
    public class MyViewModelLocator : DependencyObject
    {
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }
        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            //       obj.SetValue(AutoWireViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoWireViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(MyViewModelLocator), new PropertyMetadata(true, AutoWireViewModelChage, AutoWireViewCoerce));

        private static object AutoWireViewCoerce(DependencyObject d, object baseValue)
        {
            if ((bool)baseValue == true)
            {
                FrameworkElement element = (FrameworkElement)d;
                string asf = element.GetType().ToString();
            }
            throw new NotImplementedException();
        }

        private static void AutoWireViewModelChage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
