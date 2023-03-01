using Prism;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Runtime.CompilerServices;
using BindableBase = Prism.Mvvm.BindableBase;

namespace PrismWorkApp.Core
{
    public class LocalBindableBase : BindableBase, ILocalBindableBase, IActiveAware
    {
        //public IDialogService _dialogService { get; set; }
        //public IBuildingUnitsRepository _buildingUnitsRepository { get; set; }
        //public   IRegionManager _regionManager { get; set; }
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
                //SetPropertiesCurrentId();

            }
        }
        //private void SetPropertiesCurrentId()
        //{
        //    var prop_infoes = this.GetType().GetProperties().Where(pr => pr.GetIndexParameters().Length == 0 &&
        //                                                            pr.PropertyType is IJornalable);
        //    foreach (PropertyInfo prop_info in prop_infoes)
        //    {
        //        IJornalable prop_val = (IJornalable)prop_info.GetValue(this);
        //        if (prop_val != null)
        //        {
        //            prop_val.CurrentContextId = Id;
        //        }
        //    }
        //}
        protected override bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (val is ICuntextIdable)
            {
                ((ICuntextIdable)val).CurrentContextId = Id;
            }
            return base.SetProperty(ref member, val, propertyName);
        }

        /*   private object _selectedObject;
           public object SelectedObject
           {
               get { return _selectedObject; }
               set { SetProperty(ref _selectedObject, value); }
           }
           */
        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public event EventHandler IsActiveChanged;

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnIsActiveChanged();
            }
        }


        public virtual void OnClose(object obj)
        {

        }

        private void OnIsActiveChanged()
        {
            //UpdateCommand.IsActive = IsActive; //set the command as active
            IsActiveChanged?.Invoke(this, new EventArgs()); //invoke the event for all listeners
        }
        public virtual void OnSave()
        {

        }

        //public virtual void RaiseCanExecuteChanged(object sender, EventArgs e)
        //{

        //}


    }
}
