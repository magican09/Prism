using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule.Views
{
    public class RadGridViewTemplateSelector:DataTemplateSelector
    {
        public DataTemplate  MaterialCertificatesView { get; set; }
        public DataTemplate LaboratoryReportsView { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if(element!=null )
            {
                return MaterialCertificatesView as DataTemplate;

            }
            return base.SelectTemplate(item, container);
        }
        
    }
}
