using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class BaseViewModel<TEntity> : LocalBindableBase, INotifyPropertyChanged where TEntity : class
    {
        protected IDialogService _dialogService;
        protected IRegionManager _regionManager;
        public NotifyCommand UnDoLeftCommand { get; protected set; }
        public NotifyCommand UnDoRightCommand { get; protected set; }
        public NotifyCommand SaveCommand { get; protected set; }
        public NotifyCommand<object> CloseCommand { get; protected set; }

        private bool _keepAlive = true;
      public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }
        protected PropertiesChangeJornal _commonChangeJornal;
        public PropertiesChangeJornal CommonChangeJornal
        {
            get { return _commonChangeJornal; }
            set { SetProperty(ref _commonChangeJornal, value); }
        }

        public BaseViewModel()
        {
            

        }

        public virtual void OnUnDoRight(Guid curentContextIds)
        {
            _commonChangeJornal.UnDoRight(curentContextIds);
        }
         public virtual void OnUnDoLeft(Guid curentContextIds)
        {
            _commonChangeJornal.UnDoLeft(curentContextIds);
        }
     
        public virtual void OnSave<T>(T selected_obj,string object_name="") where T : IJornalable, INameable, IRegisterable, IBindableBase
        {
            CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    _commonChangeJornal.SaveAll(Id);
                }
                if (result.Result == ButtonResult.No)
                {
              //      propertiesChangeJornal.UnDoAll(Id);
                }

            }, _dialogService);
        }
        public  virtual void OnWindowClose()
        {

        }

        public virtual void OnClose<T>(object view, T selected_obj, string object_name = "") where T : IJornalable, INameable, IRegisterable, IBindableBase
        {
            if (_commonChangeJornal != null && _commonChangeJornal.Where(r => r.ContextId == Id).FirstOrDefault() != null)//selected_obj!=null&&добавлено 27,10,22
            {
                CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
                {
                    if (view != null)
                    {
                        if (result.Result == ButtonResult.Yes)
                        {
                            _commonChangeJornal.SaveAll(Id);
                            if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                            {
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                            }
                            OnWindowClose();
                        }
                        else if (result.Result == ButtonResult.No)
                        {
                            _commonChangeJornal.UnDoAll(Id);
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
              if (selected_obj != null && !selected_obj.IsPropertiesChangeJornalIsEmpty(Id))//selected_obj!=null&&добавлено 27,10,22
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
