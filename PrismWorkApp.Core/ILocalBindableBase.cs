using System;

namespace PrismWorkApp.Core
{
    public interface ILocalBindableBase
    {
         Guid Id { get; set; }
        //     public object SelectedObject { get; set; }
         bool EditMode { get; set; }
        //    public IDialogService _dialogService { get; set; }
        //    public  void RaiseCanExecuteChanged(object sender, EventArgs e);
        //   public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
        //     public IRegionManager _regionManager { get; set; }

    }
}