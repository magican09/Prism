using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Core 
{
    public class GetImageFrombldProjectObjectConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;
            Type node_object_type = value.GetType();
            string type_name = node_object_type.Name;
            string img_suffix = "";
            var image_path = "";
            string bldProjectImagesDir = $"Images/bldProjectImages";

            switch (node_object_type.Name)
            {
                case (nameof(NameableObservableCollection<IEntityObject>)):
                    {

                        break;
                    }
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
                case nameof(TypeOfFile):
                    {

                        TypeOfFile type_of_file = (TypeOfFile)value;
                        bldProjectImagesDir = $"Images/bldProjectImages/TypesOfFile";
                        if (type_of_file.Extention != null)
                            type_name = type_of_file.Extention.Replace("*.", "");
                        if (type_of_file.Extention == "*.pdf")
                            ;
                        else
                            ;
                        img_suffix = "";
                        break;
                    }
            }


            image_path = $"{bldProjectImagesDir}/{type_name}{img_suffix}.png";
            Uri img_uri = new Uri($"/PrismWorkApp.Modules.BuildingModule;component/Resourses/{image_path}", UriKind.Relative);




            // Uri img_uri_ = new Uri($"pack://application:,,,/Resourses/Images/Ribbon/32x32/add.png");

            //new BitmapImage(img_uri);
            return img_uri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
