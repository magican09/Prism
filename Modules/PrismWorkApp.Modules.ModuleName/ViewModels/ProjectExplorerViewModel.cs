using Prism.Commands;
using Prism.Mvvm;
using PrismWorkApp.Documents.Data;
using PrismWorkApp.ProjectModel.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Data;
using System.ComponentModel;
using OfficeOpenXml;
using System.Text.RegularExpressions;


namespace PrismWorkApp.Modules.ModuleName.ViewModels
{
    public class ProjectExplorerViewModel : BindableBase
    {
        private string _title = "Менеджер проектов";
        public Project _project; 
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private int _formAOSR;
        public int FormAOSR
        {
            get { return _formAOSR; }
            set { SetProperty(ref _formAOSR, value); }
        }
        private int _toAOSR;
        public int ToAOSR
        {
            get { return _toAOSR; }
            set { SetProperty(ref _toAOSR, value); }
        }
        public const int ROW_LENGHT = 80;
        public ObservableCollection<ConstructionCompany> Companies { get; set; }
        public ObservableCollection<ResponsibleEmployee> ResponsibleEmployees { get; set; }
        public ObservableCollection<Material> Materials { get; set; }
        public ObservableCollection<Document> LaboratoryReports { get; set; }
        public Project Project 
        {
            get { return _project; }
            set 
            {
                _project = value;
                LoadAllProjectCommand.RaiseCanExecuteChanged();
            } 
        }
        public ProjectExplorerViewModel()
        {
            LoadAllProjectCommand = new DelegateCommand(LoadAllProjects, CanLoadAllProjects);
            CreateAOSRCommand = new DelegateCommand(CreateAOSR, CanCreateAOSR);
        }
        private bool CanLoadAllProjects()
        {
            return true;
        }
        public DelegateCommand LoadAllProjectCommand { get; private set; }
        public DelegateCommand CreateAOSRCommand { get; private set; }
        #region  Реализация загрузки данных их Excel
        private void LoadAllProjects()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true)
                return;
            string fileNames = openFileDialog.FileName;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            FileInfo info = new FileInfo(fileNames);

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
                Companies = new ObservableCollection<ConstructionCompany>();
                Project = new Project();
                int rowIndex = 2;
                Project.Address = objectDataWorksheet.Cells[2, 1].Value?.ToString();
                Project.Name = objectDataWorksheet.Cells[2, 2].Value?.ToString();
                Project.ShortName = objectDataWorksheet.Cells[2, 4].Value?.ToString();
                Project.StartDate = DateTime.Parse(objectDataWorksheet.Cells[2, 3].Value?.ToString());
                while (companiesDataWorksheet.Cells[rowIndex, 2].Value?.ToString() != null) //По всем не пустым строком таблицы Организации
                {
                    ConstructionCompany company = new ConstructionCompany();
                    Participant participant = new Participant();
                    switch (companiesDataWorksheet.Cells[rowIndex, 1].Value?.ToString())
                    {
                        case "Заказчик":
                            participant.Role = ParticipantRole.DEVELOPER; break;
                        case "Генподрядчик":
                            participant.Role = ParticipantRole.GENERAL_CONTRACTOR; break;
                        case "Авторский надзор":
                            participant.Role = ParticipantRole.DISIGNER; break;
                        case "Подрядчик":
                            participant.Role = ParticipantRole.BUILDER; break;

                        default: participant.Role = ParticipantRole.NONE; break;
                    }
                    company.Name = companiesDataWorksheet.Cells[rowIndex, 2].Value?.ToString();
                    company.OGRN = companiesDataWorksheet.Cells[rowIndex, 3].Value?.ToString();
                    company.INN = companiesDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    company.Address = companiesDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    company.Contacts = companiesDataWorksheet.Cells[rowIndex, 6].Value?.ToString();
                    Company SROIssuingCompany = new Company();
                    SROIssuingCompany.Name = companiesDataWorksheet.Cells[rowIndex, 7].Value?.ToString();
                    SROIssuingCompany.OGRN = companiesDataWorksheet.Cells[rowIndex, 8].Value?.ToString();
                    SROIssuingCompany.INN = companiesDataWorksheet.Cells[rowIndex, 9].Value?.ToString();
                    company.SROIssuingCompany = SROIssuingCompany;
                    if (Companies.SingleOrDefault(c => c.INN == company.INN) == null)
                        Companies.Add(company);
                    participant.Company = company;
                    Project.Participants.Add(participant);
                    rowIndex++;
                }
                rowIndex = 2;
                ResponsibleEmployees = new ObservableCollection<ResponsibleEmployee>();
                while (responsibleEmplDataWorksheet.Cells[rowIndex, 2].Value?.ToString() != null) //По всем не пустым строком таблицы Ответсвенные
                {
                    ResponsibleEmployee employee = new ResponsibleEmployee();
                    employee.Code = (responsibleEmplDataWorksheet.Cells[rowIndex, 1].Value?.ToString()).ToString();
                    switch (responsibleEmplDataWorksheet.Cells[rowIndex, 2].Value?.ToString())
                    {
                        case "Застройщик (технический заказчик, эксплуатирующая организация или региональный оператор)":
                            employee.RoleOfResponsible = RoleOfResponsible.CUSTOMER;
                            Project.Participants[(int)ParticipantRole.DEVELOPER].ResponsibleEmployee.Add(employee);
                            break;
                        case "Лицо, осуществляющее строительство":
                            employee.RoleOfResponsible = RoleOfResponsible.GENERAL_CONTRACTOR;
                            Project.Participants[(int)ParticipantRole.GENERAL_CONTRACTOR].ResponsibleEmployee.Add(employee);
                            break;
                        case "Лицо, осуществляющее строительство отвественное за строительный контроль":
                            employee.RoleOfResponsible = RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER;
                            Project.Participants[(int)ParticipantRole.GENERAL_CONTRACTOR].ResponsibleEmployee.Add(employee);
                            break;

                        case "Лицо, осуществляющее подготовку проектной документации":
                            employee.RoleOfResponsible = RoleOfResponsible.AUTHOR_SUPERVISION;
                            Project.Participants[(int)ParticipantRole.DISIGNER].ResponsibleEmployee.Add(employee);
                            break;
                        case "Лицо, выполнившее работы, подлежащие освидетельствованию":
                            employee.RoleOfResponsible = RoleOfResponsible.WORK_PERFORMER;
                            Project.Participants[(int)ParticipantRole.BUILDER].ResponsibleEmployee.Add(employee);
                            break;
                        default: employee.RoleOfResponsible = RoleOfResponsible.NONE; break;

                    }

                    employee.NRSId = responsibleEmplDataWorksheet.Cells[rowIndex, 3].Value?.ToString();
                    employee.EmployeePosition = new EmployeePosition(responsibleEmplDataWorksheet.Cells[rowIndex, 4].Value?.ToString());
                    employee.FullName = responsibleEmplDataWorksheet.Cells[rowIndex, 5].Value?.ToString();
                    employee.DocConfirmingTheAthority = new Document(responsibleEmplDataWorksheet.Cells[rowIndex, 6].Value?.ToString());
                    string fdf = responsibleEmplDataWorksheet.Cells[rowIndex, 7].Value?.ToString();
                    var company = Companies.SingleOrDefault(c => c.Name == fdf);
                    if (company != null) employee.Company = company;

                    ResponsibleEmployees.Add(employee);
                    rowIndex++;
                }
                #endregion
                rowIndex = 3;
                Project.ResponsibleEmployees = ResponsibleEmployees;
                //  while (AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString() <!=null) //По всем не пустым строком таблицы Ответсвенные
                Materials = new ObservableCollection<Material>();
                rowIndex = 2;
                while (VKActsDataWorksheet.Cells[rowIndex, 7].Value?.ToString() != null) //По всем не пустым строком таблицы Акты ВК загружаем материалы
                {
                    Material material = new Material(VKActsDataWorksheet.Cells[rowIndex, 7].Value?.ToString());
                    material.Quantity = Convert.ToDouble(VKActsDataWorksheet.Cells[rowIndex, 9].Value?.ToString());
                    material.Measure = VKActsDataWorksheet.Cells[rowIndex, 10].Value?.ToString();
                    material.FullName = VKActsDataWorksheet.Cells[rowIndex, 20].Value?.ToString().Replace("_", " "); ;

                    if (VKActsDataWorksheet.Cells[rowIndex, 20].Value?.ToString() != null)
                    {
                        Document document = new Document(VKActsDataWorksheet.Cells[rowIndex, 20].Value?.ToString());
                        document.Name = VKActsDataWorksheet.Cells[rowIndex, 11].Value?.ToString();
                        document.RegId = VKActsDataWorksheet.Cells[rowIndex, 12].Value?.ToString();
                        document.Date = Convert.ToDateTime(VKActsDataWorksheet.Cells[rowIndex, 13].Value?.ToString());
                        document.FullName = material.FullName;
                        document.PrintingName = document.FullName + " от " + document.Date.ToString("d") + ";";
                        material.Documents.Add(document);

                    }
                    Materials.Add(material);
                    rowIndex++;
                }

