using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class BaseViewModel<TEntity> : LocalBindableBase, INotifyPropertyChanged  where TEntity : class
    {
        protected IDialogService _dialogService;
        protected IRegionManager _regionManager;
        public event Action<IDialogResult> RequestClose;

        private bool _keepAlive = true;
        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }
        //protected IUnDoReDoSystem _commonUnDoReDo;
        //public IUnDoReDoSystem CommonUnDoReDo
        //{
        //    get { return _commonUnDoReDo; }
        //    set { SetProperty(ref _commonUnDoReDo, value); }
        //}

        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }
        protected IUnDoReDoSystem _unDoReDo;

     

        public IUnDoReDoSystem UnDoReDo
        {
            get { return _unDoReDo; }
            set { SetProperty(ref _unDoReDo, value); }
        }

        public string Title => throw new NotImplementedException();

        public BaseViewModel()
        {
            // IsActiveChanged += OnActiveChanged;

        }

        public virtual void OnSave<T>(T selected_obj, string object_name = "") where T : IEntityObject
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

        public virtual void OnClose<T>(object view, T selected_obj, string object_name = "") where T : IEntityObject
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
                            else if(RequestClose!=null)
                            {
                                var _result = ButtonResult.Yes;
                                var param = new DialogParameters();
                                param.Add("confirm_dialog_param", "Подтверждено пользователем!");
                                RequestClose.Invoke(new DialogResult(_result, param));
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
                            else if (RequestClose != null)
                            {
                                var _result = ButtonResult.No;
                                var param = new DialogParameters();
                                param.Add("cancel_dialog_param", "Отменено пользователем!");
                                RequestClose.Invoke(new DialogResult(_result, param));
                            }
                            OnWindowClose();
                        }
                        else if (result.Result == ButtonResult.Cancel)
                        {
                            if (RequestClose != null)
                            {
                                var _result = ButtonResult.Cancel;
                                var param = new DialogParameters();
                                param.Add("cancel_dialog_param", "Отменено пользователем!");
                                RequestClose.Invoke(new DialogResult(_result, param));
                            }
                            OnWindowClose();
                        }
                        else
                        {
                            throw new Exception("_regionManager==null and RequestClose=null! in BaseViewModel<T>.OnClose<T>(object view, T selected_obj, string object_name)");

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
                else
                    if (RequestClose != null)
                {
                    var _result = ButtonResult.No;
                    var param = new DialogParameters();
                    param.Add("cancel_dialog_param", "Отменено пользователем!");
                    RequestClose.Invoke(new DialogResult(_result, param));
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
        #region on Activate event  
        //private void OnActiveChanged(object sender, EventArgs e)
        //{
        //    if (IsActive)
        //        RegisterAplicationCommands();
        //    else
        //        UnRegisterAplicationCommands();

        //}
        //public virtual void RegisterAplicationCommands()
        //{
        //    //ObservableCollection<INotifyCommand> undo_redo_collection =  GetUnDoReDoCommandObjects(_applicationCommands);

        //}
        //public virtual  void UnRegisterAplicationCommands()
        //{

        //}
        //private ObservableCollection<INotifyCommand> GetUnDoReDoCommandObjects(object common_command_object)
        //{
        //    ObservableCollection < INotifyCommand >  undo_redo_collection = new ObservableCollection<INotifyCommand>();

        //   // var file_infos = common_command_object.GetType().GetProperties();

        //  //  foreach(PropertyInfo prop_info in file_infos)
        //    {

        //    }

        //    return undo_redo_collection;
        //}
        #endregion
    }
}
