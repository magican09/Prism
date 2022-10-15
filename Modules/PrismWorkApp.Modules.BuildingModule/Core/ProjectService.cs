using Microsoft.Win32;
using OfficeOpenXml;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
/*using AOSRDocument = PrismWorkApp.ProjectModel.Data.Models.AOSRDocument;
using Document = PrismWorkApp.ProjectModel.Data.Models.Document;
using Position = PrismWorkApp.ProjectModel.Data.Models.Position;
using ParticipantRole = PrismWorkApp.ProjectModel.Data.Models.ParticipantRole;
using RoleOfResponsible = PrismWorkApp.ProjectModel.Data.Models.RoleOfResponsible;*/

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public static class ProjectService
    {
        public const int ROW_LENGHT = 92;
        public static bldProject LoadProjectFromExcel()
        {
            bldConstructionCompanyGroup bld_Companies = new bldConstructionCompanyGroup();
            bldResponsibleEmployeesGroup bld_ResponsibleEmployees = new bldResponsibleEmployeesGroup();
            bldMaterialsGroup BldMaterials = new bldMaterialsGroup();
            bldLaboratoryReportsGroup bld_LaboratoryReports = new bldLaboratoryReportsGroup();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true)
                return null;
            string fileNames = openFileDialog.FileName;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            FileInfo info = new FileInfo(fileNames);
            bldProject bld_project = new bldProject();


            bld_project.Name = "Проект 1";
            bldObject bld_object = new bldObject();
            bld_object.Name = "Строительный объект";
            bld_project.BuildingObjects = new bldObjectsGroup(); //new  NameableObservableCollection<bldObject>();
            bld_project.BuildingObjects.Add(bld_object);
            bld_project.BuildingObjects.Name = "Строительные объекты";
            bld_object.Constructions = new bldConstructionsGroup();
            bldConstruction bld_construction = new bldConstruction();
            bld_construction.Name = "Конструкция";
            bld_construction.Constructions = new bldConstructionsGroup("КЖ"); ;
            bld_object.Constructions.Add(bld_construction);
            bld_project.ResponsibleEmployees.Name = "Список отвественных работников";
            bld_project.Participants.Name = "Учасники строительства";
            int work_id = 0;
            int participant_id = 0;
            using (ExcelPackage xlPackage = new ExcelPackage(info))
            {
                // get the first worksheet in the workbook
                var dsd = xlPackage.Workbook.Worksheets.Count;
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[0];
                ExcelWorksheet objectDataWorksheet = xlPackage.Workbook.Worksheets["Объект"];
                ExcelWorksheet companiesDataWorksheet = xlPackage.Workbook.Worksheets["Организации"];
                ExcelWorksheet responsibleEmplDataWorksheet = xlPackage.Workbook.Worksheets["Ответственные"];
                ExcelWorksheet AOSRDataWorksheet = xlPackage.Workbook.Worksheets["АОСР"];
                ExcelWorksheet VKActsDataWorksheet = xlPackage.Workbook.Worksheets["Акты ВК"];
                ExcelWorksheet LabReporstDataWorksheet = xlPackage.Workbook.Worksheets["Лаборатория"];

                #region создание объектой модели проекта с участиниками и отвественными
                int rowIndex = 2;

                bld_project.Name = objectDataWorksheet.Cells[2, 2].Value?.ToString();
                bld_project.ShortName = objectDataWorksheet.Cells[2, 4].Value?.ToString();
                bld_project.StartTime = DateTime.Parse(objectDataWorksheet.Cells[2, 3].Value?.ToString());
                bld_project.Address = objectDataWorksheet.Cells[2, 1].Value?.ToString();
                bld_project.FullName = bld_project.Name + ", " + bld_project.Address;

                while (companiesDataWorksheet.Cells[rowIndex, 2].Value?.ToString() != null) //По всем не пустым строком таблицы Организации
                {

                    bldConstructionCompany bld_company = new bldConstructionCompany();
                    bldParticipant bld_participant = new bldParticipant();

                    //  bld_participant.Id = participant_id++;
                    //bld_participant.Id = participant.Id;
                    switch (companiesDataWorksheet.Cells[rowIndex, 1].Value?.ToString())
                    {
                        case "Заказчик":
                            bld_participant.Role = (OpenWorkLib.Data.ParticipantRole)ParticipantRole.DEVELOPER; break;
                        case "Генподрядчик":
                            bld_participant.Role = (OpenWorkLib.Data.ParticipantRole)ParticipantRole.GENERAL_CONTRACTOR;
                            break;
                        case "Авторский надзор":
                            bld_participant.Role = (OpenWorkLib.Data.ParticipantRole)ParticipantRole.DISIGNER;
                            break;
                        case "Подрядчик":
                            bld_participant.Role = (OpenWorkLib.Data.ParticipantRole)ParticipantRole.BUILDER;
                            break;
                        default:
                            bld_participant.Role = (OpenWorkLib.Data.ParticipantRole)ParticipantRole.NONE;
                            break;
                    }
                    bld_company.Name = companiesDataWorksheet.Cells[rowIndex, 2].Value?.ToString();
                    bld_company.OGRN = companiesDataWorksheet.Cells[rowIndex, 3].Value?.ToString();
                    bld_company.INN = companiesDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    bld_company.Address = companiesDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    bld_company.Contacts = companiesDataWorksheet.Cells[rowIndex, 6].Value?.ToString();
                    bldCompany bld_SROIssuingCompany = new bldCompany();

                    bld_SROIssuingCompany.Name = companiesDataWorksheet.Cells[rowIndex, 7].Value?.ToString();
                    bld_SROIssuingCompany.OGRN = companiesDataWorksheet.Cells[rowIndex, 8].Value?.ToString();
                    bld_SROIssuingCompany.INN = companiesDataWorksheet.Cells[rowIndex, 9].Value?.ToString();

                    bld_company.SROIssuingCompany = bld_SROIssuingCompany;

                    if (bld_Companies.SingleOrDefault(c => c.INN == bld_company.INN) == null)
                        bld_Companies.Add(bld_company);
                    else
                        bld_company = bld_Companies.Where(cm => cm.INN == bld_company.INN).FirstOrDefault();
                    bld_participant.ConstructionCompanies.Add(bld_company);
                    bld_project.Participants.Add(bld_participant);
                    rowIndex++;
                }
                rowIndex = 2;

                while (responsibleEmplDataWorksheet.Cells[rowIndex, 2].Value?.ToString() != null) //По всем не пустым строком таблицы Ответсвенные
                {
                    bldResponsibleEmployee bld_employee = new bldResponsibleEmployee();
                    bld_employee.Code = (responsibleEmplDataWorksheet.Cells[rowIndex, 1].Value?.ToString()).ToString();
                    switch (responsibleEmplDataWorksheet.Cells[rowIndex, 2].Value?.ToString())
                    {
                        case "Застройщик (технический заказчик, эксплуатирующая организация или региональный оператор)":
                            bld_employee.RoleOfResponsible = (OpenWorkLib.Data.RoleOfResponsible)RoleOfResponsible.CUSTOMER;
                            // bld_project.Participants[(int)ParticipantRole.DEVELOPER].ConstructionCompanies[].ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, осуществляющее строительство":
                            bld_employee.RoleOfResponsible = (OpenWorkLib.Data.RoleOfResponsible)RoleOfResponsible.GENERAL_CONTRACTOR;
                            // bld_project.Participants[(int)ParticipantRole.GENERAL_CONTRACTOR].ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, осуществляющее строительство отвественное за строительный контроль":
                            bld_employee.RoleOfResponsible = (OpenWorkLib.Data.RoleOfResponsible)RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER;
                            // bld_project.Participants[(int)ParticipantRole.GENERAL_CONTRACTOR].ResponsibleEmployees.Add(bld_employee);
                            break;

                        case "Лицо, осуществляющее подготовку проектной документации":
                            bld_employee.RoleOfResponsible = (OpenWorkLib.Data.RoleOfResponsible)RoleOfResponsible.AUTHOR_SUPERVISION;
                            //  bld_project.Participants[(int)ParticipantRole.DISIGNER].ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, выполнившее работы, подлежащие освидетельствованию":
                            bld_employee.RoleOfResponsible = (OpenWorkLib.Data.RoleOfResponsible)RoleOfResponsible.WORK_PERFORMER;
                            // bld_project.Participants[(int)ParticipantRole.BUILDER].ResponsibleEmployees.Add(bld_employee);
                            break;
                        default:
                            bld_employee.RoleOfResponsible = (OpenWorkLib.Data.RoleOfResponsible)RoleOfResponsible.NONE;
                            break;

                    }

                    bld_employee.NRSId = responsibleEmplDataWorksheet.Cells[rowIndex, 3].Value?.ToString();
                    bld_employee.Position = new EmployeePosition(responsibleEmplDataWorksheet.Cells[rowIndex, 4].Value?.ToString());
                    bld_employee.FullName = responsibleEmplDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    bld_employee.Name = responsibleEmplDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    bld_employee.DocConfirmingTheAthority = new bldDocument(responsibleEmplDataWorksheet.Cells[rowIndex, 6].Value?.ToString());


                    string fdf = responsibleEmplDataWorksheet.Cells[rowIndex, 7].Value?.ToString();
                    var bld_company = bld_Companies.SingleOrDefault(c => c.Name == fdf);
                    if (bld_company != null)
                    {
                        bld_employee.Company = bld_company;
                        bld_company.ResponsibleEmployees.Add(bld_employee);
                    }
                    bld_ResponsibleEmployees.Add(bld_employee);
                    rowIndex++;
                }
                #endregion
                rowIndex = 3;
                bld_project.ResponsibleEmployees = bld_ResponsibleEmployees;
                bld_project.ResponsibleEmployees.Name = "Список отвественных работников";
                //  while (AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString() <!=null) //По всем не пустым строком таблицы Ответсвенные
                rowIndex = 2;
                while (VKActsDataWorksheet.Cells[rowIndex, 7].Value?.ToString() != null) //По всем не пустым строком таблицы Акты ВК загружаем материалы
                {
                    bldMaterial bld_material = new bldMaterial();

                    bld_material.Quantity = (decimal)Convert.ToDouble(VKActsDataWorksheet.Cells[rowIndex, 9].Value?.ToString()); ;
                    if (VKActsDataWorksheet.Cells[rowIndex, 10].Value?.ToString() != null)
                        bld_material.UnitOfMeasurement = new bldUnitOfMeasurement(VKActsDataWorksheet.Cells[rowIndex, 10].Value?.ToString());
                    bld_material.FullName = VKActsDataWorksheet.Cells[rowIndex, 20].Value?.ToString().Replace("_", " ");
                    bld_material.Name = VKActsDataWorksheet.Cells[rowIndex, 7].Value?.ToString();

                    if (VKActsDataWorksheet.Cells[rowIndex, 20].Value?.ToString() != null)
                    {
                        string text = VKActsDataWorksheet.Cells[rowIndex, 12].Value?.ToString(); //выбираем номера документов
                        string[] text_array;
                        if (text != null)
                            text_array = Regex.Split(text, @",");
                        else
                            text_array = new string[] { "отсутвует" };

                        foreach (string doc_nam in text_array)
                        {
                            bldMaterialCertificate bld_document = new bldMaterialCertificate();
                            bld_document.Name = "№" + doc_nam + " " + VKActsDataWorksheet.Cells[rowIndex, 11].Value?.ToString() + ": " + bld_material.Name;
                            bld_document.RegId = doc_nam;
                            bld_document.Date = Convert.ToDateTime(VKActsDataWorksheet.Cells[rowIndex, 13].Value?.ToString());
                            bld_document.FullName = bld_document.Name + " " + bld_document.RegId;
                            //  bld_document.PrintingName = document.FullName + " от " + document.Date.ToString("d") + ";";
                            bldDocumentsGroup bldGroup = new bldDocumentsGroup();
                            bld_material.Documents.Add(bld_document);
                        }

                    }
                    BldMaterials.Add(bld_material);
                    rowIndex++;
                }

                rowIndex = 2;
                while (LabReporstDataWorksheet.Cells[rowIndex, 3].Value?.ToString() != null) //По всем не пустым строком таблицы Лаборатория
                {
                    bldLaboratoryReport bld_report = new bldLaboratoryReport(LabReporstDataWorksheet.Cells[rowIndex, 7].Value?.ToString());
                    bld_report.RegId = LabReporstDataWorksheet.Cells[rowIndex, 1].Value?.ToString(); ;
                    bld_report.Date = Convert.ToDateTime(LabReporstDataWorksheet.Cells[rowIndex, 2].Value?.ToString());
                    bld_report.Name = LabReporstDataWorksheet.Cells[rowIndex, 3].Value?.ToString() + " " + LabReporstDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    bld_report.FullName = LabReporstDataWorksheet.Cells[rowIndex, 7].Value?.ToString().Replace("_", " ");
                    bld_LaboratoryReports.Add(bld_report);
                    rowIndex++;
                }

                rowIndex = 2;
                while (AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString() != null) //По всем не пустым строком таблицы АОСР создаем работы
                {
                    bldWork bld_work = new bldWork();
                    bldAOSRDocument bld_AOSR = new bldAOSRDocument("Акт освидетельствования скрытых работ");
                    bld_AOSR.RegId = AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    for (int ii = 5; ii <= 9; ii++) //Определяем отвественных для данной работы 
                    {
                        var bld_code = bld_project.ResponsibleEmployees
                                .FirstOrDefault(emp => emp.Code == AOSRDataWorksheet.Cells[rowIndex, ii].Value?.ToString().ToString());
                        bld_AOSR.ResponsibleEmployees.Add(bld_code);
                    }
                    bld_work.Name = AOSRDataWorksheet.Cells[rowIndex, 11].Value?.ToString();
                    bld_work.Code = AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    bld_work.WorkArea = new bldWorkArea();
                    bld_work.WorkArea.Levels = AOSRDataWorksheet.Cells[rowIndex, 12].Value?.ToString();
                    bld_work.WorkArea.Axes = AOSRDataWorksheet.Cells[rowIndex, 13].Value?.ToString();
                    bld_work.Quantity = (decimal)Convert.ToDouble(AOSRDataWorksheet.Cells[rowIndex, 14].Value?.ToString()); ;
                    if (AOSRDataWorksheet.Cells[rowIndex, 15].Value?.ToString() != null)
                        bld_work.UnitOfMeasurement = new bldUnitOfMeasurement(AOSRDataWorksheet.Cells[rowIndex, 15].Value?.ToString());
                    bldProjectDocument project_documentacion = new bldProjectDocument();
                    project_documentacion.Name = AOSRDataWorksheet.Cells[rowIndex, 17].Value?.ToString();
                    bld_work.ProjectDocuments.Add(project_documentacion);

                    string text = AOSRDataWorksheet.Cells[rowIndex, 18].Value?.ToString();
                    if (text != null)
                    {
                        string[] text_array = Regex.Split(text, @"; ");
                        foreach (string str in text_array)
                        {
                            var bld_material = BldMaterials.FirstOrDefault(m => m.FullName == str.Replace("_", " "));
                            if (bld_material != null)
                                bld_work.Materials.Add(bld_material);
                        }
                    }
                    text = AOSRDataWorksheet.Cells[rowIndex, 19].Value?.ToString();
                    if (text != null)
                    {
                        string[] text_array = Regex.Split(text, @"; ");
                        foreach (string str in text_array)
                        {
                            var bld_report = bld_LaboratoryReports.FirstOrDefault(rp => rp.FullName == str.Replace("_", " "));
                            if (bld_report != null)
                                bld_work.LaboratoryReports.Add(bld_report); //Временно. Материал -  просто список в имя одного материала

                        }
                    }
                    bld_AOSR.Date = Convert.ToDateTime(AOSRDataWorksheet.Cells[rowIndex, 25].Value?.ToString());
                    bldExecutiveScheme bld_executiveScheme = new bldExecutiveScheme();
                    string str_scheme_num = (string)AOSRDataWorksheet.Cells[rowIndex, 20].Value?.ToString();
                    str_scheme_num = str_scheme_num?.Replace("№", "").Replace(" ", "");
                    if (str_scheme_num != null && str_scheme_num != "")
                    {
                        bld_executiveScheme.RegId = str_scheme_num.ToString();
                        bld_executiveScheme.Date = bld_AOSR.Date;
                        if (bld_work.WorkArea.Axes != null)
                        {
                            bld_executiveScheme.Name = "Исполнительная схема. " + bld_work.Name
                                                  + " " + bld_work?.WorkArea.Levels + " в осях " + bld_work.WorkArea.Axes;
                            bld_executiveScheme.FullName = bld_executiveScheme.Name + " №" + bld_executiveScheme.RegId.ToString();
                        }
                        else
                        {
                            bld_executiveScheme.Name = "Исполнительная схема. " + bld_work.Name
                                              + " " + bld_work?.WorkArea.Levels;
                            bld_executiveScheme.FullName = bld_executiveScheme.Name + " №" + bld_executiveScheme.RegId.ToString();
                        }

                        bld_work.ExecutiveSchemes.Add(bld_executiveScheme); //Временно. Исполнительные схемы -  просто список в имя одной схмемы
                    }
                    bldRegulationtDocument reg_docs = new bldRegulationtDocument();
                    reg_docs.Name = AOSRDataWorksheet.Cells[rowIndex, 23].Value?.ToString();
                    bld_work.RegulationDocuments.Add(reg_docs);
                    bld_work.AOSRDocuments.Add(bld_AOSR);
                    bld_AOSR.bldWork = bld_work;

                    bld_work.StartTime = Convert.ToDateTime(AOSRDataWorksheet.Cells[rowIndex, 26].Value?.ToString());
                    bld_work.EndTime = Convert.ToDateTime(AOSRDataWorksheet.Cells[rowIndex, 27].Value?.ToString());
                    bld_construction.Works.Add(bld_work);
                    rowIndex++;
                }

                foreach (bldWork work in bld_construction.Works)
                {
                    bldWork next_work = null;
                    rowIndex = 2;
                    while (AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString() != null) //По всем не пустым строком таблицы АОСР создаем работы
                    {
                        if (work.Code == AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString())
                        {
                            next_work = bld_construction.Works.
                                 FirstOrDefault(w => w.Code == AOSRDataWorksheet.Cells[rowIndex, 16].Value?.ToString());
                            if (next_work != null) next_work.PreviousWorks.Add(work);
                            if (next_work != null) work.NextWorks.Add(next_work);
                            break;
                        }
                        rowIndex++;
                    }

                    foreach (bldDocument document in work.ExecutiveSchemes)
                        work.AOSRDocuments[0]?.AttachedDocuments.Add(document);
                    foreach (bldDocument document in work.LaboratoryReports)
                        work.AOSRDocuments[0]?.AttachedDocuments.Add(document);

                    foreach (bldMaterial material in work.Materials)
                        foreach (bldDocument document in material.Documents)
                            work.AOSRDocuments[0]?.AttachedDocuments.Add(document);
                   
                    work.AOSRDocuments[0].StartTime = work.StartTime;
                    work.AOSRDocuments[0].EndTime = work.EndTime;

                }
            }

            return bld_project;
        }
        public static void SaveAOSRToWord(bldAOSRDocument aOSRDocument)
        {
            string templates_path = Directory.GetCurrentDirectory() + "\\Шаблоны";

            Microsoft.Office.Interop.Word._Application world_application;
            Microsoft.Office.Interop.Word._Document world_document;
            Microsoft.Office.Interop.Word._Document world_doc_appendix;
            Microsoft.Office.Interop.Word._Document world_attached_doc;
            int i_doc_att_num = 1;
            int i_table2_row_add_counter = 0;
            world_application = new Microsoft.Office.Interop.Word.Application();
            world_application.Visible = true;
            //   using (Microsoft.Office.Interop.Word._Application world_application = new Microsoft.Office.Interop.Word._Application())
            {

                world_document = world_application.Documents.Add(templates_path + "\\АОСР.docx");


                //   for (int work_index = selection_row_from; work_index <= selection_row_from; work_index++)
                {
                    //Work current_work = project.Works[work_index];
                    bldWork current_work = aOSRDocument.bldWork;
                    bldProject project = current_work.bldConstruction.bldObject.bldProject;
                    //  AOSRDocument aOSRDocument = current_work.AOSRDocuments
                    //  foreach (AOSRDocument aOSRDocument in current_work.AOSRDocuments)
                    {
                        //  aOSRDocument.AttachDocuments.Clear();
                        world_document.Bookmarks["Number"].Range.Text = aOSRDocument.RegId; //Номер акта
                        string s_date = aOSRDocument.Date.ToString("d");
                        world_document.Bookmarks["Date_Sign"].Range.Text =
                                ((DateTime)aOSRDocument.Date).ToString("d");// Дата акта

                        world_document.Bookmarks["Object_name"].Range.Text = project.FullName;//Наименованиа объекта

                        string Client_name = project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER)?.ConstructionCompanies[0]?.FullName + //Застройщик
                            ", " + project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER)?.ConstructionCompanies[0]?.Contacts +
                            ", " + project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER)?.ConstructionCompanies[0]?.SROIssuingCompany.FullName;
                        world_document.Bookmarks["Client_name"].Range.Text = Client_name;

                        string GCC_name = project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompanies[0]?.FullName +
                            ", " + project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompanies[0]?.Contacts +
                            ", " + project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompanies[0]?.SROIssuingCompany.FullName;
                        world_document.Bookmarks["GCC_name"].Range.Text = GCC_name;//Ген подрядчик

                        string Author_name = project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER)?.ConstructionCompanies[0]?.FullName +
                            ", " + project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER)?.ConstructionCompanies[0]?.Contacts +
                            ", " + project.Participants?.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER)?.ConstructionCompanies[0]?.SROIssuingCompany.FullName;
                        world_document.Bookmarks["Author_name"].Range.Text = Author_name;//Проективрощики

                        string Client_Signer = project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).Position.Name +
                            " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName +
                            " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).DocConfirmingTheAthority.Name +
                            " " + project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER)?.ConstructionCompanies[0].FullName;
                        world_document.Bookmarks["Client_Signer"].Range.Text = Client_Signer;//Предстваитель затройщика


                        string GCC1_Signer = project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR)?.Position?.Name +
                        " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR)?.FullName +
                        " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR)?.DocConfirmingTheAthority?.Name
                        ;//+ " " + project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.FullName;
                        world_document.Bookmarks["GCC1_Signer"].Range.Text = GCC1_Signer;//Предстваитель лица осуществлящего строительва ( ген подрядчк)

                        string GCC2_Signer = project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).Position.Name +
                           " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName +
                           " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).DocConfirmingTheAthority.Name
                           ;// +" " + project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.FullName;
                        world_document.Bookmarks["GCC2_Signer"].Range.Text = GCC2_Signer;//Предстваитель лица осуществлящего строительва ( ген подрядчк) по строй котролю


                        string Author_Signer = project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).Position.Name +
                       " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName +
                       " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).DocConfirmingTheAthority.Name +
                       " " + project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).ConstructionCompanies[0].FullName;
                        world_document.Bookmarks["Author_Signer"].Range.Text = Author_Signer;//Предстваитель австорского надзора
                        string SubC_Signer = project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).Position.Name +
                        " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName +
                        " " + project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).DocConfirmingTheAthority.Name +
                        " " + project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.BUILDER).ConstructionCompanies[0].FullName;
                        world_document.Bookmarks["SubC_Signer"].Range.Text = SubC_Signer;//Предстваитель лица  непосредственно выполняющего работы

                        string SubC_name1 = project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).ConstructionCompanies[0].Name;
                        world_document.Bookmarks["SubC_name1"].Range.Text = SubC_name1; // Иные лизац
                        string str = "";


                        if (current_work.WorkArea.Axes != null)
                            str = current_work.Name//Нименование работы к освидетелтьсвтованию
                            + " " + current_work?.WorkArea.Levels + " в осях " + current_work.WorkArea.Axes + " " + project.ShortName+". ";
                        else
                            str = current_work.Name//Нименование работы к освидетелтьсвтованию
                         + " " + current_work?.WorkArea.Levels + " " + project.ShortName + ". "; 

                        List<string> str_arr = new List<string>();
                        str_arr = DivideOnSubstring(str, ROW_LENGHT);
                        world_document.Tables[2].Rows[3].Range.Text = str_arr[str_arr.Count - 1];
                        for (int ii = 2; str_arr.Count - ii >= 0; ii++)
                        {
                            world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[3]);
                            world_document.Tables[2].Rows[3].Range.Text = str_arr[str_arr.Count - ii];
                            i_table2_row_add_counter++;
                        }

                        world_document.Bookmarks["project"].Range.Text = current_work.ProjectDocuments[0]?.Name //Проектная документация
                          + ", " + project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).ConstructionCompanies[0]?.Name;
                        #region Вывод данных по используемым материалам
                        str = "";
                        foreach (bldMaterial material in current_work.Materials)
                            str += material.FullName + ". ";

                        str_arr = DivideOnSubstring(str, ROW_LENGHT);
                        bldDocumentsGroup docs_for_word_attach = new bldDocumentsGroup();
                        if (str_arr.Count <= 5)
                        {
                            foreach (bldMaterial material in current_work.Materials)
                                foreach (bldDocument doc in material.Documents)
                                    docs_for_word_attach.Add(doc);

                            world_document.Tables[2].Rows[i_table2_row_add_counter + 9].Range.Text = str_arr[str_arr.Count - 1];
                            for (int ii = 0; ii < str_arr.Count - 1; ii++)
                            {
                                world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[i_table2_row_add_counter + 9 - ii]);
                                world_document.Tables[2].Rows[i_table2_row_add_counter + 9 - ii].Range.Text = str_arr[str_arr.Count - 2 - ii];
                                i_table2_row_add_counter++;
                            }

                        }
                        else
                        {
                            world_document.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                            world_attached_doc = world_application.Documents.Add(templates_path + "\\Приложения.docx");
                            Microsoft.Office.Interop.Word._Document world_attached_doc_table = world_application.Documents.Add(templates_path + "\\Таблица к Приложениям.docx");
                            bldDocument attach_doc = new bldDocument();
                            attach_doc.Name = "Реестр строительных материалов (конструкций) ";//+aOSRDocument.FullName;
                            attach_doc.Date = aOSRDocument.Date;
                            attach_doc.PagesNumber = Convert.ToInt32(world_attached_doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages));
                            attach_doc.RegId = i_doc_att_num.ToString();
                            docs_for_word_attach.Add(attach_doc);

                            DateTime material_last_date = current_work.Materials.OrderBy(m => m.Date).FirstOrDefault().Date;
                            if (material_last_date < aOSRDocument.Date) material_last_date = aOSRDocument.Date;
                            Microsoft.Office.Interop.Word.Table attached_table = world_attached_doc_table.Tables[1];
                            attach_doc.FullName = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + material_last_date.ToString("d")
                               + " на " + (Math.Ceiling((double)attach_doc.PagesNumber / 2)).ToString() + " листе (ах). ";

                            attached_table.Cell(1, 1).Range.Text = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + material_last_date.ToString("d") + ".";

                            int material_number = 1;
                            //   for (int ii = 0; ii < current_work.Materials.Count - 1; ii++)
                            //   attached_table.Rows.Add(attached_table.Rows[4]);
                            int row_index = 3;
                            int start_row = 4;
                            int end_row = 4;
                            int row_i = 0;
                            Dictionary<int, int> MergeRanges = new Dictionary<int, int>();
                            foreach (bldMaterial material in current_work.Materials)
                            {
                                row_index++;
                                for (int ii = 0; ii < material.Documents.Count; ii++)
                                    attached_table.Rows.Add(attached_table.Rows[row_index]);

                                start_row = row_index;
                                row_i = -1;                                                         // bldDocumentsGroup one_date_documents = new bldDocumentsGroup();
                                foreach (bldDocument document in material.Documents)
                                {
                                    row_i++;
                                    attached_table.Cell(row_index + row_i, 3).Range.Text = document.RegId;
                                    attached_table.Cell(row_index + row_i, 4).Range.Text = document.Date.ToString("d");
                                    //      attached_table.Cell(row_index + row_i, 1).Range.Text  = (row_index + row_i).ToString();

                                }
                                row_index += row_i;
                                attached_table.Cell(start_row, 1).Range.Text = material_number.ToString(); ;// 
                                attached_table.Cell(start_row, 2).Range.Text = material.Name;// + material.Documents[0].Name;

                                end_row = row_index;
                                MergeRanges.Add(start_row, end_row);

                                material_number++;

                            }
                            attached_table.Rows[row_index + 1].Delete();
                            foreach (var merg_pare in MergeRanges)
                                if (merg_pare.Key != merg_pare.Value)
                                {
                                    attached_table.Cell(merg_pare.Key, 2).Merge(attached_table.Cell(merg_pare.Value, 2));
                                    attached_table.Cell(merg_pare.Key, 1).Merge(attached_table.Cell(merg_pare.Value, 1));

                                }
                            world_attached_doc.Bookmarks["Parent_doc"].Range.Text = "Приложение №"
                                + (docs_for_word_attach.Count).ToString()
                                + " к АОСР №" + aOSRDocument.RegId
                                + " от " + ((DateTime)(aOSRDocument.Date)).ToString("d");
                            world_attached_doc.Bookmarks["Client_Signer2"].Range.Text =
                                 project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName;
                            world_attached_doc.Bookmarks["GCC1_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName;
                            world_attached_doc.Bookmarks["GCC2_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName;
                            world_attached_doc.Bookmarks["Author_Signer2"].Range.Text =
                               project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName;
                            world_attached_doc.Bookmarks["SubC_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName;


                            attached_table.Select();
                            world_application.Selection.Copy();
                            world_attached_doc.Tables[1].Cell(2, 1).Range.PasteSpecial();
                            world_attached_doc.Tables[1].Select();
                            world_application.Selection.Copy();
                            world_document.Words.Last.PasteSpecial();
                            world_document.Bookmarks["Materials"].Range.Text = attach_doc.FullName;
                            world_application.Documents[world_attached_doc].Close(false);
                            world_application.Documents[world_attached_doc_table].Close(false);
                            // world_attached_doc.Close();
                        }

                        #endregion
                        #region Вывод данных по  п4.АОСР
                        str_arr.Clear();
                        str = "";
                        foreach (bldDocument scheme in current_work.ExecutiveSchemes)
                        {
                            str += scheme.FullName + ". ";
                            //  aOSRDocument.AttachDocuments.Add(scheme);
                        }
                        foreach (bldDocument report in current_work.LaboratoryReports)
                        {
                            str += report.FullName + ". ";
                            //      aOSRDocument.AttachDocuments.Add(report);
                        }
                        #endregion
                        str_arr = DivideOnSubstring(str, ROW_LENGHT);
                        if (str_arr.Count <= 5)
                        {
                            foreach (bldDocument scheme in current_work.ExecutiveSchemes)
                                docs_for_word_attach.Add(scheme);
                            foreach (bldDocument report in current_work.LaboratoryReports)
                                docs_for_word_attach.Add(report);
                            int loc_table_dimention = 0;
                            for (int ii = 2; str_arr.Count - ii >= 0; ii++)
                            {
                                world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[12 + i_table2_row_add_counter]);
                                i_table2_row_add_counter++;
                                loc_table_dimention++;
                            }
                            for (int ii = loc_table_dimention; ii >= 0; ii--)
                                world_document.Tables[2].Rows[12 + i_table2_row_add_counter - ii].Range.Text = str_arr[loc_table_dimention - ii];


                            /*      world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - 1];
                          for (int ii = 2; str_arr.Count - ii >= 0; ii++)//Заполняем поле документы подтверждающие качество выпоненных работ
                            {
                                world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[12 + i_table2_row_add_counter]);
                                world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - ii];
                                i_table2_row_add_counter++;
                            }*/
                        }
                        else
                        {
                            world_document.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                            world_attached_doc = world_application.Documents.Add(templates_path + "\\Приложения.docx");
                            Microsoft.Office.Interop.Word._Document world_attached_doc_table = world_application.Documents.Add(templates_path + "\\Таблица к Приложениям.docx");
                            bldDocument attach_doc = new bldDocument();
                            attach_doc.Name = "Реестр документов, подтверждающих соответствие работ предъявляемым к ним требованиям ";
                            attach_doc.Date = aOSRDocument.Date;
                            attach_doc.PagesNumber = Convert.ToInt32(world_attached_doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages));
                            attach_doc.RegId = i_doc_att_num.ToString();
                            docs_for_word_attach.Add(attach_doc);

                            DateTime attach_last_date = current_work.LaboratoryReports.OrderBy(r => r.Date).FirstOrDefault().Date;
                            Microsoft.Office.Interop.Word.Table attached_table = world_attached_doc_table.Tables[1];
                            attach_doc.FullName = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + attach_last_date.ToString("d")
                               + " на " + (Math.Ceiling((double)attach_doc.PagesNumber / 2)).ToString() + " листе (ах). ";

                            attached_table.Cell(1, 1).Range.Text = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + attach_last_date.ToString("d") + ".";

                            int _number = 1;
                            for (int ii = 0; ii < current_work.ExecutiveSchemes.Count - 1 + current_work.LaboratoryReports.Count; ii++)
                                attached_table.Rows.Add(attached_table.Rows[4]);
                            int row_index = 4;
                            foreach (bldDocument scheme in current_work.ExecutiveSchemes)
                            {
                                attached_table.Cell(row_index, 1).Range.Text = _number.ToString();// 
                                attached_table.Cell(row_index, 2).Range.Text = scheme.Name;
                                attached_table.Cell(row_index, 3).Range.Text = scheme.RegId; ;
                                attached_table.Cell(row_index, 4).Range.Text = scheme.Date.ToString("d");
                                _number++;
                                row_index++;
                            }
                            foreach (bldDocument report in current_work.LaboratoryReports)
                            {
                                attached_table.Cell(row_index, 1).Range.Text = _number.ToString(); ;// 
                                attached_table.Cell(row_index, 2).Range.Text = report.Name;
                                attached_table.Cell(row_index, 3).Range.Text = report.RegId; ;
                                attached_table.Cell(row_index, 4).Range.Text = report.Date.ToString("d");
                                _number++;
                                row_index++;
                            }


                            world_attached_doc.Bookmarks["Parent_doc"].Range.Text = "Приложение №"
                                + (docs_for_word_attach.Count).ToString()
                                + " к АОСР №" + aOSRDocument.RegId
                                + " от " + ((DateTime)(aOSRDocument.Date)).ToString("d");
                            world_attached_doc.Bookmarks["Client_Signer2"].Range.Text =
                                 project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName;
                            world_attached_doc.Bookmarks["GCC1_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName;
                            world_attached_doc.Bookmarks["GCC2_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName;
                            world_attached_doc.Bookmarks["Author_Signer2"].Range.Text =
                               project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName;
                            world_attached_doc.Bookmarks["SubC_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName;

                            attached_table.Select();
                            world_application.Selection.Copy();
                            world_attached_doc.Tables[1].Cell(2, 1).Range.PasteSpecial();
                            world_attached_doc.Tables[1].Select();
                            world_application.Selection.Copy();
                            world_document.Words.Last.PasteSpecial();

                            // world_document.Bookmarks["Documents"].Range.Text = attach_doc.PrintingName;
                            str_arr.Clear();
                            str = "";
                            str_arr = DivideOnSubstring(attach_doc.FullName, ROW_LENGHT);
                            world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - 1];
                            for (int ii = 2; str_arr.Count - ii >= 0; ii++)//Заполняем поле документы подтверждающие качество выпоненных работ
                            {
                                world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[12 + i_table2_row_add_counter]);
                                world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - ii];
                                i_table2_row_add_counter++;
                            }

                            world_application.Documents[world_attached_doc].Close(false);
                            world_application.Documents[world_attached_doc_table].Close(false);
                        }
                        str = "";
                        str_arr.Clear();
                        int attach_doc_counter = 1;
                        foreach (bldDocument doc in docs_for_word_attach)
                        {
                            str += attach_doc_counter++.ToString() + ")" + doc.FullName + " ";
                        }

                        str_arr = DivideOnSubstring(str, ROW_LENGHT); ;
                        // world_document.Tables[2].Rows[29 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - 1];
                        for (int ii = 0; ii < str_arr.Count; ii++) //Заполняем пункт "Приложения"
                        {
                            world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[29 + i_table2_row_add_counter]);
                            world_document.Tables[2].Rows[29 + i_table2_row_add_counter].Range.Text = str_arr[ii];
                            i_table2_row_add_counter++;
                        }

                        foreach (bldDocument document in current_work.RegulationDocuments)
                            world_document.Bookmarks["SNiP"].Range.Text += document.Name + ". ";
                        foreach (bldWork work in current_work.NextWorks)
                            world_document.Bookmarks["Next_Work"].Range.Text += work.Name + ". ";
                        world_document.Bookmarks["Client_Signer2"].Range.Text =
                            project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName;
                        world_document.Bookmarks["GCC1_Signer2"].Range.Text =
                            project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName;
                        world_document.Bookmarks["GCC2_Signer2"].Range.Text =
                            project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName;
                        world_document.Bookmarks["Author_Signer2"].Range.Text =
                           project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName;
                        world_document.Bookmarks["SubC_Signer2"].Range.Text =
                         project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName;

                        //world_document.Bookmarks["Date_Begin"].Range.Text = current_work.StartTime.ToString("d");
                        // world_document.Bookmarks["Date_End"].Range.Text = current_work.EndTime.ToString("d
                         world_document.Bookmarks["Date_Begin"].Range.Text = current_work.AOSRDocuments[0].StartTime.ToString("d");
                         world_document.Bookmarks["Date_End"].Range.Text = current_work.AOSRDocuments[0].EndTime.ToString("d");

                    }
                }
                world_application.Visible = true;
            }
        }
        public static void SaveAOSRToWord(bldWork work)
        {
            foreach (bldAOSRDocument aOSRDocument in work.AOSRDocuments)
            {
                SaveAOSRToWord(aOSRDocument);
            }


        }

        public static List<string> DivideOnSubstring(string s, int chunkSize)
        {
            if (s.Length < chunkSize)
            {
                List<string> ans = new List<string>();
                ans.Add(s);
                return ans;
            }
            var result = (from Match m in Regex.Matches(s, @".{1," + chunkSize + "}")
                          select m.Value).ToList();


            return result;
        }
    }
}
