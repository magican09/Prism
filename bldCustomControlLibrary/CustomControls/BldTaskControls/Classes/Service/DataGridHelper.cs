using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace bldCustomControlLibrary
{
    internal static class DataGridHelper
    {
        #region Property Helpers

        public static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
        {
            return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
        }

        public static object GetCoercedTransferPropertyValue(
            DependencyObject baseObject,
            object baseValue,
            DependencyProperty baseProperty,
            DependencyObject parentObject,
            DependencyProperty parentProperty)
        {
            return GetCoercedTransferPropertyValue(
                baseObject,
                baseValue,
                baseProperty,
                parentObject,
                parentProperty,
                null,
                null);
        }

        /// <summary>
        ///     Computes the value of a given property based on the DataGrid property transfer rules.
        /// </summary>
        /// <remarks>
        ///     This is intended to be called from within the coercion of the baseProperty.
        /// </remarks>
        /// <param name="baseObject">The target object which recieves the transferred property</param>
        /// <param name="baseValue">The baseValue that was passed into the coercion delegate</param>
        /// <param name="baseProperty">The property that is being coerced</param>
        /// <param name="parentObject">The object that contains the parentProperty</param>
        /// <param name="parentProperty">A property who's value should be transfered (via coercion) to the baseObject if it has a higher precedence.</param>
        /// <param name="grandParentObject">Same as parentObject but evaluated at a lower presedece for a given BaseValueSource</param>
        /// <param name="grandParentProperty">Same as parentProperty but evaluated at a lower presedece for a given BaseValueSource</param>
        /// <returns></returns>
        public static object GetCoercedTransferPropertyValue(
            DependencyObject baseObject,
            object baseValue,
            DependencyProperty baseProperty,
            DependencyObject parentObject,
            DependencyProperty parentProperty,
            DependencyObject grandParentObject,
            DependencyProperty grandParentProperty)
        {
            // Transfer Property Coercion rules:
            //
            // Determine if this is a 'Transfer Property Coercion'.  If so:
            //   We can safely get the BaseValueSource because the property change originated from another
            //   property, and thus this BaseValueSource wont be stale.
            //   Pick a value to use based on who has the greatest BaseValueSource
            // If not a 'Transfer Property Coercion', simply return baseValue.  This will cause a property change if the value changes, which
            // will trigger a 'Transfer Property Coercion', and we will no longer have a stale BaseValueSource
            var coercedValue = baseValue;

            if (IsPropertyTransferEnabled(baseObject, baseProperty))
            {
                var propertySource = DependencyPropertyHelper.GetValueSource(baseObject, baseProperty);
                var maxBaseValueSource = propertySource.BaseValueSource;

                if (parentObject != null)
                {
                    var parentPropertySource = DependencyPropertyHelper.GetValueSource(parentObject, parentProperty);

                    if (parentPropertySource.BaseValueSource > maxBaseValueSource)
                    {
                        coercedValue = parentObject.GetValue(parentProperty);
                        maxBaseValueSource = parentPropertySource.BaseValueSource;
                    }
                }

                if (grandParentObject != null)
                {
                    var grandParentPropertySource = DependencyPropertyHelper.GetValueSource(grandParentObject, grandParentProperty);

                    if (grandParentPropertySource.BaseValueSource > maxBaseValueSource)
                    {
                        coercedValue = grandParentObject.GetValue(grandParentProperty);
                        maxBaseValueSource = grandParentPropertySource.BaseValueSource;
                    }
                }
            }

            return coercedValue;
        }

        /// <summary>
        ///     Causes the given DependencyProperty to be coerced in transfer mode.
        /// </summary>
        /// <remarks>
        ///     This should be called from within the target object's NotifyPropertyChanged.  It MUST be called in
        ///     response to a change in the target property.
        /// </remarks>
        /// <param name="d">The DependencyObject which contains the property that needs to be transfered.</param>
        /// <param name="p">The DependencyProperty that is the target of the property transfer.</param>
        public static void TransferProperty(DependencyObject d, DependencyProperty p)
        {
            var transferEnabledMap = GetPropertyTransferEnabledMapForObject(d);
            transferEnabledMap[p] = true;
            d.CoerceValue(p);
            transferEnabledMap[p] = false;
        }

        private static Dictionary<DependencyProperty, bool> GetPropertyTransferEnabledMapForObject(DependencyObject d)
        {
            Dictionary<DependencyProperty, bool> propertyTransferEnabledForObject;

            if (!_propertyTransferEnabledMap.TryGetValue(d, out propertyTransferEnabledForObject))
            {
                propertyTransferEnabledForObject = new Dictionary<DependencyProperty, bool>();
                _propertyTransferEnabledMap.Add(d, propertyTransferEnabledForObject);
            }

            return propertyTransferEnabledForObject;
        }

        internal static bool IsPropertyTransferEnabled(DependencyObject d, DependencyProperty p)
        {
            Dictionary<DependencyProperty, bool> propertyTransferEnabledForObject;

            if (_propertyTransferEnabledMap.TryGetValue(d, out propertyTransferEnabledForObject))
            {
                bool isPropertyTransferEnabled;
                if (propertyTransferEnabledForObject.TryGetValue(p, out isPropertyTransferEnabled))
                {
                    return isPropertyTransferEnabled;
                }
            }

            return false;
        }

        /// <summary>
        ///     Tracks which properties are currently being transfered.  This information is needed when GetPropertyTransferEnabledMapForObject
        ///     is called inside of Coercion.
        /// </summary>
        private static ConditionalWeakTable<DependencyObject, Dictionary<DependencyProperty, bool>> _propertyTransferEnabledMap = new ConditionalWeakTable<DependencyObject, Dictionary<DependencyProperty, bool>>();

        #endregion
        #region Notification Propagation

        public static bool ShouldNotifyCells(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Cells);
        }

        public static bool ShouldNotifyCellsPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.CellsPresenter);
        }

        public static bool ShouldNotifyColumns(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Columns);
        }

        public static bool ShouldNotifyColumnHeaders(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnHeaders);
        }

        public static bool ShouldNotifyColumnHeadersPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnHeadersPresenter);
        }

        public static bool ShouldNotifyColumnCollection(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.ColumnCollection);
        }

        public static bool ShouldNotifyDataGrid(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.DataGrid);
        }

        public static bool ShouldNotifyDetailsPresenter(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.DetailsPresenter);
        }

        public static bool ShouldRefreshCellContent(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.RefreshCellContent);
        }

        public static bool ShouldNotifyRowHeaders(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.RowHeaders);
        }

        public static bool ShouldNotifyRows(DataGridNotificationTarget target)
        {
            return TestTarget(target, DataGridNotificationTarget.Rows);
        }

        public static bool ShouldNotifyRowSubtree(DataGridNotificationTarget target)
        {
            DataGridNotificationTarget value =
                DataGridNotificationTarget.Rows |
                DataGridNotificationTarget.RowHeaders |
                DataGridNotificationTarget.CellsPresenter |
                DataGridNotificationTarget.Cells |
                DataGridNotificationTarget.RefreshCellContent |
                DataGridNotificationTarget.DetailsPresenter;

            return TestTarget(target, value);
        }

        private static bool TestTarget(DataGridNotificationTarget target, DataGridNotificationTarget value)
        {
            return (target & value) != 0;
        }

        #endregion
        #region Binding

        /// <summary>
        ///     Returns true if the binding (or any part of it) is OneWay.
        /// </summary>
        internal static bool IsOneWay(BindingBase bindingBase)
        {
            if (bindingBase == null)
            {
                return false;
            }

            // If it is a standard Binding, then check if it's Mode is OneWay
            Binding binding = bindingBase as Binding;
            if (binding != null)
            {
                return binding.Mode == BindingMode.OneWay;
            }

            // A multi-binding can be OneWay as well
            MultiBinding multiBinding = bindingBase as MultiBinding;
            if (multiBinding != null)
            {
                return multiBinding.Mode == BindingMode.OneWay;
            }

            // A priority binding is a list of bindings, if any are OneWay, we'll call it OneWay
            PriorityBinding priBinding = bindingBase as PriorityBinding;
            if (priBinding != null)
            {
                Collection<BindingBase> subBindings = priBinding.Bindings;
                int count = subBindings.Count;
                for (int i = 0; i < count; i++)
                {
                    if (IsOneWay(subBindings[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal static BindingExpression GetBindingExpression(FrameworkElement element, DependencyProperty dp)
        {
            if (element != null)
            {
                return element.GetBindingExpression(dp);
            }

            return null;
        }

        //internal static bool ValidateWithoutUpdate(FrameworkElement element)
        //{
        //    bool result = true;
        //    BindingGroup bindingGroup = element.BindingGroup;
        //    BldTaskDataGridCell cell = (element != null) ? element.Parent as BldTaskDataGridCell : null;

        //    if (bindingGroup != null && cell != null)
        //    {
        //        Collection<BindingExpressionBase> expressions = bindingGroup.BindingExpressions;
        //        BindingExpressionBase[] bindingExpressionsCopy = new BindingExpressionBase[expressions.Count];
        //        expressions.CopyTo(bindingExpressionsCopy, 0);

        //        for (int i = 0; i < bindingExpressionsCopy.Length; i++)
        //        {
        //            // Check the binding's target element - it might have been GC'd
        //            // (this can happen when column-virtualization is enabled, see Dev11 131232).
        //            // If so, fetching TargetElement will detach the binding and remove it
        //            // from the binding group's collection.  This side-effect is why we
        //            // loop through a copy of the original collection, and don't rely
        //            // on i to be a valid index into the original collection.
        //            BindingExpressionBase beb = bindingExpressionsCopy[i];
        //            if (BindingExpressionBelongsToElement<BldTaskDataGridCell>(beb, cell))
        //            {
        //                result = beb.ValidateWithoutUpdate() && result;
        //            }
        //        }
        //    }

        //    return result;
        //}

        //internal static bool BindingExpressionBelongsToElement<T>(BindingExpressionBase beb, T element) where T : FrameworkElement
        //{
        //    DependencyObject targetElement = beb.TargetElement;
        //    if (targetElement != null)
        //    {
        //        DependencyObject contextElement = FindContextElement(beb);
        //        if (contextElement == null)
        //        {
        //            contextElement = targetElement;
        //        }

        //        if (contextElement is Visual || contextElement is System.Windows.Media.Media3D.Visual3D)
        //        {
        //            return VisualTreeHelper.IsAncestorOf(element, contextElement, typeof(T));
        //        }
        //    }

        //    return false;
        //}

        //private static DependencyObject FindContextElement(BindingExpressionBase beb)
        //{
        //    BindingExpression be;
        //    MultiBindingExpression mbe;
        //    PriorityBindingExpression pbe;

        //    if ((be = beb as BindingExpression) != null)
        //    {
        //        // leaf binding - return its context element
        //        return be.ContextElement;
        //    }

        //    // otherwise, depth-first search through tree of bindings
        //    ReadOnlyCollection<BindingExpressionBase> childBindings = null;
        //    if ((mbe = beb as MultiBindingExpression) != null)
        //    {
        //        childBindings = mbe.BindingExpressions;
        //    }
        //    else if ((pbe = beb as PriorityBindingExpression) != null)
        //    {
        //        childBindings = pbe.BindingExpressions;
        //    }

        //    if (childBindings != null)
        //    {
        //        foreach (BindingExpressionBase childBEB in childBindings)
        //        {
        //            DependencyObject result = FindContextElement(childBEB);
        //            if (result != null)
        //            {
        //                return result;
        //            }
        //        }
        //    }

        //    return null;
        //}

        private static readonly DependencyProperty FlowDirectionCacheProperty = DependencyProperty.Register("FlowDirectionCache", typeof(FlowDirection), typeof(DataGridHelper));

        internal static void CacheFlowDirection(FrameworkElement element, BldTaskDataGridCell cell)
        {
            if (element != null && cell != null)
            {
                object flowDirectionObj = element.ReadLocalValue(FrameworkElement.FlowDirectionProperty);
                if (flowDirectionObj != DependencyProperty.UnsetValue)
                {
                    cell.SetValue(FlowDirectionCacheProperty, flowDirectionObj);
                }
            }
        }

        internal static void RestoreFlowDirection(FrameworkElement element, BldTaskDataGridCell cell)
        {
            if (element != null && cell != null)
            {
                object flowDirectionObj = cell.ReadLocalValue(DataGridHelper.FlowDirectionCacheProperty);
                if (flowDirectionObj != DependencyProperty.UnsetValue)
                {
                    element.SetValue(FrameworkElement.FlowDirectionProperty, flowDirectionObj);
                }
            }
        }

        //internal static void UpdateTarget(FrameworkElement element)
        //{
        //    BindingGroup bindingGroup = element.BindingGroup;
        //    BldTaskDataGridCell cell = (element != null) ? element.Parent as BldTaskDataGridCell : null;

        //    if (bindingGroup != null && cell != null)
        //    {
        //        Collection<BindingExpressionBase> expressions = bindingGroup.BindingExpressions;
        //        BindingExpressionBase[] bindingExpressionsCopy = new BindingExpressionBase[expressions.Count];
        //        expressions.CopyTo(bindingExpressionsCopy, 0);

        //        for (int i = 0; i < bindingExpressionsCopy.Length; i++)
        //        {
        //            // Check the binding's target element - it might have been GC'd
        //            // (this can happen when column-virtualization is enabled, see Dev11 131232).
        //            // If so, fetching TargetElement will detach the binding and remove it
        //            // from the binding group's collection.  This side-effect is why we
        //            // loop through a copy of the original collection, and don't rely
        //            // on i to be a valid index into the original collection.
        //            BindingExpressionBase beb = bindingExpressionsCopy[i];
        //            DependencyObject targetElement = beb.TargetElement;
        //            if (targetElement != null &&
        //                VisualTreeHelper.IsAncestorOf(cell, targetElement, typeof(DataGridCell)))
        //            {
        //                beb.UpdateTarget();
        //            }
        //        }
        //    }
        //}

        internal static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
        {
            if (IsDefaultValue(column, columnProperty))
            {
                content.ClearValue(contentProperty);
            }
            else
            {
                content.SetValue(contentProperty, column.GetValue(columnProperty));
            }
        }

        internal static string GetPathFromBinding(Binding binding)
        {
            if (binding != null)
            {
                if (!string.IsNullOrEmpty(binding.XPath))
                {
                    return binding.XPath;
                }
                else if (binding.Path != null)
                {
                    return binding.Path.Path;
                }
            }

            return null;
        }

        #endregion
        #region Tree Helpers

        /// <summary>
        ///     Walks up the templated parent tree looking for a parent type.
        /// </summary>
        public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }

            return null;
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        /// <summary>
        ///     Helper method which determines if any of the elements of
        ///     the tree is focusable and has tab stop
        /// </summary>
        public static bool TreeHasFocusAndTabStop(DependencyObject element)
        {
            if (element == null)
            {
                return false;
            }

            UIElement uielement = element as UIElement;
            if (uielement != null)
            {
                if (uielement.Focusable && KeyboardNavigation.GetIsTabStop(uielement))
                {
                    return true;
                }
            }
            else
            {
                ContentElement contentElement = element as ContentElement;
                if (contentElement != null && contentElement.Focusable && KeyboardNavigation.GetIsTabStop(contentElement))
                {
                    return true;
                }
            }

            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i) as DependencyObject;
                if (TreeHasFocusAndTabStop(child))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
