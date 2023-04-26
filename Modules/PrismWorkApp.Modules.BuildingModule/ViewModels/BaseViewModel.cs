using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Dialogs;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.ComponentModel;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class BaseViewModel<TEntity> : LocalBindableBase, INotifyPropertyChanged where TEntity : class
    {
        private string _title = "______";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        protected IDialogService _dialogService;
        protected IRegionManager _regionManager;
        public event Action<IDialogResult> RequestClose;

        private bool _keepAlive = true;
        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }

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



        public BaseViewModel()
        {
            // IsActiveChanged += OnActiveChanged;
            IsActiveChanged += OnIsAciveChanged;
        }
        public virtual void OnSave(string object_name = "")
        {
            var dialog_par = new DialogParameters();
            dialog_par.Add("massege",
               $"Вы действительно хотите сохранить измениния в {object_name }\" ?!");
            dialog_par.Add("confirm_button_content", "Cохранить");
            dialog_par.Add("refuse_button_content", "Отмена");
            dialog_par.Add("cancel_button_content", "Закрыть");
            _dialogService.ShowDialog(typeof(ConfirmActionDialog).Name, dialog_par, result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    UnDoReDo.SaveAll();

                }
            });

        }
        public virtual void OnSave<T>(T selected_obj, Action<IDialogResult> result, string object_name = "") where T : IEntityObject
        {
            CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена",
            result, _dialogService);
        }
        public virtual void OnSave<T>(T selected_obj, string object_name = "") where T : IEntityObject
        {
            CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    //   CommonUnDoReDo.AddUnDoReDoSysAsCommand(UnDoReDo);
                    UnDoReDo.Save(selected_obj);
                }
                if (result.Result == ButtonResult.No)
                {
                }

            }, _dialogService);
        }
        public virtual void OnWindowClose()
        {

        }
        public virtual void OnClose<T>(object view, T selected_obj, Action<IDialogResult> result_action, string object_name = "") where T : IEntityObject
        {
            if (UnDoReDo != null && !UnDoReDo.IsSatcksEmpty())//selected_obj!=null&&добавлено 27,10,22
            {
                CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", object_name, "Сохранить", "Не сохранять", "Отмена", (result) =>
                {

                    if (view != null)
                    {
                        result_action.Invoke(result);
                        if (result.Result == ButtonResult.Yes)
                        {

                            //  UnDoReDo.SaveAllChages(selected_obj);
                            if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                            {
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                            }
                            else if (RequestClose != null)
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
                            //   UnDoReDo.UnDoAll();
                            //   UnDoReDo.SaveAllChages();
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
                            // OnWindowClose();
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

                            UnDoReDo.Save(selected_obj);
                            if (_regionManager != null && _regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                            {
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                                _regionManager.Regions[RegionNames.ContentRegion].Remove(view);
                            }
                            else if (RequestClose != null)
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
                            UnDoReDo.SaveAll();
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


        #region on Activate event  
        private void OnIsAciveChanged(object sender, EventArgs e)
        {
            if (UnDoReDo != null)
            {
                if (this.IsActive == false)
                {
                    if (UnDoReDo.ChangedObjects.Count > 0)
                        this.OnSave<bldDocumentsGroup>(null,
                         (result) =>
                         {
                             if (result.Result == ButtonResult.Yes)
                             {
                                 UnDoReDo.ParentUnDoReDo.SaveAll();
                             }
                             if (result.Result == ButtonResult.No)
                             {
                                 UnDoReDo.UnDoAll();
                                 UnDoReDo.ClearStacks();
                             }
                         });

                    foreach (IJornalable obj in UnDoReDo._RegistedModels.Keys)
                    {
                        obj.UnDoReDoSystem = UnDoReDo.ParentUnDoReDo;
                    }
                }
                else
                {
                    foreach (IJornalable obj in UnDoReDo._RegistedModels.Keys)
                    {
                        obj.UnDoReDoSystem = UnDoReDo;
                    }
                }
            }
        }
        #endregion
    }
}
