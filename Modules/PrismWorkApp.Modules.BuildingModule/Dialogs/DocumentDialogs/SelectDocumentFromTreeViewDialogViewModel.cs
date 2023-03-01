using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class SelectDocumentFromTreeViewDialogViewModel :
        SelectElementFromCollectionTreeViewDialogViewModel<ObservableCollection<IbldDocument>, IbldDocument>
    {
        public SelectDocumentFromTreeViewDialogViewModel()
        {

        }
    }
}
