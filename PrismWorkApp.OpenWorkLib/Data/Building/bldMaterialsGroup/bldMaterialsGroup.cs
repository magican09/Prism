using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldMaterialsGroup : NameableObservableCollection<bldMaterial>, IbldMaterialsGroup, INotifyPropertyChanged, IEntityObject
    {

        public bldMaterialsGroup()
        {
            Name = "Материалы:";
        }
        public bldMaterialsGroup(string name)
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
            foreach (bldMaterial bldMaterial in this.Items)
            {
                total_cost += bldMaterial.Cost;
            }
            Cost = total_cost;

        }
    }
}
