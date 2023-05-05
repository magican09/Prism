using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.Dialogs
{
    public class SelectObjectInAppModelDialogViewModel: SelectObjectFromAppModelDialogVewModelBase<bldDocument>
    {

        public SelectObjectInAppModelDialogViewModel(IAppObjectsModel appObjectsModel):base(appObjectsModel)
        {

        }
    }
}
