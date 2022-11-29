using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldParticipantToCollectionViewModel :
        AddElementToCollectionDialogViewModel<ObservableCollection<bldParticipant>, bldParticipant>
    {
        public AddbldParticipantToCollectionViewModel(IDialogService dialogService)
          : base(dialogService)
        {

        }
    }
}
