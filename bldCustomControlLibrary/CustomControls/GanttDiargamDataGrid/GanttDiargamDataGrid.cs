using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace bldCustomControlLibrary
{
    public class GanttDiargamDataGrid:DataGrid
    {


        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSourse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSourse", typeof(object), typeof(GanttDiargamDataGrid), new PropertyMetadata(new CoerceValueCallback(OnCoerceValueCallback)));



        public object ConsructionProject
        {
            get { return (object)GetValue(ConsructionProjectProperty); }
            set { SetValue(ConsructionProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConsructionProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConsructionProjectProperty =
            DependencyProperty.Register("ConsructionProject", typeof(object), typeof(GanttDiargamDataGrid), new PropertyMetadata(0));

        public override void OnApplyTemplate()
        {
            
            base.OnApplyTemplate();
        }

    }
}
