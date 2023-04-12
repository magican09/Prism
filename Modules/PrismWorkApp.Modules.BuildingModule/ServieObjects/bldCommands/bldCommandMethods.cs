using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule 
{
    public   class bldCommandMethods
    {


        public bldCommandMethods(IEventAggregator eventAggregator,
                            IRegionManager regionManager, IDialogService dialogService,
                                           IBuildingUnitsRepository buildingUnitsRepository, IApplicationCommands applicationCommands, IAppObjectsModel appObjectsModel, IUnDoReDoSystem unDoReDoSystem)
        {


        }
    
        //private void OnRemoveAggregationDocument(object obj)
        //{
        //    object selected_object = null;
        //    if (obj is IList list) selected_object = list[0]; else selected_object = obj;
        //    if (selected_object is bldDocument document)
        //    {
        //        int ch_namber = UnDoReDo.GetChangesNamber(document); //Отнимает 1, так как в изменениях 
        //        if (ch_namber != 0)
        //        {
        //            CoreFunctions.ConfirmChangesDialog(_dialogService, "документе",
        //                (result) =>
        //                {
        //                    if (result.Result == ButtonResult.Yes || result.Result == ButtonResult.No)
        //                    {
        //                        foreach (object parent in new List<object>(document.Parents))
        //                        {
        //                            if (parent is bldDocument parent_doc)
        //                                parent_doc.RemoveDocument(document);
        //                            if (parent is IList list_parent)
        //                                list_parent.Remove(document);
        //                        }
        //                        if (result.Result == ButtonResult.Yes)
        //                        {
        //                            UnDoReDo.Save(document);
        //                            UnDoReDo.UnRegister(document);
        //                            var dialog_par = new DialogParameters();
        //                            dialog_par.Add("message", $"{ch_namber.ToString()} изменения(й) сохранено!");
        //                            _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
        //                        }
        //                        if (result.Result == ButtonResult.No)
        //                        {
        //                            UnDoReDo.UnDoAll(document);
        //                            UnDoReDo.UnRegister(document);
        //                            Documentation.Remove(document);
        //                            var dialog_par = new DialogParameters();
        //                            dialog_par.Add("message", $"{ch_namber.ToString()} изменения(й) сброшено!");
        //                            _dialogService.ShowDialog(nameof(MessageDialog), dialog_par, (result) => { });
        //                        }

        //                    }
        //                    if (result.Result == ButtonResult.Cancel)
        //                    {

        //                    }
        //                });

        //        }
        //        else
        //        {
        //            Documentation.Remove(document);
        //        }





        //    }
        //}

    }
}
