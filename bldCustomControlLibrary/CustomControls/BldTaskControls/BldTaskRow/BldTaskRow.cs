using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace bldCustomControlLibrary
{
    public class BldTaskRow:DataGridRow 
    {


        //public object Item
        //{
        //    get { return (object)GetValue(ItemProperty); }
        //    set { SetValue(ItemProperty, value); }
        //}

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ItemProperty =
        //    DependencyProperty.Register("Item", typeof(object), typeof(BldTaskRow), new PropertyMetadata(null,
        //        new PropertyChangedCallback(OnItemPropertyChangedCallback),
        //        new CoerceValueCallback(OnItemCoerceValueCallback)));

        public BldTaskRow()
        {
            Binding bind = new Binding("Item");
            bind.Source = this;
            bind.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(this, DataGridRow.ItemProperty, bind);
           
        }

        static  BldTaskCellsPresenter _bldTaskCellsPresenter = null;
        public ObservableCollection<BldTaskCell> cells { get; set; } = new ObservableCollection<BldTaskCell>();
        public ObservableCollection<object> collection { get; set; } = new ObservableCollection<object>();
        public BldTaskCell bldTaskCell { get; set; }
        public override void OnApplyTemplate()
        {

          
            _bldTaskCellsPresenter = Template.FindName("PART_BldTaskCellsPresenter", this) as BldTaskCellsPresenter;
            DataGridColumn column = new BldTaskCellColumn();
            column.Width =50;
            column.Header = "sfsfsf";

            bldTaskCell = new BldTaskCell();
            bldTaskCell.DataContext = Item;
     
            bldTaskCell.SetBinding(BldTaskCell.ContentProperty, new Binding("Item") { Source = this });

            cells.Add(bldTaskCell);
            cells.Add(bldTaskCell);
            cells.Add(bldTaskCell);

            collection.Add(Item);
            collection.Add(Item);
            Binding bind = new Binding("cells") { Source = this };
            _bldTaskCellsPresenter.SetBinding(BldTaskCellsPresenter.ItemsSourceProperty, bind);
            //_bldTaskCellsPresenter.ItemsSource = collection;
           
            base.OnApplyTemplate();
            _bldTaskCellsPresenter.UpdateLayout();
        }
        private static object OnItemCoerceValueCallback(DependencyObject d, object baseValue)
        {
        
            return baseValue;
        }

        private static void OnItemPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }
        //protected override void OnItemChanged(object oldItem, object newItem)
        //{
        //    base.OnItemChanged(oldItem, newItem);
          
        //}
    }
}
