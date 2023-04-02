using PrismWorkApp.Core.Commands;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace PrismWorkApp.Modules.BuildingModule
{
    public class AppObjectsModel : IAppObjectsModel
    {
        #region Commands 
        public NotifyMenuCommands DocumentationCommands { get; set; } = new NotifyMenuCommands();
        public NotifyCommand<object> RemoveDocumentCommand { get; private set; }
        public NotifyCommand<object> CreateNewDocumentCommand { get; set; }
        #endregion
        #region Contructors
        public AppObjectsModel()
        {
            Documentation.Name = "Документация";
            RemoveDocumentCommand = new NotifyCommand<object>(OnRemoveDocument);
            RemoveDocumentCommand.Name = "Удалить документ";
            CreateNewDocumentCommand = new NotifyCommand<object>(OnCreateNewDocument);
            CreateNewDocumentCommand.Name = "Создать документ";
            DocumentationCommands.Add(CreateNewDocumentCommand);
            DocumentationCommands.Add(RemoveDocumentCommand);
        }

        private void OnCreateNewDocument(object obj)
        {
            
        }

        private void OnRemoveDocument(object obj)
        {
           
        }
        #endregion
        #region Model data 
        #region Documentation
        /// <summary>
        /// Коллекция для хранения документации
        /// </summary>
        public bldDocumentsGroup Documentation { get; set; } = new bldDocumentsGroup();
              #endregion
       
        #endregion
        #region DataItems imlemantation
        static private GetImageFrombldProjectObjectConvecter ObjecobjectTo_Url_Convectert = new GetImageFrombldProjectObjectConvecter();
        /// <summary>
        /// Шаблон построения дерева DataItems для TreeView в форме метода, который вызываеся каждый при инициализации или 
        /// обновления DataItem или вызове IPropertyChanged, ICollectionChanged прикрепленных к DataItem объектов
        /// </summary>
        /// <param name="dataItem"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDataItemInit(DataItem dataItem, object sender, PropertyChangedEventArgs e)
        {
            switch (dataItem.AttachedObject.GetType().Name)
            {

                case (nameof(bldAggregationDocument)):
                    {
                        bldAggregationDocument document = ((bldAggregationDocument)dataItem.AttachedObject);
                        dataItem.Text = document.Name;
                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        DataItem attachedDocs = new DataItem();
                        dataItem.Items.Add(attachedDocs);
                        attachedDocs.AttachedObject = document.AttachedDocuments;
                        break;
                    }
                case (nameof(bldDocumentsGroup)):
                case (nameof(bldAggregationDocumentsGroup)):
                case (nameof(bldMaterialCertificatesGroup)):
                    {
                        bldDocumentsGroup documents = ((bldDocumentsGroup)dataItem.AttachedObject);
                        dataItem.Text = documents.Name;
                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        foreach (bldDocument doc in documents)
                        {
                            DataItem atch_doc_item = new DataItem();
                            dataItem.Items.Add(atch_doc_item);
                            atch_doc_item.AttachedObject = doc;
                        }
                        break;
                    }
                case (nameof(bldMaterialCertificate)):
                    {
                        bldMaterialCertificate document = ((bldMaterialCertificate)dataItem.AttachedObject);
                        dataItem.Text = document.MaterialName;
                        dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                        break;
                    }
            }
        }
        #endregion
    }
}
