using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResourseCategoriesGroup : NameableObservableCollection<bldResourseCategory>
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
