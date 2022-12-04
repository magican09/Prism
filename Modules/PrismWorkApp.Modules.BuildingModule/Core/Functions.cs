using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using OfficeOpenXml;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
/*using AOSRDocument = PrismWorkApp.ProjectModel.Data.Models.AOSRDocument;
using Document = PrismWorkApp.ProjectModel.Data.Models.Document;
using Position = PrismWorkApp.ProjectModel.Data.Models.Position;
using ParticipantRole = PrismWorkApp.ProjectModel.Data.Models.ParticipantRole;
using RoleOfResponsible = PrismWorkApp.ProjectModel.Data.Models.RoleOfResponsible;*/

namespace PrismWorkApp.Modules.BuildingModule.Core
{
    public static class Functions
    {
        public const int ROW_LENGHT = 92;
        public static bldProject LoadProjectFromExcel()
        {
            bldConstructionCompanyGroup bld_Companies = new bldConstructionCompanyGroup();
            bldResponsibleEmployeesGroup bld_ResponsibleEmployees = new bldResponsibleEmployeesGroup();
            bldMaterialsGroup BldMaterials = new bldMaterialsGroup();
            bldLaboratoryReportsGroup bld_LaboratoryReports = new bldLaboratoryReportsGroup();
            bldParticipantsGroup bld_Participants = new bldParticipantsGroup();

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
                            bld_participant.Role = new bldParticipantRole(ParticipantRole.DEVELOPER);
                            bld_participant.Role.FullName = "Застройщик(технический заказчик, эксплуатирующая организация или региональный оператор)";
                            bld_participant.Role.Name = "Заказчик";
                            break;
                        case "Генподрядчик":
                            bld_participant.Role = new bldParticipantRole(ParticipantRole.GENERAL_CONTRACTOR);
                            bld_participant.Role.FullName = "Генеральный подрядчик(лицо, осуществляющее строительство)";
                            bld_participant.Role.Name = "Генподрядчик";
                            break;
                        case "Авторский надзор":
                            bld_participant.Role = new bldParticipantRole(ParticipantRole.DISIGNER);
                            bld_participant.Role.FullName = "Проектировщик (Лицо, осуществляющее подготовку проектной документации";
                            bld_participant.Role.Name = "Авторский надзор";
                            break;
                        case "Подрядчик":
                            bld_participant.Role = new bldParticipantRole(ParticipantRole.BUILDER);
                            bld_participant.Role.FullName = "Подрядчик(лицо, выполнившеее работы)";
                            bld_participant.Role.Name = "Подрядчик";
                            break;
                        default:
                            bld_participant.Role = new bldParticipantRole(ParticipantRole.NONE);
                            bld_participant.Role.FullName = "Не определено";
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
                    bld_participant.ConstructionCompany = (bld_company);
                    bld_project.Participants.Add(bld_participant);
                    bld_Participants.Add(bld_participant);
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
                            bld_employee.Role = new bldResponsibleEmployeeRole(RoleOfResponsible.CUSTOMER);
                            bld_employee.Role.FullName = "Застройщик (технический заказчик, эксплуатирующая организация или региональный оператор)";
                            bld_project.Participants.Where(pr => pr.Role.RoleCode == ParticipantRole.DEVELOPER).FirstOrDefault()?.ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, осуществляющее строительство":
                            bld_employee.Role = new bldResponsibleEmployeeRole(RoleOfResponsible.GENERAL_CONTRACTOR);
                            bld_employee.Role.FullName = "Лицо, осуществляющее строительство";
                            bld_project.Participants.Where(pr => pr.Role.RoleCode == ParticipantRole.GENERAL_CONTRACTOR).FirstOrDefault()?.ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, осуществляющее строительство отвественное за строительный контроль":
                            bld_employee.Role = new bldResponsibleEmployeeRole(RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER);
                            bld_employee.Role.FullName = "Лицо, осуществляющее строительство отвественное за строительный контроль";
                            bld_project.Participants.Where(pr => pr.Role.RoleCode == ParticipantRole.GENERAL_CONTRACTOR).FirstOrDefault()?.ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, осуществляющее подготовку проектной документации":
                            bld_employee.Role = new bldResponsibleEmployeeRole(RoleOfResponsible.AUTHOR_SUPERVISION);
                            bld_employee.Role.FullName = "Лицо, осуществляющее подготовку проектной документации";
                            bld_project.Participants[(int)ParticipantRole.DISIGNER].ResponsibleEmployees.Add(bld_employee);
                            break;
                        case "Лицо, выполнившее работы, подлежащие освидетельствованию":
                            bld_employee.Role = new bldResponsibleEmployeeRole(RoleOfResponsible.WORK_PERFORMER);
                            bld_employee.Role.FullName = "Лицо, выполнившее работы, подлежащие освидетельствованию";
                            bld_project.Participants.Where(pr => pr.Role.RoleCode == ParticipantRole.BUILDER).FirstOrDefault()?.ResponsibleEmployees.Add(bld_employee);
                            break;
                        default:
                            bld_employee.Role = new bldResponsibleEmployeeRole(RoleOfResponsible.NONE);
                            break;

                    }

                    bld_employee.NRSId = responsibleEmplDataWorksheet.Cells[rowIndex, 3].Value?.ToString();
                    Employee employee = new Employee();
                    employee.Position = new EmployeePosition(responsibleEmplDataWorksheet.Cells[rowIndex, 4].Value?.ToString());
                    employee.FullName = responsibleEmplDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    employee.Name = responsibleEmplDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    bld_employee.Employee = employee;
                    bld_employee.DocConfirmingTheAthority = new bldDocument(responsibleEmplDataWorksheet.Cells[rowIndex, 6].Value?.ToString());


                    string fdf = responsibleEmplDataWorksheet.Cells[rowIndex, 7].Value?.ToString();
                    var bld_company = bld_Companies.SingleOrDefault(c => c.Name == fdf);
                    if (bld_company != null)
                    {
                        bld_employee.Employee.Company = bld_company;
                        //bld_company.ResponsibleEmployees.Add(bld_employee);


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
        public static string GetFolderPath()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            //  dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            return null;
        }

    }
}
