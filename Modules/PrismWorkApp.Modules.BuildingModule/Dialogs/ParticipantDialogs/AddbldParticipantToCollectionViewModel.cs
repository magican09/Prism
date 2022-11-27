using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldParticipantToCollectionViewModel :
        AddElementToCollectionDialogViewModel<bldParticipantsGroup, bldParticipant>
    {
        public AddbldParticipantToCollectionViewModel(IDialogService dialogService)
          : base(dialogService)
        {

        }
    }
}
