﻿using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class BaseViewModel<TEntity> : LocalBindableBase, INotifyPropertyChanged where TEntity : class
    {
        protected IDialogService _dialogService;
        protected  IRegionManager _regionManager;

        
        public void OnSave<T>(T selected_obj) where T:IJornalable,INameable,IRegisterable, IBindableBase
        {
            CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", "проект", "Сохранить", "Не сохранять", "Отмена", (result) =>
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

        public void  OnClose<T>(object view,T selected_obj) where T : IJornalable, INameable, IRegisterable, IBindableBase
        {
            if (!selected_obj.IsPropertiesChangeJornalIsEmpty(Id))
            {
                CoreFunctions.ConfirmActionOnElementDialog<T>(selected_obj, "Сохранить", "проект", "Сохранить", "Не сохранять", "Отмена", (result) =>
                {
                    if (view != null)
                    {
                        if (result.Result == ButtonResult.Yes)
                        {
                            selected_obj.SaveAll(Id);
                            if (_regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);

                        }
                        else if (result.Result == ButtonResult.No)
                        {
                            selected_obj.UnDoAll(Id);
                            if (_regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                                _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
                        }
                        else if (result.Result == ButtonResult.Cancel)
                        {

                        }
                    }
                }, _dialogService);

            }
            else
            {
                if (_regionManager.Regions[RegionNames.ContentRegion].Views.Contains(view))
                    _regionManager.Regions[RegionNames.ContentRegion].Deactivate(view);
            }

        }
    }
}
