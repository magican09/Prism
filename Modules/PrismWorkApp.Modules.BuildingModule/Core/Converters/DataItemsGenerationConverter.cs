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

                    case (nameof(bldAggregationDocument)):
                    case (nameof(bldLaboratoryReport)):
                    case (nameof(bldExecutiveScheme)):
                        {
                            bldDocument document = ((bldDocument)dataItem.AttachedObject);
                            Binding binding = new Binding("Name");
                            binding.Source = document;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                            DataItem attachedDocs_dataitem = new DataItem();
                            dataItem.Items.Add(attachedDocs_dataitem);
                            attachedDocs_dataitem.AttachedObject = document.AttachedDocuments;
                            break;
                        }
                    
                    case (nameof(bldMaterialCertificate)):
                        {
                            bldDocument document = ((bldDocument)dataItem.AttachedObject);
                            Binding binding = new Binding("MaterialName");
                            binding.Source = document;
                            binding.Path = new PropertyPath("MaterialName");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);

                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                            DataItem attachedDocs_dataitem = new DataItem();
                            dataItem.Items.Add(attachedDocs_dataitem);
                            attachedDocs_dataitem.AttachedObject = document.AttachedDocuments;
                            break;
                        }
                    case (nameof(bldUnitOfMeasurement)):
                        {
                            IEntityObject entity = ((IEntityObject)dataItem.AttachedObject);
                            Binding binding = new Binding("Name");
                            binding.Source = entity;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);
                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
                            break;
                        }
                    case (nameof(TypeOfFile)):
                        {
                            TypeOfFile entity = ((TypeOfFile)dataItem.AttachedObject);
                            Binding binding = new Binding("Name");
                            binding.Source = entity;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);
                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);

                            DataItem attachedDocs_dataitem = new DataItem();
                            dataItem.Items.Add(attachedDocs_dataitem);
                            attachedDocs_dataitem.AttachedObject = entity;

                            Binding binding_2 = new Binding("Extention");
                            binding_2.Source = entity;
                            binding_2.Path = new PropertyPath("Extention");
                            binding_2.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(attachedDocs_dataitem, DataItem.TextProperty, binding_2);
                            break;
                        }
                    case (nameof(bldDocumentsGroup)):
                    case (nameof(bldAggregationDocumentsGroup)):
                    case (nameof(bldMaterialCertificatesGroup)):
                    case (nameof(bldUnitOfMeasurementsGroup)):
                    case (nameof(TypesOfFileGroup)):
                        {
                            INameableObservableCollection entity_collection = (INameableObservableCollection)dataItem.AttachedObject;
                            Binding binding = new Binding("Name");
                            binding.Source = entity_collection;
                            binding.Path = new PropertyPath("Name");
                            binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(dataItem, DataItem.TextProperty, binding);
                            dataItem.ImageUrl = (Uri)ObjecobjectTo_Url_Convectert.Convert(dataItem.AttachedObject, null, null, CultureInfo.CurrentCulture);
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
