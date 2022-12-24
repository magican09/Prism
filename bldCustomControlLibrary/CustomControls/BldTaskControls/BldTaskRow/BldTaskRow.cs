using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace bldCustomControlLibrary
{
    public class BldTaskRow:DataGridRow 
    {


        public object Item
        {
            get { return (object)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(object), typeof(BldTaskRow), new PropertyMetadata(null,
                new PropertyChangedCallback(OnItemPropertyChangedCallback),
                new CoerceValueCallback(OnItemCoerceValueCallback)));

        public BldTaskRow()
        {
            Binding bind = new Binding("Item");
            bind.Source = this;
            bind.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(this, DataGridRow.ItemProperty, bind);
          
        }

        static  BldTaskCellsPresenter BldTaskCellsPresenter = null;
        public override void OnApplyTemplate()
        {
            BldTaskCellsPresenter = Template.FindName("PART_BldTaskCellsPresenter",this) as BldTaskCellsPresenter;
          
          
            BldTaskCellsPresenter.Item = Item;

            BldTaskCell bldTaskCell = new BldTaskCell();
            bldTaskCell.DataContext = Item;
            BldTaskCellsPresenter.DataContext = Item; 
            BldTaskCellsPresenter.Items.Add(Item);
           
            base.OnApplyTemplate();
        }
        private static object OnItemCoerceValueCallback(DependencyObject d, object baseValue)
        {
        
            return baseValue;
        }

        private static void OnItemPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }
        protected override void OnItemChanged(object oldItem, object newItem)
        {
            base.OnItemChanged(oldItem, newItem);
          
        }
    }
}
