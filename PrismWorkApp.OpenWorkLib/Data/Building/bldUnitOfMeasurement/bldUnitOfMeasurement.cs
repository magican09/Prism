namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldUnitOfMeasurement : BindableBase, IbldUnitOfMeasurement, IEntityObject
    {
        private string _name = "ед.изм.";
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _shortName;
        public string ShortName
        {
            get { return _shortName; }
            set { SetProperty(ref _shortName, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public bldUnitOfMeasurement(string name)
        {
            Name = name;
        }
        public bldUnitOfMeasurement()
        {

        }
    }
}
