using System.Windows;
using System.Windows.Controls;

namespace bldCustomControlLibrary
{
    public class GanttDiargamDataGrid : DataGrid   //DataGridCellsPresenter// DataGridColumnHeader GridViewColumnHeader//DataGridColumnHeader  //DataGridColumnHeadersPresenter, DataGridRowHeader
    {
        //public object SelectedItem
        //{
        //    get { return (object)GetValue(SelectedItemProperty); }
        //    set { SetValue(SelectedItemProperty, value); }
        //}

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SelectedItemProperty =
        //    DependencyProperty.Register("SelectedItem", typeof(object), typeof(ConstructionDataGrid),
        //        new PropertyMetadata(new ValidateValueCallback(OnValidateValueCallback),
        //                new PropertyChangedCallback(OnPropertyChangedCallback), new CoerceValueCallback(OnCoerceValueCallback)));

        //public object ItemsSource
        //{
        //    get { return (object)GetValue(ItemsSourceProperty); }
        //    set { SetValue(ItemsSourceProperty, value); }
        //}

        // Using a DependencyProperty as the backing store for ItemsSourse.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ItemsSourceProperty =
        //    DependencyProperty.Register("ItemsSource", typeof(object), typeof(GanttDiargamDataGrid), new PropertyMetadata(new CoerceValueCallback(OnCoerceValueCallback)));

        public object ConsructionProject
        {
            get { return (object)GetValue(ConsructionProjectProperty); }
            set { SetValue(ConsructionProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConsructionProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConsructionProjectProperty =
            DependencyProperty.Register("ConsructionProject", typeof(object), typeof(GanttDiargamDataGrid), new PropertyMetadata(0));

        public GanttDiargamDataGrid()
        {
            //SelectedItemProperty.OverrideMetadata(typeof(GanttDiargamDataGrid), new FrameworkPropertyMetadata(
            //   null,
            //   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            //   SelectedItemPropertyChangedCallback, new CoerceValueCallback(OnSelectedItemCoerceValueCallback)));

            //ItemsSourceProperty.OverrideMetadata(typeof(GanttDiargamDataGrid),
            //    new FrameworkPropertyMetadata(
            //        null,
            //        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            //    SourcePropertyChangedCallback, SourcePropertyCoerceValueCallback));
        }
        #region CalckbackMethods
        private object OnSelectedItemCoerceValueCallback(DependencyObject d, object baseValue)
        {
            return baseValue;
        }
        object dsd;
        public override void OnApplyTemplate()
        {
            //ScrollViewer diagrammViewer = Template.FindName("DG_ScrollViewer", this) as ScrollViewer;

            //var dgsd = Template.FindName("PART_ColumnHeadersPresenter", this);

            //var presenter = WPFExtendedFunctions.GetVisualChild<TaskDataGridColumnHeaderPresenter>(diagrammViewer);
            //var sd = LogicalTreeHelper.GetChildren(diagrammViewer);

            base.OnApplyTemplate();
            dsd = Template.FindName("ganttItemsPresenter", this);

        }

        private object SourcePropertyCoerceValueCallback(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private void SourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void SelectedItemPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
