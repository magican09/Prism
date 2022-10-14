using Prism.Services.Dialogs;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class AddbldParticipantToCollectionViewModel:
        AddElementToCollectionDialogViewModel<bldParticipantsGroup, bldParticipant>
    {
        public AddbldParticipantToCollectionViewModel(IDialogService dialogService)
          : base(dialogService)
        {

        }
    }
}
