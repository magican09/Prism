using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
