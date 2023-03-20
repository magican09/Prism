using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResourseCategoriesGroup : NameableObservableCollection<bldResourseCategory>, INotifyPropertyChanged, IEntityObject
    {

        public bldResourseCategoriesGroup()
        {
            Name = "Категории ресурсов:";
        }
        public bldResourseCategoriesGroup(string name)
        {
            Name = name;
        }
    }
}
