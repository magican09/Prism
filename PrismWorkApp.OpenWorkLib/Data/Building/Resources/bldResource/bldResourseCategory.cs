namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResourseCategory : BindableBase
    {
        public NameableObservableCollection<bldResource> Resources { get; set; } = new NameableObservableCollection<bldResource>();
    }
}
