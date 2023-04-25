namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldExecutiveScheme : bldDocument, IbldExecutiveScheme, IEntityObject
    {
      
        public bldExecutiveScheme()
        {
            Code = "ипл_схм";
        }
        public bldExecutiveScheme(string name) : base(name)
        {
            Code = "ипл_схм";
        }

    }
}
