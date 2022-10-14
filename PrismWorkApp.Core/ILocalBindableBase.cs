using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Services.Repositories;
using System;

namespace PrismWorkApp.Core
{
    public interface ILocalBindableBase
    {
        public Guid Id { get; set; }
        public object SelectedObject { get; set; }
        public bool EditMode { get; set; }
    //    public IDialogService _dialogService { get; set; }
    //    public  void RaiseCanExecuteChanged(object sender, EventArgs e);
     //   public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
   //     public IRegionManager _regionManager { get; set; }
    
    }
}