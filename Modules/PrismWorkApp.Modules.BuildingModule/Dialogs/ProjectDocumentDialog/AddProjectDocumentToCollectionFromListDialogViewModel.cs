using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddProjectDocumentToCollectionFromListDialogViewModel :
        AddElementsToCollectionDialogFromListViewModel<bldProjectDocumentsGroup, bldProjectDocument>
    {
        public AddProjectDocumentToCollectionFromListDialogViewModel(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