                LaboratoryReports = new ObservableCollection<Document>();
                rowIndex = 2;
                while (LabReporstDataWorksheet.Cells[rowIndex, 3].Value?.ToString() != null) //По всем не пустым строком таблицы Лаборатория
                {
                    Document report = new Document(LabReporstDataWorksheet.Cells[rowIndex, 7].Value?.ToString());
                    report.RegId = LabReporstDataWorksheet.Cells[rowIndex, 1].Value?.ToString();
                    report.Date = Convert.ToDateTime(LabReporstDataWorksheet.Cells[rowIndex, 2].Value?.ToString());
                    //report.Name = LabReporstDataWorksheet.Cells[rowIndex, 2].Value?.ToString();
                    report.Name = LabReporstDataWorksheet.Cells[rowIndex, 3].Value?.ToString() + " " + LabReporstDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    report.FullName = LabReporstDataWorksheet.Cells[rowIndex, 7].Value?.ToString().Replace("_", " ");
                    report.PrintingName = report.FullName + " от " + report.Date.ToString("d") + ";";
                    LaboratoryReports.Add(report);
                    rowIndex++;
                }

                rowIndex = 2;
                while (AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString() != null) //По всем не пустым строком таблицы АОСР создаем работы
                {
                    Work work = new Work();
                    AOSRDocument AOSR = new AOSRDocument("Акт освидетельствования скрытых работ");
                    work.AOSRDocument = AOSR;
                    AOSR.RegId = AOSRDataWorksheet.Cells[rowIndex, 4].Value?.ToString();
                    for (int ii = 5; ii <= 9; ii++) //Определяем отвественных для данной работы 
                    {
                        var code = Project.ResponsibleEmployees
                               .FirstOrDefault(emp => emp.Code == AOSRDataWorksheet.Cells[rowIndex, ii].Value?.ToString().ToString());
                        AOSR.ResponsibleEmployees.Add(code);
                    }
                    work.Name = AOSRDataWorksheet.Cells[rowIndex, 11].Value?.ToString();
                    work.Level = AOSRDataWorksheet.Cells[rowIndex, 12].Value?.ToString();
                    work.Axes = AOSRDataWorksheet.Cells[rowIndex, 13].Value?.ToString();
                    work.Quantity = Convert.ToDouble(AOSRDataWorksheet.Cells[rowIndex, 14].Value?.ToString());
                    work.Measure = AOSRDataWorksheet.Cells[rowIndex, 15].Value?.ToString();
                    work.NextWorks.Add(new Work(AOSRDataWorksheet.Cells[rowIndex, 16].Value?.ToString()));
                    work.ProjectCode = AOSRDataWorksheet.Cells[rowIndex, 17].Value?.ToString();
                    string text = AOSRDataWorksheet.Cells[rowIndex, 18].Value?.ToString();
                    if (text != null)
                    {
                        string[] text_array = Regex.Split(text, @"; ");
                        foreach (string str in text_array)
                        {
                            var material = Materials.FirstOrDefault(mt => mt.FullName == str.Replace("_", " "));
                            if (material != null) work.Materials.Add(material); //Временно. Материал -  просто список в имя одного материала
                        }
                    }
                    text = AOSRDataWorksheet.Cells[rowIndex, 19].Value?.ToString();
                    if (text != null)
                    {
                        string[] text_array = Regex.Split(text, @"; ");
                        foreach (string str in text_array)
                        {
                            var report = LaboratoryReports.FirstOrDefault(rp => rp.FullName == str.Replace("_", " "));
                            if (report != null) work.LaboratoryReports.Add(report); //Временно. Материал -  просто список в имя одного материала
                        }
                    }
                    AOSR.Date = Convert.ToDateTime(AOSRDataWorksheet.Cells[rowIndex, 25].Value?.ToString());
                    Document executiveScheme = new Document();
                    string str_scheme_num = (string)AOSRDataWorksheet.Cells[rowIndex, 20].Value?.ToString();
                    str_scheme_num = str_scheme_num?.Replace("№", "").Replace(" ", "");
                    if (str_scheme_num != null && str_scheme_num != "")
                    {
                        executiveScheme.RegId = str_scheme_num.ToString();
                        executiveScheme.Date = AOSR.Date;
                        /*executiveScheme.Name = "Исполнительная схема. " + work.Name
                            + " на отметке " + work.Level + " в осях " + work.Axes;*/
                        if (work.Axes != null)
                        {
                            executiveScheme.Name = "Исполнительная схема. " + work.Name
                                                  + " " + work?.Level + " в осях " + work.Axes;
                            executiveScheme.FullName = executiveScheme.Name + " №" + executiveScheme.RegId.ToString();
                        }
                        else
                        {
                            executiveScheme.Name = "Исполнительная схема. " + work.Name
                                              + " " + work?.Level;
                            executiveScheme.FullName = executiveScheme.Name + " №" + executiveScheme.RegId.ToString();
                        }

                        executiveScheme.PrintingName = executiveScheme.FullName + " от " + work.AOSRDocument.Date.ToString("d") + ";";
                        work.ExecutiveSchemes.Add(executiveScheme); //Временно. Исполнительные схемы -  просто список в имя одной схмемы
                    }
                    work.Regulations = AOSRDataWorksheet.Cells[rowIndex, 23].Value?.ToString();
                    work.StartDate = Convert.ToDateTime(AOSRDataWorksheet.Cells[rowIndex, 26].Value?.ToString());
                    work.EndDate = Convert.ToDateTime(AOSRDataWorksheet.Cells[rowIndex, 27].Value?.ToString());
                    Project.Works.Add(work);
                    rowIndex++;
                }
            }
        }

        private bool CanCreateAOSR()
        {
        //    if (Project?.Works?.Count > 0)
                return true;
          //  else
               // return false;
        }
        private void CreateAOSR()
        {

            string templates_path = Directory.GetCurrentDirectory() + "\\Шаблоны";

            Microsoft.Office.Interop.Word._Application world_application;
            Microsoft.Office.Interop.Word._Document world_document;
            Microsoft.Office.Interop.Word._Document world_doc_appendix;
            Microsoft.Office.Interop.Word._Document world_attached_doc;

            int i_doc_att_num = 1;
            int selection_row_from = FormAOSR;//Globals.ThisAddIn.Application.Selection.Cells[0].Row - 1; //Address;
            int selection_row_to = ToAOSR; //Globals.ThisAddIn.Application.Selection.Cells[Globals.ThisAddIn.Application.Selection.Cells.Count].Row - 1; //Address;
            int i_table2_row_add_counter = 0;
            world_application = new Microsoft.Office.Interop.Word.Application();
            //   using (Microsoft.Office.Interop.Word._Application world_application = new Microsoft.Office.Interop.Word._Application())
            {

                world_document = world_application.Documents.Add(templates_path + "\\АОСР.docx");
                world_application.Visible = true;

                for (int work_index = selection_row_from; work_index <= selection_row_from; work_index++)
                {
                    Work current_work = Project.Works[work_index];
                    current_work.AOSRDocument.AttachDocuments.Clear();
                    world_document.Bookmarks["Number"].Range.Text = current_work.AOSRDocument.RegId; //Номер акта
                    string s_date = current_work.AOSRDocument.Date.ToString("d");
                    world_document.Bookmarks["Date_Sign"].Range.Text =
                            ((DateTime)current_work.AOSRDocument.Date).ToString("d");// Дата акта

                    world_document.Bookmarks["Object_name"].Range.Text = Project.FullName;//Наименованиа объекта

                    string Client_name = Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER).Company.FullName + //Застройщик
                        ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER).Company.Contacts +
                        ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER).Company.SROIssuingCompany.FullName;
                    world_document.Bookmarks["Client_name"].Range.Text = Client_name;

                    string GCC_name = Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.FullName +
                        ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.Contacts +
                        ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.SROIssuingCompany.FullName;
                    world_document.Bookmarks["GCC_name"].Range.Text = GCC_name;//Ген подрядчик

                    string Author_name = Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).Company.FullName +
                        ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).Company.Contacts +
                        ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).Company.SROIssuingCompany.FullName;
                    world_document.Bookmarks["Author_name"].Range.Text = Author_name;//Проективрощики

                    string Client_Signer = Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).EmployeePosition.Name +
                        " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName +
                        " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).DocConfirmingTheAthority.Name +
                        " " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DEVELOPER).Company.FullName;
                    world_document.Bookmarks["Client_Signer"].Range.Text = Client_Signer;//Предстваитель затройщика


                    string GCC1_Signer = Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).EmployeePosition.Name +
                    " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName +
                    " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).DocConfirmingTheAthority.Name
                    ;//+ " " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.FullName;
                    world_document.Bookmarks["GCC1_Signer"].Range.Text = GCC1_Signer;//Предстваитель лица осуществлящего строительва ( ген подрядчк)

                    string GCC2_Signer = Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).EmployeePosition.Name +
                       " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName +
                       " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).DocConfirmingTheAthority.Name
                       ;// +" " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.FullName;
                    world_document.Bookmarks["GCC2_Signer"].Range.Text = GCC2_Signer;//Предстваитель лица осуществлящего строительва ( ген подрядчк) по строй котролю


                    string Author_Signer = Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).EmployeePosition.Name +
                   " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName +
                   " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).DocConfirmingTheAthority.Name +
                   " " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).Company.FullName;
                    world_document.Bookmarks["Author_Signer"].Range.Text = Author_Signer;//Предстваитель австорского надзора
                    string SubC_Signer = Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).EmployeePosition.Name +
                    " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName +
                    " " + Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).DocConfirmingTheAthority.Name +
                    " " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.BUILDER).Company.FullName;
                    world_document.Bookmarks["SubC_Signer"].Range.Text = SubC_Signer;//Предстваитель лица  непосредственно выполняющего работы

                    string SubC_name1 = Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.GENERAL_CONTRACTOR).Company.Name;
                    world_document.Bookmarks["SubC_name1"].Range.Text = SubC_name1; // Иные лизац

                    if (current_work.Axes != null)
                        world_document.Bookmarks["Work1"].Range.Text = current_work.Name//Нименование работы к освидетелтьсвтованию
                        + " " + current_work?.Level + " в осях " + current_work.Axes + " " + Project.ShortName;
                    else
                        world_document.Bookmarks["Work1"].Range.Text = current_work.Name//Нименование работы к освидетелтьсвтованию
                     + " " + current_work?.Level + " " + Project.ShortName;

                    world_document.Bookmarks["Project"].Range.Text = current_work.ProjectCode //Проектная документация
                      + ", " + Project.Participants.FirstOrDefault(p => p.Role == ParticipantRole.DISIGNER).Company.Name;
                    #region Вывод данных по используемым материалам
                    string str = "";
                    foreach (Material material in current_work.Materials)
                        str += material.FullName + " ";

                    List<string> str_arr = DivideOnSubstring(str, ROW_LENGHT);
                    if (str_arr.Count <= 3)
                    {
                        foreach (Material material in current_work.Materials)
                            foreach (Document doc in material.Documents)
                                current_work.AOSRDocument.AttachDocuments.Add(doc);

                        world_document.Tables[2].Rows[9].Range.Text = str_arr[str_arr.Count - 1];
                        for (int ii = 0; ii < str_arr.Count - 1; ii++)
                        {
                            world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[9]);
                            world_document.Tables[2].Rows[9].Range.Text = str_arr[ii];
                            i_table2_row_add_counter++;
                        }

                    }
                    else
                    {
                        world_document.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                        world_attached_doc = world_application.Documents.Add(templates_path + "\\Приложения.docx");
                        Microsoft.Office.Interop.Word._Document world_attached_doc_table = world_application.Documents.Add(templates_path + "\\Таблица к Приложениям.docx");
                        AOSRDocument attach_doc = new AOSRDocument();
                        attach_doc.Name = "Реестр строительных материалов (конструкций) ";//+ current_work.AOSRDocument.FullName;
                        attach_doc.Date = current_work.AOSRDocument.Date;
                        attach_doc.PagesNumber = Convert.ToInt32(world_attached_doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages));
                        attach_doc.RegId = i_doc_att_num.ToString();
                        current_work.AOSRDocument.AttachDocuments.Add(attach_doc);

                        DateTime material_last_date = current_work.Materials.OrderBy(m => m.Date).FirstOrDefault().Date;
                        if (material_last_date < current_work.AOSRDocument.Date) material_last_date = current_work.AOSRDocument.Date;
                        Microsoft.Office.Interop.Word.Table attached_table = world_attached_doc_table.Tables[1];
                        attach_doc.PrintingName = attach_doc.Name + " №" + current_work.AOSRDocument.RegId
                          + " от " + material_last_date.ToString("d")
                           + " на " + (Math.Ceiling((double)attach_doc.PagesNumber / 2)).ToString() + " листе (ах). ";

                        attached_table.Cell(1, 1).Range.Text = attach_doc.Name + " №" + current_work.AOSRDocument.RegId
                          + " от " + material_last_date.ToString("d") + ".";

                        int material_number = 1;
                        for (int ii = 0; ii < current_work.Materials.Count - 1; ii++)
                            attached_table.Rows.Add(attached_table.Rows[4]);
                        int row_index = 4;
                        foreach (Material material in current_work.Materials)
                        {
                            attached_table.Cell(row_index, 1).Range.Text = material_number.ToString(); ;// 
                            attached_table.Cell(row_index, 2).Range.Text = material.Name + material.Documents[0].Name;
                            attached_table.Cell(row_index, 3).Range.Text = material.Documents[0].RegId; ;
                            attached_table.Cell(row_index, 4).Range.Text = material.Documents[0].Date.ToString("d");
                            material_number++;
                            row_index++;
                        }
                        world_attached_doc.Bookmarks["Parent_doc"].Range.Text = "Приложение №"
                            + (current_work.AOSRDocument.AttachDocuments.Count).ToString()
                            + " к АОСР №" + current_work.AOSRDocument.RegId
                            + " от " + ((DateTime)(current_work.AOSRDocument.Date)).ToString("d");
                        world_attached_doc.Bookmarks["Client_Signer2"].Range.Text =
                             Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName;
                        world_attached_doc.Bookmarks["GCC1_Signer2"].Range.Text =
                            Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName;
                        world_attached_doc.Bookmarks["GCC2_Signer2"].Range.Text =
                            Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName;
                        world_attached_doc.Bookmarks["Author_Signer2"].Range.Text =
                           Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName;
                        world_attached_doc.Bookmarks["SubC_Signer2"].Range.Text =
                            Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName;


                        attached_table.Select();
                        world_application.Selection.Copy();
                        world_attached_doc.Tables[1].Cell(2, 1).Range.PasteSpecial();
                        world_attached_doc.Tables[1].Select();
                        world_application.Selection.Copy();
                        world_document.Words.Last.PasteSpecial();

                        world_document.Bookmarks["Materials"].Range.Text = attach_doc.PrintingName;

                        world_application.Documents[world_attached_doc].Close(false);
                        world_application.Documents[world_attached_doc_table].Close(false);
                        // world_attached_doc.Close();
                    }

                    #endregion
                    #region Вывод данных по  п4.АОСР
                    str_arr.Clear();
                    str = "";
                    foreach (Document scheme in current_work.ExecutiveSchemes)
                    {
                        str += scheme.PrintingName + " ";
                        //   current_work.AOSRDocument.AttachDocuments.Add(scheme);
                    }
                    foreach (Document report in current_work.LaboratoryReports)
                    {
                        str += report.PrintingName + " ";
                        //       current_work.AOSRDocument.AttachDocuments.Add(report);
                    }
                    #endregion
                    str_arr = DivideOnSubstring(str, ROW_LENGHT);
                    if (str_arr.Count <= 3)
                    {
                        foreach (Document scheme in current_work.ExecutiveSchemes)
                            current_work.AOSRDocument.AttachDocuments.Add(scheme);
                        foreach (Document report in current_work.LaboratoryReports)
                            current_work.AOSRDocument.AttachDocuments.Add(report);

                        world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - 1];
                        for (int ii = 0; ii < str_arr.Count - 1; ii++)//Заполняем поле документы подтверждающие качество выпоненных работ
                        {
                            world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[12 + i_table2_row_add_counter]);
                            world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[ii];
                            i_table2_row_add_counter++;
                        }
                    }
                    else
                    {
                        world_document.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                        world_attached_doc = world_application.Documents.Add(templates_path + "\\Приложения.docx");
                        Microsoft.Office.Interop.Word._Document world_attached_doc_table = world_application.Documents.Add(templates_path + "\\Таблица к Приложениям.docx");
                        AOSRDocument attach_doc = new AOSRDocument();
                        attach_doc.Name = "Реестр документов, подтверждающих соответствие работ предъявляемым к ним требованиям ";
                        attach_doc.Date = current_work.AOSRDocument.Date;
                        attach_doc.PagesNumber = Convert.ToInt32(world_attached_doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages));
                        attach_doc.RegId = i_doc_att_num.ToString();
                        current_work.AOSRDocument.AttachDocuments.Add(attach_doc);

                        DateTime attach_last_date = current_work.LaboratoryReports.OrderBy(r => r.Date).FirstOrDefault().Date;
                        Microsoft.Office.Interop.Word.Table attached_table = world_attached_doc_table.Tables[1];
                        attach_doc.PrintingName = attach_doc.Name + " №" + current_work.AOSRDocument.RegId
                          + " от " + attach_last_date.ToString("d")
                           + " на " + (Math.Ceiling((double)attach_doc.PagesNumber / 2)).ToString() + " листе (ах). ";

                        attached_table.Cell(1, 1).Range.Text = attach_doc.Name + " №" + current_work.AOSRDocument.RegId
                          + " от " + attach_last_date.ToString("d") + ".";

                        int _number = 1;
                        for (int ii = 0; ii < current_work.ExecutiveSchemes.Count - 1 + current_work.LaboratoryReports.Count; ii++)
                            attached_table.Rows.Add(attached_table.Rows[4]);
                        int row_index = 4;
                        foreach (Document scheme in current_work.ExecutiveSchemes)
                        {
                            attached_table.Cell(row_index, 1).Range.Text = _number.ToString();// 
                            attached_table.Cell(row_index, 2).Range.Text = scheme.Name;
                            attached_table.Cell(row_index, 3).Range.Text = scheme.RegId; ;
                            attached_table.Cell(row_index, 4).Range.Text = scheme.Date.ToString("d");
                            _number++;
                            row_index++;
                        }
                        foreach (Document report in current_work.LaboratoryReports)
                        {
                            attached_table.Cell(row_index, 1).Range.Text = _number.ToString(); ;// 
                            attached_table.Cell(row_index, 2).Range.Text = report.Name;
                            attached_table.Cell(row_index, 3).Range.Text = report.RegId; ;
                            attached_table.Cell(row_index, 4).Range.Text = report.Date.ToString("d");
                            _number++;
                            row_index++;
                        }


                        world_attached_doc.Bookmarks["Parent_doc"].Range.Text = "Приложение №"
                            + (current_work.AOSRDocument.AttachDocuments.Count).ToString()
                            + " к АОСР №" + current_work.AOSRDocument.RegId
                            + " от " + ((DateTime)(current_work.AOSRDocument.Date)).ToString("d");
                        world_attached_doc.Bookmarks["Client_Signer2"].Range.Text =
                             Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName;
                        world_attached_doc.Bookmarks["GCC1_Signer2"].Range.Text =
                            Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName;
                        world_attached_doc.Bookmarks["GCC2_Signer2"].Range.Text =
                            Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName;
                        world_attached_doc.Bookmarks["Author_Signer2"].Range.Text =
                           Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName;
                        world_attached_doc.Bookmarks["SubC_Signer2"].Range.Text =
                            Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName;

                        attached_table.Select();
                        world_application.Selection.Copy();
                        world_attached_doc.Tables[1].Cell(2, 1).Range.PasteSpecial();
                        world_attached_doc.Tables[1].Select();
                        world_application.Selection.Copy();
                        world_document.Words.Last.PasteSpecial();

                        // world_document.Bookmarks["Documents"].Range.Text = attach_doc.PrintingName;
                        str_arr.Clear();
                        str = "";
                        str_arr = DivideOnSubstring(attach_doc.PrintingName, ROW_LENGHT);
                        world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - 1];
                        for (int ii = 0; ii < str_arr.Count - 1; ii++)//Заполняем поле документы подтверждающие качество выпоненных работ
                        {
                            world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[12 + i_table2_row_add_counter]);
                            world_document.Tables[2].Rows[12 + i_table2_row_add_counter].Range.Text = str_arr[ii];
                            i_table2_row_add_counter++;
                        }

                        world_application.Documents[world_attached_doc].Close(false);
                        world_application.Documents[world_attached_doc_table].Close(false);
                    }
                    str = "";
                    str_arr.Clear();
                    int attach_doc_counter = 1;
                    foreach (Document doc in current_work.AOSRDocument.AttachDocuments)
                    {
                        str += attach_doc_counter++.ToString() + ")" + doc.PrintingName + " ";
                    }

                    str_arr = DivideOnSubstring(str, ROW_LENGHT); ;
                    world_document.Tables[2].Rows[29 + i_table2_row_add_counter].Range.Text = str_arr[str_arr.Count - 1];
                    for (int ii = 0; ii < str_arr.Count - 1; ii++) //Заполняем пункт "Приложения"
                    {
                        world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[29 + i_table2_row_add_counter]);
                        world_document.Tables[2].Rows[29 + i_table2_row_add_counter].Range.Text = str_arr[ii];
                        i_table2_row_add_counter++;
                    }

                    world_document.Bookmarks["SNiP"].Range.Text = current_work.Regulations;
                    world_document.Bookmarks["Next_Work"].Range.Text = current_work.NextWorks[0].Name;
                    world_document.Bookmarks["Client_Signer2"].Range.Text =
                        Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.CUSTOMER).FullName;
                    world_document.Bookmarks["GCC1_Signer2"].Range.Text =
                        Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR).FullName;
                    world_document.Bookmarks["GCC2_Signer2"].Range.Text =
                        Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).FullName;
                    world_document.Bookmarks["Author_Signer2"].Range.Text =
                       Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.AUTHOR_SUPERVISION).FullName;
                    world_document.Bookmarks["SubC_Signer2"].Range.Text =
                     Project.ResponsibleEmployees.FirstOrDefault(re => re.RoleOfResponsible == RoleOfResponsible.WORK_PERFORMER).FullName;
                    world_document.Bookmarks["Date_Begin"].Range.Text = current_work.StartDate.ToString("d");
                    world_document.Bookmarks["Date_End"].Range.Text = current_work.EndDate.ToString("d");


                }
                world_application.Visible = true;
            }
        }
        private static string SubString(string s, int start, int size, out int newStart, out bool isEnd)
        {
            StringBuilder strB = new StringBuilder();
            strB.Capacity = size;

            int end = (start + size > s.Length) ? s.Length : start + size;
            for (int i = start; i < end; i++)
            {
                strB.Append(s[i]);
            }

            isEnd = (end == s.Length);
            newStart = end;
            return strB.ToString();
        }

        public static List<string> DivideOnSubstring(string s, int stepSize)
        {
            List<string> ans = new List<string>();

            string[] worlds = s.Split(new char[] { ' ' });
            int char_count = 0;
            string temp_str = "";
            if (s.Length < stepSize)
            {
                ans.Add(s);
                return ans;
            }
            for (int ii = 0; ii < worlds.Length; ii++)
            {
                temp_str += worlds[ii] + " ";
                char_count += worlds[ii].Length;

                if (char_count + worlds[ii].Length > stepSize || ii == worlds.Length - 1)
                {
                    ans.Add(temp_str);
                    temp_str = "";
                    char_count = 0;
                }
            }
            return ans;
        }

        public static List<string> DivideOnSubstring2(string s, int stepSize)
        {
            List<string> ans = new List<string>();
            int start = 0;
            bool flag = false;
            if (stepSize > s.Length) stepSize = s.Length;
            do
            {
                ans.Add(s.Substring(start, stepSize));
                start += stepSize;
                if (stepSize > s.Length - start) flag = true;
            } while (!flag);
            ans.Add(s.Substring(start, s.Length - start));
            return ans;
        }

        #endregion

    }
}
