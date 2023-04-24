using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core 
{
    public class DataItemsGenerationConverter : IValueConverter
    {
        static private GetImageFrombldProjectObjectConvecter ObjecobjectTo_Url_Convectert = new GetImageFrombldProjectObjectConvecter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataItem dataItem = value as DataItem;
            if (dataItem != null && dataItem.AttachedObject!=null)
            {
               string type_name = dataItem.AttachedObject.GetType().Name;
                //    if (dataItem.AllItems.ContainsKey(dataItem.AttachedObject)) 
                //       return;
                if (dataItem.Items == null) dataItem.Items = new DataItemCollection(dataItem);
                switch (dataItem.AttachedObject.GetType().Name)
                {

                    case (nameof(bldMaterialCertificate)):
                        {
                            bldMaterialCertificate document = ((bldMaterialCertificate)dataItem.AttachedObject);
                            Binding binding = new Binding("MaterialName");
                            binding.Source = document;
                            binding.Path = new PropertyPath("MaterialName");
                            binding.Mode = BindingMode.OneWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                            DataItem attachedDocs_dataitem = new DataItem();
                            dataItem.Items.Add(attachedDocs_dataitem);
                            attachedDocs_dataitem.AttachedObject = document.AttachedDocuments;
                            break;
                        }
                    case (nameof(bldAggregationDocument)):
                        {
                            bldAggregationDocument document = ((bldAggregationDocument)dataItem.AttachedObject);
                            Binding binding = new Binding("Name");
                            binding.Source = document;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                           // dataItem.SetItems(document.AttachedDocuments);
                            DataItem attachedDocs = new DataItem();
                            attachedDocs.AttachedObject = document.AttachedDocuments;
                            dataItem.Items.Add(attachedDocs);
                            break;
                        }
                    case (nameof(bldDocumentsGroup)):
                    case (nameof(bldAggregationDocumentsGroup)):
                    case (nameof(bldMaterialCertificatesGroup)):
                        {
                            bldDocumentsGroup documents = ((bldDocumentsGroup)dataItem.AttachedObject);
                            Binding binding = new Binding("Name");
                            binding.Source = documents;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);
                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(new bldDocumentsGroup(), null, null, CultureInfo.CurrentCulture);
                     //       dataItem.SetItems(documents);

                            //foreach (bldDocument doc in documents)
                            //{
                            //    DataItem atch_doc_item = new DataItem();
                            //    dataItem.Items.Add(atch_doc_item);
                            //    atch_doc_item.AttachedObject = doc;

                            //}
                            break;
                        }
                    case (nameof(bldUnitOfMeasurement)):
                        {
                            bldUnitOfMeasurement unit_of_measurement = ((bldUnitOfMeasurement)dataItem.AttachedObject);
                            Binding binding = new Binding("Name");
                            binding.Source = unit_of_measurement;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);
                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                            break;
                        }
                    case (nameof(bldUnitOfMeasurementsGroup)):
                        {
                            IList entity_collection = (IList)dataItem.AttachedObject;
                            Binding binding = new Binding("Name");
                            binding.Source = entity_collection;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                         //   dataItem.SetItems(entity_collection);
                           //foreach (IEntityObject entity in entity_collection)
                            //{
                            //    DataItem ent_item = new DataItem();
                            //    //dataItem.Items.Add(ent_item);
                            //    ent_item.AttachedObject = entity;
                            //}
                            break;
                        }
                    case ("NameableObservableCollection`1"): //Если тип объекта NameableObservableCollection<IEntityObject>
                        {
                            IList entity_collection = (IList)dataItem.AttachedObject;
                            Binding binding = new Binding("Name");
                            binding.Source = entity_collection;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(new NameableObservableCollection<IEntityObject>(), null, null, CultureInfo.CurrentCulture);
                            //dataItem.SetItems(entity_collection);
                            //foreach (IEntityObject entity in entity_collection)
                            //{
                            //    DataItem ent_item = new DataItem();
                            //    dataItem.Items.Add(ent_item);
                            //    ent_item.AttachedObject = entity;
                            //}
                            break;
                        }

                }
            }
            return dataItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
