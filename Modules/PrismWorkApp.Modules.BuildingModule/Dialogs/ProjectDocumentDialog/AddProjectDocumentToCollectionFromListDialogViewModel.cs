using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

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
