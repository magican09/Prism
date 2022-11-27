using System.Windows;
using System.Windows.Controls;

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public class TreeViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            /*  User user = item as User;
              if (user.IsPremiumUser)
              {
                  return elemnt.FindResource("PremiumUserDataTemplate") as DataTemplate;
              }
              else
              {
                  return elemnt.FindResource("NormalUserDataTemplate") as DataTemplate;
              }
              */
            return elemnt.FindResource("bldObjectTemplate") as DataTemplate;
        }
    }
}
