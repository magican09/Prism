using Prism.Regions;
using System.Linq;
using System.Windows.Controls.Ribbon;

namespace PrismWorkApp.Core
{
    public class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
    {
        private Ribbon _ribbonTarget;

        public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {

        }

        protected override void Adapt(IRegion region, Ribbon regionTarget)
        {
            _ribbonTarget = regionTarget;

            region.Views.CollectionChanged += delegate
            {
                foreach (RibbonTab tab in region.Views.Cast<RibbonTab>())
                {
                    if (!_ribbonTarget.Items.Contains(tab))
                    {
                        _ribbonTarget.Items.Add(tab);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
