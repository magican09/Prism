using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldAOSRDocument : bldDocument, IbldAOSRDocument, INameable, IEntityObject
    {
        private bldResponsibleEmployeesGroup _responsibleEmployees = new bldResponsibleEmployeesGroup();
        public virtual bldResponsibleEmployeesGroup ResponsibleEmployees
        {
            get { return _responsibleEmployees; }
            set { SetProperty(ref _responsibleEmployees, value); }
        }
        private string _fullName;
        [NotJornaling]
        public override string ShortName
        {
            get
            {
                this.JornalingOff();
                int wrk_name_leng = 30;
                if (bldWork?.Name.Length < wrk_name_leng) wrk_name_leng = bldWork.Name.Length;
                _fullName = $"АОСР №{RegId} от {Date.ToString("d")} {bldWork?.Name.Substring(0, wrk_name_leng)}...";
                this.JornalingOn();
                return _fullName;
            }
            set { }
        }
        public override string Name
        {
            get
            {
                if (base.Name == null)
                {
                    return ShortName;
                }
                return base.Name;
            }
            set => base.Name = value;
        }
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }//Дата начала
        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }//Дата окончания
        public bldAOSRDocument(string name) : base(name)
        {

        }
        public bldAOSRDocument()
        {

        }

        [NavigateProperty]
        public Guid bldWorkId { get; set; }
        private bldWork _work;
        [NavigateProperty]
        public virtual bldWork bldWork
        {
            get { return _work; }
            set { SetProperty(ref _work, value); }
        }
        public const int ROW_LENGHT = 92;
        public void SaveAOSRToWord(string folderPath)
        {
            bldAOSRDocument aOSRDocument = this;
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
                    bldParticipantsGroup all_participants = current_work.Participants;
                    //  AOSRDocument aOSRDocument = current_work.AOSRDocuments
                    //  foreach (AOSRDocument aOSRDocument in current_work.AOSRDocuments)
                    {
                        //  aOSRDocument.AttachDocuments.Clear();
                        world_document.Bookmarks["Number"].Range.Text = aOSRDocument.RegId; //Номер акта
                        string s_date = aOSRDocument.Date.ToString("d");
                        world_document.Bookmarks["Date_Sign"].Range.Text =
                                ((DateTime)aOSRDocument.Date).ToString("d");// Дата акта

                        world_document.Bookmarks["Object_name"].Range.Text = project.FullName;//Наименованиа объекта

                        string developer_company_name = project.Participants.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DEVELOPER)?.ConstructionCompany?.FullName;
                        string developer_company_contacts = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DEVELOPER)?.ConstructionCompany?.Contacts;
                        string developer_company_SROCompany_name = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DEVELOPER)?.ConstructionCompany?.SROIssuingCompany.FullName;
                        string Client_name = $"{developer_company_name}, {developer_company_contacts}, {developer_company_SROCompany_name}."; //Застройщик
                        world_document.Bookmarks["Client_name"].Range.Text = Client_name;

                        string genera_contr_company_full_name = project.Participants.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompany?.FullName;
                        string genera_contr_company_contacts = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompany?.Contacts;
                        string genera_contr_company_SROCompany_name = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompany?.SROIssuingCompany.FullName;
                        string GCC_name = $"{genera_contr_company_full_name}, {genera_contr_company_contacts}, {genera_contr_company_SROCompany_name}."; //Застройщик
                        world_document.Bookmarks["GCC_name"].Range.Text = GCC_name;//Ген подрядчик

                        string author_company_name = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DISIGNER)?.ConstructionCompany?.FullName;
                        string author_company_contacts = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DISIGNER)?.ConstructionCompany?.Contacts;
                        string author_company_SROCompany_name = project.Participants?.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DISIGNER)?.ConstructionCompany?.SROIssuingCompany.FullName;
                        string Author_name = $"{author_company_name}, {author_company_contacts}, {author_company_SROCompany_name}."; //Застройщик
                        world_document.Bookmarks["Author_name"].Range.Text = Author_name;//Проективрощики


                        string cuctomer_position_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.CUSTOMER).Employee.Position.Name;
                        string cuctomer_fullname = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.CUSTOMER).Employee.FullName;
                        string cuctomer_emp_doc_confirm_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.CUSTOMER).DocConfirmingTheAthority.Name;
                        string Client_Signer = $"{cuctomer_position_name} {cuctomer_fullname} {cuctomer_emp_doc_confirm_name} {developer_company_name}."; //Застройщик
                        world_document.Bookmarks["Client_Signer"].Range.Text = Client_Signer;//Предстваитель затройщика

                        string genera_contr_emp_position_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR)?.Employee?.Position?.Name;
                        string genera_contr_emp_fullname = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR)?.Employee?.FullName;
                        string genera_contr_emp_doc_confirm_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR)?.DocConfirmingTheAthority?.Name;
                        string GCC1_Signer = $"{genera_contr_emp_position_name} {genera_contr_emp_fullname} {genera_contr_emp_doc_confirm_name}."; //Генподрядчик
                        world_document.Bookmarks["GCC1_Signer"].Range.Text = GCC1_Signer;//Предстваитель лица осуществлящего строительва ( ген подрядчк)

                        string genera_contr_quality_emp_position_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).Employee?.Position.Name;
                        string genera_contr_quality_emp_fullname = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).Employee?.FullName;
                        string genera_contr_quality_emp_doc_confirm_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).DocConfirmingTheAthority.Name;
                        string GCC2_Signer = $"{genera_contr_quality_emp_position_name} {genera_contr_quality_emp_fullname} {genera_contr_quality_emp_doc_confirm_name}."; //Генподрядчик технадзор
                        world_document.Bookmarks["GCC2_Signer"].Range.Text = GCC2_Signer;//Предстваитель лица осуществлящего строительва ( ген подрядчк) по строй котролю

                        string author_supervision_emp_position_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.AUTHOR_SUPERVISION).Employee.Position.Name;
                        string author_supervision_emp_fullname = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.AUTHOR_SUPERVISION).Employee?.FullName;
                        string author_supervision_emp_doc_confirm_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.AUTHOR_SUPERVISION).DocConfirmingTheAthority.Name;
                        string disigner_company_fullname = all_participants.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DISIGNER).ConstructionCompany.FullName;
                        string Author_Signer = $"{author_supervision_emp_position_name} {author_supervision_emp_fullname} {author_supervision_emp_doc_confirm_name} {disigner_company_fullname}."; //Авторский надзор
                        world_document.Bookmarks["Author_Signer"].Range.Text = Author_Signer;//Предстваитель австорского надзора

                        string work_performer_emp_position_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.WORK_PERFORMER).Employee?.Position.Name;
                        string work_performer_emp_fullname = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.WORK_PERFORMER).Employee?.FullName;
                        string work_performer_emp_doc_confirm_name = project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.WORK_PERFORMER).DocConfirmingTheAthority.Name;
                        string builder_company_fullname = all_participants.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.BUILDER)?.ConstructionCompany.FullName; ;
                        string SubC_Signer = $"{work_performer_emp_position_name} {work_performer_emp_fullname} {work_performer_emp_doc_confirm_name} {builder_company_fullname}."; //Подрядчик
                        world_document.Bookmarks["SubC_Signer"].Range.Text = SubC_Signer;//Предстваитель лица  непосредственно выполняющего работы

                        string genera_contr_company_name = all_participants.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.GENERAL_CONTRACTOR)?.ConstructionCompany.Name;
                        world_document.Bookmarks["SubC_name1"].Range.Text = genera_contr_company_name; // Иные лизац

                        string str = "";

                        if (current_work.WorkArea.Axes != null)
                            str = $"{current_work.Name} {current_work?.WorkArea.Levels} в осях {current_work.WorkArea.Axes} {project.ShortName}.";
                        else
                            str = $"{current_work.Name} {current_work?.WorkArea.Levels} {project.ShortName}.";

                        List<string> str_arr = new List<string>();
                        str_arr = DivideOnSubstring(str, ROW_LENGHT);
                        world_document.Tables[2].Rows[3].Range.Text = str_arr[str_arr.Count - 1];
                        for (int ii = 2; str_arr.Count - ii >= 0; ii++)
                        {
                            world_document.Tables[2].Rows.Add(world_document.Tables[2].Rows[3]);
                            world_document.Tables[2].Rows[3].Range.Text = str_arr[str_arr.Count - ii];
                            i_table2_row_add_counter++;
                        }
                        string disigner_company_name = all_participants.FirstOrDefault(p => p.Role.RoleCode == ParticipantRole.DISIGNER)?.Name;

                        if (current_work.ProjectDocuments.Count > 0)
                            world_document.Bookmarks["project"].Range.Text = $"{current_work.ProjectDocuments[0]?.Name} {disigner_company_name}";  //Проектная документация

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
                            bldDocumentsRegister attach_doc = new bldDocumentsRegister();
                            attach_doc.Name = "Реестр строительных материалов (конструкций) ";//+aOSRDocument.FullName;
                            attach_doc.Date = aOSRDocument.Date;
                            attach_doc.PagesNumber = Convert.ToInt32(world_attached_doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages));
                            attach_doc.RegId = i_doc_att_num.ToString();
                            docs_for_word_attach.Add(attach_doc);

                            DateTime? material_last_date = current_work.Materials.OrderBy(m => m.Date).FirstOrDefault().Date;
                            if (material_last_date < aOSRDocument.Date) material_last_date = aOSRDocument.Date;
                            Microsoft.Office.Interop.Word.Table attached_table = world_attached_doc_table.Tables[1];
                            attach_doc.FullName = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + material_last_date?.ToString("d")
                               + " на " + (Math.Ceiling((double)attach_doc.PagesNumber / 2)).ToString() + " листе (ах). ";

                            attached_table.Cell(1, 1).Range.Text = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + material_last_date?.ToString("d") + ".";

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
                                 project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.CUSTOMER).Employee?.FullName;
                            world_attached_doc.Bookmarks["GCC1_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR).Employee?.FullName;
                            world_attached_doc.Bookmarks["GCC2_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).Employee?.FullName;
                            world_attached_doc.Bookmarks["Author_Signer2"].Range.Text =
                               project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.AUTHOR_SUPERVISION).Employee?.FullName;
                            world_attached_doc.Bookmarks["SubC_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.WORK_PERFORMER).Employee?.FullName;


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
                            bldAggregationDocument attach_doc = new bldAggregationDocument();
                            attach_doc.Name = "Реестр документов, подтверждающих соответствие работ предъявляемым к ним требованиям ";
                            attach_doc.Date = aOSRDocument.Date;
                            attach_doc.PagesNumber = Convert.ToInt32(world_attached_doc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages));
                            attach_doc.RegId = i_doc_att_num.ToString();
                            docs_for_word_attach.Add(attach_doc);

                            DateTime? attach_last_date = current_work.LaboratoryReports.OrderBy(r => r.Date).FirstOrDefault().Date;
                            Microsoft.Office.Interop.Word.Table attached_table = world_attached_doc_table.Tables[1];
                            attach_doc.FullName = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + attach_last_date?.ToString("d")
                               + " на " + (Math.Ceiling((double)attach_doc.PagesNumber / 2)).ToString() + " листе (ах). ";

                            attached_table.Cell(1, 1).Range.Text = attach_doc.Name + " №" + aOSRDocument.RegId
                              + " от " + attach_last_date?.ToString("d") + ".";

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
                                 project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.CUSTOMER).Employee?.FullName;
                            world_attached_doc.Bookmarks["GCC1_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR).Employee?.FullName;
                            world_attached_doc.Bookmarks["GCC2_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).Employee?.FullName;
                            world_attached_doc.Bookmarks["Author_Signer2"].Range.Text =
                               project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.AUTHOR_SUPERVISION).Employee?.FullName;
                            world_attached_doc.Bookmarks["SubC_Signer2"].Range.Text =
                                project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.WORK_PERFORMER).Employee?.FullName;

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
                            project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.CUSTOMER).Employee?.FullName;
                        world_document.Bookmarks["GCC1_Signer2"].Range.Text =
                            project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR).Employee?.FullName;
                        world_document.Bookmarks["GCC2_Signer2"].Range.Text =
                            project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.GENERAL_CONTRACTOR_CONSTRUCTION_QUALITY_CONTROLLER).Employee?.FullName;
                        world_document.Bookmarks["Author_Signer2"].Range.Text =
                           project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.AUTHOR_SUPERVISION).Employee?.FullName;
                        world_document.Bookmarks["SubC_Signer2"].Range.Text =
                         project.ResponsibleEmployees.FirstOrDefault(re => re.Role.RoleCode == RoleOfResponsible.WORK_PERFORMER).Employee?.FullName;

                        //world_document.Bookmarks["Date_Begin"].Range.Text = current_work.StartTime.ToString("d");
                        // world_document.Bookmarks["Date_End"].Range.Text = current_work.EndTime.ToString("d
                        world_document.Bookmarks["Date_Begin"].Range.Text = current_work.AOSRDocument.StartTime.ToString("d");
                        world_document.Bookmarks["Date_End"].Range.Text = current_work.AOSRDocument.EndTime?.ToString("d");

                    }
                }
                world_application.Visible = true;
                if (folderPath != null && folderPath != "")
                {
                    string file_name =
                        $"{folderPath}\\АОСР {RegId} от {Date.ToString("d")} {bldWork.Name} {bldWork.WorkArea.PlaceFullName}.docx";
                    file_name = file_name.Replace('/', '_');
                    world_document.SaveAs2(file_name);
                    world_application.Quit();
                }
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
