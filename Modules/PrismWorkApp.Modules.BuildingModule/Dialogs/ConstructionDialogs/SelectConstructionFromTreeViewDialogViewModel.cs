using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class SelectConstructionFromTreeViewDialogViewModel :
        SelectElementFromCollectionTreeViewDialogViewModel<ObservableCollection<IEntityObject>, IEntityObject>
    {
        public SelectConstructionFromTreeViewDialogViewModel()
        {

        }
    }
}
