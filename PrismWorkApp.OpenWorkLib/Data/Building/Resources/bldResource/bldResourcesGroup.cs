using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public  class bldResourcesGroup : NameableObservableCollection<bldResource>, IbldResourcesGroup, INotifyPropertyChanged, IEntityObject
    {

        public bldResourcesGroup()
        {
            Name = "Ресурсы:";
        }
        public bldResourcesGroup(string name)
        {
            Name = name;
        }
        private decimal _cost;
        public decimal Cost
        {
            get
            {
                CalcTotalCost();
                return _cost;
            }
            set { SetProperty(ref _cost, value); }
        }
        private void CalcTotalCost()
        {
            decimal total_cost = 0;
            foreach (bldResource  resource in this.Items)
            {
                total_cost += resource.Cost;
            }
            Cost = total_cost;

        }
    }
}
