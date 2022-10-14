using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Core
{
    public class LocalBindableBase : BindableBase, ILocalBindableBase
    {
        //public IDialogService _dialogService { get; set; }
        //public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
        //public   IRegionManager _regionManager { get; set; }
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        protected override bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (val is IJornalable)
                ((IJornalable)val).CurrentContextId = Id;
            return base.SetProperty(ref member, val, propertyName);
        }

        private object _selectedObject;
        public object SelectedObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value); }
        }
        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }
        public virtual  void OnSave()
        {

        }
        public virtual void OnClose(object obj)
        {

        }
        //public virtual void RaiseCanExecuteChanged(object sender, EventArgs e)
        //{

        //}
     
       
    }
}
