using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using System;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class BaseViewModel<TEntity> : LocalBindableBase, INotifyPropertyChanged where TEntity : class
    {
        protected IDialogService _dialogService;
        protected IRegionManager _regionManager;


        private bool _keepAlive = true;
        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }
        protected IUnDoReDoSystem _commonUnDoReDo;
        public IUnDoReDoSystem CommonUnDoReDo
        {
            get { return _commonUnDoReDo; }
            set { SetProperty(ref _commonUnDoReDo, value); }
        }

        protected IUnDoReDoSystem _unDoReDo;
        public IUnDoReDoSystem UnDoReDo
        {
            get { return _unDoReDo; }
            set { SetProperty(ref _unDoReDo, value); }
        }
        public BaseViewModel()
        {


        }

        public virtual void OnSave<T>(T selected_obj, string object_name = "") where T : IJornalable, INameable, IRegisterable, IBindableBase
        {
            CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    //   CommonUnDoReDo.AddUnDoReDo(UnDoReDo);
                    UnDoReDo.ClearStacks();
                }
                if (result.Result == ButtonResult.No)
                {
                }

            }, _dialogService);
        }
        public virtual void OnWindowClose()
        {

        }

        public virtual void OnClose<T>(object view, T selected_obj, string object_name = "") where T : IJornalable, INameable, IRegisterable, IBindableBase
        {
            if (UnDoReDo != null && !UnDoReDo.IsSatcksEmpty())//selected_obj!=null&&добавлено 27,10,22
            {
                CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
                {
                    if (view != null)
                    {
                        if (result.Result == ButtonResult.Yes)
                        {
                            //    CommonUnDoReDo.AddUnDoReDo(UnDoReDo);
                            UnDoReDo.ClearStacks();
                            if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                            {
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                            }
                            OnWindowClose();
                        }
                        else if (result.Result == ButtonResult.No)
                        {
                            UnDoReDo.UnDoAll();
                            UnDoReDo.ClearStacks();
                            if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                            {
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                            }
                            OnWindowClose();
                        }
                        else if (result.Result == ButtonResult.Cancel)
                        {

                        }
                    }
                }, _dialogService);

            }
            else
            {
                if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                {
                    _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                    _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                }
                OnWindowClose();
            }

        }

        /* 
         *    public virtual void OnSave<T>(T selected_obj, string object_name = "") where T : IJornalable, INameable, IRegisterable, IBindableBase
          {
              CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
              {
                  if (result.Result == ButtonResult.Yes)
                  {
                      selected_obj.SaveAll(Id);
                  }
                  if (result.Result == ButtonResult.No)
                  {
                     selected_obj.UnDoAll(Id);
                  }

              }, _dialogService);
          }

         * public virtual void OnClose<T>(object view, T selected_obj, string object_name = "") where T : IJornalable, INameable, IRegisterable, IBindableBase
          {
              if (selected_obj != null && !selected_obj.IsUnDoReDoSystemIsEmpty(Id))//selected_obj!=null&&добавлено 27,10,22
              {
                  CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
                  {
                      if (view != null)
                      {
                          if (result.Result == ButtonResult.Yes)
                          {
                              selected_obj.SaveAll(Id);
                              if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                              {
                                  _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                  _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                              }
                              OnWindowClose();
                          }
                          else if (result.Result == ButtonResult.No)
                          {
                              selected_obj.UnDoAll(Id);
                              if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                              {
                                  _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                  _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                              }
                              OnWindowClose();
                          }
                          else if (result.Result == ButtonResult.Cancel)
                          {

                          }
                      }
                  }, _dialogService);

              }
              else
              {
                  if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                  {
                      _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                      _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                  }
                  OnWindowClose();
              }

          }
          */
    }
}
