using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core
{

    //[ValueConversion(typeof(object), typeof(object))]
    public class ObjectsToTreeViewNodeModelConvecter : IValueConverter
    {
        public static ObjectsToTreeViewNodeModelConvecter Instance = new ObjectsToTreeViewNodeModelConvecter();
        public object Convert(object value, Type targetType = null, object parameter = null, System.Globalization.CultureInfo culture = null)
        {

            Nodes rootNodes = new Nodes();
            var dfd = value.GetType();
            var dfdf = value.GetType().GetProperties();
            /*  if (value is IEnumerable<IRegisterable> || value is INameableOservableCollection<KeyValue>)
              {
                  rootNodes.Name = "Имя корневого узала";// ((INameable)value).Name;
                 if(value is IEnumerable<IRegisterable>)
                  foreach (var val in (IEnumerable<IRegisterable>)value)
                  {
                      Node new_node = new Node(((INameable)val).Name);
                      new_node.Value = val;
                      rootNodes.Add(new_node);
                  }
                 else
                      foreach (var val in (INameableOservableCollection<KeyValue>)value)

                      {
                          Node new_node = new Node(val.Key);
                      new_node.Value = val;
                      rootNodes.Add(new_node);
                  }
              }
              else
                  foreach (var prop in value.GetType().GetProperties())
                  {

                      var prop_value = prop.GetValue(value, null);

                      if (prop_value is IEnumerable<IRegisterable> || prop_value is INameable)
                      {

                          Node new_node = new Node(((INameable)prop_value).Name);
                          new_node.Value = prop_value;
                          rootNodes.Add(new_node);
                      }


                  }

              */
            return rootNodes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
    public class GetImageFrombldProjectObjectConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type node_object_type = value.GetType();
            string img_suffix = "";

            switch (value.GetType().Name)
            {
                case nameof(bldParticipant):
                    {
                        bldParticipant participant = (bldParticipant)value;
                        switch (participant.Role.RoleCode)
                        {
                            case ParticipantRole.DEVELOPER:
                                img_suffix = "_DEVELOPER";
                                break;
                            case ParticipantRole.GENERAL_CONTRACTOR:
                                img_suffix = "_GENERAL_CONTRACTOR";
                                break;
                            case ParticipantRole.DISIGNER:
                                img_suffix = "_DISIGNER";
                                break;
                            case ParticipantRole.BUILDER:
                                img_suffix = "_BUILDER";
                                break;
                            case ParticipantRole.NONE:
                                img_suffix = "_NONE";
                                break;
                        }

                        break;
                    }
                case nameof(bldResponsibleEmployee):
                    {
                        bldResponsibleEmployee employee = (bldResponsibleEmployee)value;
                        switch (employee.Role.RoleCode)
                        {
                            case RoleOfResponsible.CUSTOMER:
                                img_suffix = "_CUSTOMER";
                                break;
                            case RoleOfResponsible.GENERAL_CONTRACTOR:
                                img_suffix = "_GENERAL_CONTRACTOR";
                                break;
                            case RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER:
                                img_suffix = "_GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER";
                                break;
                            case RoleOfResponsible.AUTHOR_SUPERVISION:
                                img_suffix = "_AUTHOR_SUPERVISION";
                                break;
                            case RoleOfResponsible.WORK_PERFORMER:
                                img_suffix = "_WORK_PERFORMER";
                                break;
                            case RoleOfResponsible.OTHER:
                                img_suffix = "_OTHER";
                                break;
                            case RoleOfResponsible.NONE:
                                img_suffix = "_NONE";
                                break;
                        }
                        break;
                    }
            }

            string type_name = node_object_type.Name;

            var image = "Images/Ribbon/32x32/add.png";
            image = $"Images/bldProjectImages/{type_name}{img_suffix}.png";
            Uri img_uri = new Uri($"/PrismWorkApp.Modules.BuildingModule;component/Resourses/{image}", UriKind.Relative);




            // Uri img_uri_ = new Uri($"pack://application:,,,/Resourses/Images/Ribbon/32x32/add.png");

            //new BitmapImage(img_uri);
            return img_uri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SetTreeViewVisibilityFrombldObjectState : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((bool)value) == true) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GetCollectionFrombldProjectModelConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type val_type = value.GetType();
            ObservableCollection<object> collection = new ObservableCollection<object>();
            switch (value.GetType().Name)
            {
                case nameof(bldProject):
                    {
                        bldProject project = value as bldProject;
                        if (project.BuildingObjects != null) collection.Add(project.BuildingObjects);
                        if (project.Participants != null) collection.Add(project.Participants);
                        if (project.ResponsibleEmployees != null) collection.Add(project.ResponsibleEmployees);
                        break;
                    }
                case nameof(bldObjectsGroup):
                    {
                        /*  foreach(bldObject obj in (bldObjectsGroup)value)
                              collection.Add(obj);
                         */
                        bldObjectsGroup bld_objects = value as bldObjectsGroup;
                        if (bld_objects.Count > 0)
                            return value;
                        else
                            return null;
                        break;
                        return value;
                        break;
                    }
                case nameof(bldObject):
                    {
                        bldObject bld_object = value as bldObject;
                        if (bld_object.BuildingObjects != null) collection.Add(bld_object.BuildingObjects);
                        if (bld_object.Constructions != null) collection.Add(bld_object.Constructions);
                        //  bld_object.BuildingObjects.Add(new bldObject());
                        // collection.Add(bld_object.BuildingObjects);
                        //  collection.Add(bld_object.Constructions);

                        break;
                    }
                case nameof(bldConstructionsGroup):
                    {
                        // foreach (bldObject obj in (bldConstructionsGroup)value)
                        //collection.Add(obj);
                        return value;
                        break;
                    }
                case nameof(bldConstructionCompanyGroup):
                    {
                        // foreach (bldObject obj in (bldConstructionsGroup)value)
                        //collection.Add(obj);
                        return value;
                        break;
                    }

                case nameof(bldConstruction):
                    {
                        if (((bldConstruction)value).Constructions != null) collection.Add(((bldConstruction)value).Constructions);
                        if (((bldConstruction)value).Works != null)
                        {
                            /*ListCollectionView view = new ListCollectionView((IList)((bldConstruction)value).Works);
                            SortDescription sort = new SortDescription("Date", ListSortDirection.Ascending);
                            view.SortDescriptions.Add(sort);
                            collection.Add(view);*/
                    //        bldWorksGroup  sorted_coll = new bldWorksGroup(((bldConstruction)value).Works);
                            collection.Add(((bldConstruction)value).Works);

                        }
                        break;
                    }
                case nameof(bldWorksGroup):
                    {

                        //  bldWorksGroup sorted_coll = new bldWorksGroup(((bldWorksGroup)value).OrderByDescending(w=>w.Date));
                        /*  CollectionViewSource collectionViewSource = new CollectionViewSource();
                          collectionViewSource.Source = value;
                            collectionViewSource.View.SortDescriptions.Add(sort);
                          */
                        SortDescription sort = new SortDescription("StartTime", ListSortDirection.Ascending);
                        ICollectionView collectionView;
                        collectionView = CollectionViewSource.GetDefaultView((bldWorksGroup)value);
                  //      collectionView.SortDescriptions.Add(sort);
                   //     Task task  = new Task( ()=> { collectionView.SortDescriptions.Add(sort); });
                    //    task.Wait();
                        return collectionView;
                        break;
                    }
                case nameof(bldWork):
                    {
                        bldWork work = value as bldWork;
                        /* if (work.AOSRDocuments != null) collection.Add(work.AOSRDocuments);
                         if (work.LaboratoryReports != null) collection.Add(work.LaboratoryReports);
                         if (work.Materials != null) collection.Add(work.Materials);
                         if (work.PreviousWorks != null) collection.Add(work.PreviousWorks);
                         if (work.NextWorks != null) collection.Add(work.NextWorks);*/

                        /*if (work.AOSRDocuments != null) collection.Add(work.AOSRDocuments);
                        if (work.LaboratoryReports != null) collection.Add(work.LaboratoryReports);
                        if (work.ExecutiveSchemes != null) collection.Add(work.ExecutiveSchemes);
                        */
                        if (work.PreviousWorks != null) collection.Add(work.PreviousWorks);
                        if (work.NextWorks != null) collection.Add(work.NextWorks);
                        if (work.Materials != null) collection.Add(work.Materials);
                        NameableObservabelObjectsCollection docs_treeViewItem = new NameableObservabelObjectsCollection();
                        docs_treeViewItem.Name = "Документация";
                        if (work.AOSRDocuments != null) docs_treeViewItem.Add(work.AOSRDocuments);
                        if (work.LaboratoryReports != null) docs_treeViewItem.Add(work.LaboratoryReports);
                        if (work.ExecutiveSchemes != null) docs_treeViewItem.Add(work.ExecutiveSchemes);

                        if (work.Materials != null)
                        {

                            NameableObservabelObjectsCollection materials_docs = new NameableObservabelObjectsCollection();
                            materials_docs.Name = "Документы на материалы";
                            foreach (bldMaterial material in work.Materials)
                                foreach (bldDocument document in material.Documents)
                                    materials_docs.Add(document);
                            docs_treeViewItem.Add(materials_docs);

                        }
                        collection.Add(docs_treeViewItem);
                        break;
                    }
                case nameof(bldParticipant):
                    {
                        bldParticipant participant = value as bldParticipant;
                        //foreach (bldConstructionCompany company in participant.ConstructionCompanies)
                        //{
                        //    collection.Add(company);
                        //    var employees = company?.ResponsibleEmployees
                        //        .Where(emp => bldProjectFuncions.ResponsibleRoleComparer(participant.Role, emp.RoleOfResponsible)).ToList();
                        //    foreach (bldResponsibleEmployee employee in employees)
                        //    {
                        //        collection.Add(employee);
                        //    }
                        //}
                        foreach (bldResponsibleEmployee employee in participant.ResponsibleEmployees)
                                {
                                    collection.Add(employee);
                                }
                            break;
                    }
                case nameof(bldCompany):
                    {
                        bldCompany company = value as bldCompany;
                        collection.Add(company.FullName);

                        break;
                    }
                case nameof(bldConstructionCompany):
                    {
                        bldConstructionCompany company = value as bldConstructionCompany;
                        collection.Add(company.FullName);

                        break;
                    }
                case nameof(bldParticipantsGroup):
                    {
                        //collection.Add(obj);
                        return value;
                        break;
                    }
                case nameof(bldAOSRDocumentsGroup):
                    {
                        return value;
                        break;
                    }

                case nameof(bldAOSRDocument):
                    {
                        bldAOSRDocument document = value as bldAOSRDocument;
                        if (document.AttachedDocuments != null) collection.Add(document.AttachedDocuments);
                        if (document.ResponsibleEmployees != null) collection.Add(document.ResponsibleEmployees);
                        break;
                    }
                case nameof(bldMaterialsGroup):
                    {
                        return value;
                        break;
                    }

                case nameof(bldMaterial):
                    {
                        bldMaterial material = value as bldMaterial;
                        if (material?.Documents != null) collection.Add(material.Documents);

                        break;
                    }
                case nameof(bldMaterialCertificate):
                    {
                        bldMaterialCertificate document = value as bldMaterialCertificate;
                        if (document?.AttachedDocuments != null) collection.Add(document.AttachedDocuments);

                        break;
                    }
                case nameof(bldLaboratoryReportsGroup):
                    {
                        return value;
                        break;
                    }

                case nameof(bldLaboratoryReport):
                    {
                        bldLaboratoryReport report = value as bldLaboratoryReport;
                        if (report?.AttachedDocuments != null) collection.Add(report.AttachedDocuments);

                        break;
                    }
                case nameof(bldExecutiveSchemesGroup):
                    {
                        return value;
                        break;
                    }

                case nameof(bldExecutiveScheme):
                    {
                        bldExecutiveScheme scheme = value as bldExecutiveScheme;
                        if (scheme?.AttachedDocuments != null) collection.Add(scheme.AttachedDocuments);

                        break;
                    }
                case nameof(bldDocumentsGroup):
                    {
                        return value;
                        break;
                    }

                case nameof(bldDocument):
                    {
                        bldDocument document = value as bldDocument;
                        if (document?.AttachedDocuments != null) collection.Add(document.AttachedDocuments);

                        break;
                    }
                case nameof(NameableObservabelObjectsCollection):
                    {
                        return value;
                        break;
                    }



            }

            return collection;
        }

        private bool FilterWorks(object obj)
        {
            if (obj is bldWork work) return true;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GetKeyValueFromChildrenKeyValuesConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* Type val_type = value.GetType();
             if (value is INameableOservableCollection<KeyValue>)
                 foreach(KeyValue val in (INameableOservableCollection<KeyValue>)value )
                     if(val.Key== parameter.ToString())
                 return val.Value;
            */

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class testConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type val_type = value.GetType();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GetChildOfTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fdf = CoreFunctions.GetChildOfType<DataGrid>((DependencyObject)value);
            return CoreFunctions.GetChildOfType<DataGrid>((DependencyObject)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GetChildOfTypeSelectedItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fdf = CoreFunctions.GetChildOfType<DataGrid>((DependencyObject)value).SelectedItem;
            return fdf;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
