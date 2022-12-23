using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Console;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Core;
using PrismWorkApp.Modules.BuildingModule.Dialogs;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ProjectManagerRibbonTabViewModel : LocalBindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public NotifyCommand LoadProjectFromExcelCommand { get; private set; }
        public NotifyCommand CreateProjectStructureCommand { get; private set; }
        public NotifyCommand LoadProjectFromDBCommand { get; private set; }
        public NotifyCommand SaveDataToDBCommand { get; private set; }
        public NotifyCommand CreateAOSRCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; private set; }
        public NotifyCommand LoadMaterialsFromAccessCommand { get; private set; }

        private const int CURRENT_MODULE_ID = 2;
        public IBuildingUnitsRepository _buildingUnitsRepository;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string _title = "Конвертер";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private IModulesContext _modulesContext;
        public IModulesContext ModulesContext
        {
            get { return _modulesContext; }
            set { SetProperty(ref _modulesContext, value); }
        }
        private IModuleInfoData _moduleInfoData;
        public IModuleInfoData ModuleInfo
        {
            get { return _moduleInfoData; }
            set { SetProperty(ref _moduleInfoData, value); }
        }
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private IDialogService _dialogService;
        private bldProjectsGroup _allProjects = new bldProjectsGroup();
        public bldProjectsGroup AllProjects
        {
            get { return _allProjects; }
            set { _allProjects = value; }
        }
        private bldProjectsGroup _allProjectsContext = new bldProjectsGroup();
        public bldProjectsGroup AllProjectsContext
        {
            get { return _allProjectsContext; }
            set { _allProjectsContext = value; }
        }
        public bldProjectsGroup allbldProjects { get; set; } = new bldProjectsGroup();

        private bool _allChangesIsDone;

        public bool AllChangesIsDone
        {
            get { return _allChangesIsDone; }
            set { _allChangesIsDone = value; }
        }


        private IApplicationCommands _applicationCommands;
        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }

        //public ProjectManagerRibbonTabViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
        //                                    IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands)
        //{

        //}


        public ProjectManagerRibbonTabViewModel(IRegionManager regionManager, IModulesContext modulesContext, IEventAggregator eventAggregator,
                                    IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands,
                                    IUnDoReDoSystem unDoReDo)
        {
        }
        public ProjectManagerRibbonTabViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
                                            IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands)
        {

            _regionManager = regionManager;

            // ModulesContext = modulesContext;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            ApplicationCommands = applicationCommands;
            //  ModuleInfo = ModulesContext.ModulesInfoData.Where(mi => mi.Id == CURRENT_MODULE_ID).FirstOrDefault();

            LoadProjectFromExcelCommand = new NotifyCommand(LoadProjectFromExcel, CanLoadAllProjects);
            LoadProjectFromDBCommand = new NotifyCommand(LoadProjectFomDB, CanLoadProjectFromDb);
            SaveDataToDBCommand = new NotifyCommand(SaveDataToDB, CanSaveDataToDB)
                .ObservesProperty(() => AllChangesIsDone);
            AllChangesIsDone = true;
            ApplicationCommands.SaveAllCommand.SetLastCommand(SaveDataToDBCommand);
            LoadMaterialsFromAccessCommand = new NotifyCommand(OnLoadMaterialsFromAccess);
            ApplicationCommands.LoadMaterialsFromAccessCommand.RegisterCommand(LoadMaterialsFromAccessCommand);

        }

        private void OnLoadMaterialsFromAccess()
        {
            string access_file_name;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                access_file_name = openFileDialog.FileName;


            string ConnectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=" +
                 openFileDialog.FileName + "; Uid = Admin; Pwd =; ";
            string table_name = "Таблица_1";
            string query = $"SELECT * FROM {table_name}";

            string BD_FilesDir = Directory.GetCurrentDirectory();
            BD_FilesDir = Path.Combine(BD_FilesDir, table_name);
            Directory.CreateDirectory(BD_FilesDir);

            MemoryStream memoryStream = new MemoryStream();

            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //OdbcDataAdapter dataAdapter = new OdbcDataAdapter
                //         (query, connection);
                //DataSet dataSet = new DataSet();
                string stmt = "SELECT COUNT(*) FROM " + table_name;
                OdbcCommand command = connection.CreateCommand();
                command.CommandText = query;
                connection.Open();
                //dataAdapter.Fill(dataSet,0,10, table_name);
                //DataTable dataTable = dataSet.Tables[0];
                OdbcDataReader row = command.ExecuteReader();
                int file_count = 0;
                //foreach (DataRow row in dataTable.Rows)
                while (row.Read())
                {
                    file_count++;
                    try
                    {

                        bldMaterialCertificate materialCertificate = new bldMaterialCertificate();
                        materialCertificate.MaterialName = row["Наименование _материала"].ToString();
                        materialCertificate.GeometryParameters = row["Геометрические_параметры"].ToString();
                        if (row["Кол-во"].ToString() != "-" && row["Кол-во"].ToString() != "")
                            materialCertificate.MaterialQuantity = Convert.ToDecimal(row["Кол-во"].ToString().Replace(',', '.'));
                        materialCertificate.UnitsOfMeasure = row["Ед_изм"].ToString();
                        materialCertificate.Name = row["Сертификаты,_паспорта"].ToString();
                        materialCertificate.RegId = row["№_документа_о_качестве"].ToString();
                        string[] st_dates = row["Дата_документа"].ToString().Split('-');
                        if ( row["Дата_документа"].ToString() != "")
                        {
                            if (st_dates.Length > 1 && st_dates[0] != "")
                            {
                                materialCertificate.Date = Convert.ToDateTime(st_dates[0]?.ToString());
                                materialCertificate.StartTime = materialCertificate.Date;
                                materialCertificate.EndTime = Convert.ToDateTime(st_dates[1]?.ToString());
                            }
                            else if (st_dates.Length == 1 && st_dates[0] != "")
                            {
                                materialCertificate.Date = Convert.ToDateTime(row["Дата_документа"]?.ToString());
                                materialCertificate.StartTime = materialCertificate.Date;
                            }
                        }

                        materialCertificate.ControlingParament = row["Контрольный_параметр"].ToString();
                        materialCertificate.RegulationDocumentsName = row["ГОСТ,_ТУ"].ToString();
                        Picture picture = new Picture();
                        //  picture.FileName = Guid.NewGuid().ToString()+".pdf";
                        picture.FileName = ($"{materialCertificate.MaterialName} {materialCertificate.GeometryParameters}  №{materialCertificate.RegId} от {materialCertificate.Date.ToString("d")}  {file_count.ToString()}.pdf")
                            .Replace("/", "_").Replace("(", "").Replace(")", "").Replace("*", " ").Replace("\n","").Replace(@"\", "_")
                            .Replace("\r", "_");

                        //  picture.ImageFile = (byte[])row["files"];
                        byte[] bytes = (byte[])row["files"];
                        materialCertificate.ImageFile = picture;
                        bldMaterial material = new bldMaterial();
                        material.Name = materialCertificate.MaterialName;
                        material.Quantity = materialCertificate.MaterialQuantity;
                        material.UnitOfMeasurement = new bldUnitOfMeasurement(materialCertificate.UnitsOfMeasure);
                        material.Documents.Add(materialCertificate);
                        _buildingUnitsRepository.MaterialCertificates.Add(materialCertificate);
                        _buildingUnitsRepository.Materials.Add(material);
                        //  byte[] bytes = picture.ImageFile;
                        if (bytes != null)
                        {
                            string byte_string = Encoding.GetEncoding(1251).GetString(bytes);
                            Regex regex_1 = new Regex($"%PDF-");
                            MatchCollection matches_1 = regex_1.Matches(byte_string);
                            int fist_byte = matches_1[0].Index;
                            Regex regex_2 = new Regex(@"%%EOF");
                            MatchCollection matches_2 = regex_2.Matches(byte_string);
                            int last_byte = matches_2[matches_2.Count - 1].Index + 6;

                            using (System.IO.FileStream fs = new System.IO.FileStream($"{BD_FilesDir }\\{picture.FileName}", FileMode.OpenOrCreate))
                            {
                                fs.Write(bytes, fist_byte, last_byte - fist_byte);

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Обнаружено не обработанное исключение: " + e.Message, "Ошибка записи данных", MessageBoxButton.OK, MessageBoxImage.Error);
                   //     break;
                    }
                    // System.Threading.Thread.Sleep(100);
                }
                row.Close();
                _buildingUnitsRepository.Complete();

            }
        }
        //private void OnLoadMaterialsFromAccess_1()
        //{
        //   // string sMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    //string sPath = sMyDocumentsPath + "\\Employees.mdb"; C:\work\Металл 1.accdb
        //    string sPath = @"C:\test\Металл_1.mdb";
        //    string sOutFileName = @"C:\test\";

        //    string sPassword = "";

        //    DAO.Database DAODataBase;
        //    DAO.DBEngine DAODBEngine = new DAO.DBEngine();
        //    DAO.Workspace DAOWorkSpace;
        //    DAOWorkSpace = DAODBEngine.Workspaces[0];
        //  //  byte[] bytes;
        //    DAO.Recordset rs;
        //    try
        //    {
        //        DAODataBase = DAOWorkSpace.OpenDatabase(sPath, null, false, null);
        //        rs = DAODataBase.OpenTable("Металл_1", 0);
        //        Recordset rst = DAODataBase.OpenRecordset("SELECT * FROM  Металл_1");
        //        while (!rst.EOF)
        //        {
        //            string file_name =(string) rst.Fields[1].Value;

        //            byte[] bytes = (byte[])rst.Fields[2].Value;
        //            string byte_string = Encoding.GetEncoding(1251).GetString(bytes);
        //            Regex regex_1 = new Regex($"%PDF-");
        //            MatchCollection matches_1 = regex_1.Matches(byte_string);
        //            int fist_byte = matches_1[0].Index;
        //            Regex regex_2 = new Regex(@"%%EOF");
        //            MatchCollection matches_2 = regex_2.Matches(byte_string);
        //            int last_byte = matches_2[matches_2.Count-1].Index+6;

        //            using (System.IO.FileStream fs = new System.IO.FileStream(sOutFileName + file_name + ".pdf", FileMode.OpenOrCreate))
        //            {
        //                fs.Write(bytes, fist_byte, last_byte- fist_byte);
        //            }

        //            rst.MoveNext();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //   System.Windows.Forms.MessageBox.Show("Database Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private bool CanSaveDataToDB()
        {
            return true;//AllProjectsContext.UnDoReDoSystem.Count > 0;
        }
        private void SaveDataToDB()
        {
            CoreFunctions.ConfirmActionDialog(
                "Cохранить в БД изменения", "проектов", "Сохранить", "Отмена", "Сохраниение в БД завершено!",
                 (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        foreach (bldProject project in AllProjects)
                        {
                            var find_project = AllProjectsContext.Where(pr => pr.Id == project.Id).FirstOrDefault();
                            if (find_project != null)
                            {


                            }
                        }

                        SaveDataToDBCommand.RaiseCanExecuteChanged();
                        _buildingUnitsRepository.Complete();
                    }
                }, _dialogService);

        }
        private bool CanLoadProjectFromDb()
        {
            return true;
        }

        private bool CanLoadAllProjects()
        {
            return true;
        }
        private void LoadProjectFromExcel()
        {
            var project = Functions.LoadProjectFromExcel();
            bldProject bld_project = project;


            _buildingUnitsRepository.Projects.Add(bld_project);
            _buildingUnitsRepository.Complete();

            //     var pr_var = _buildingUnitsRepository.Projects.GetAll().FirstOrDefault();
            var pr_var_og = _buildingUnitsRepository.Projects.GetProjectWithAll();
            bld_project = pr_var_og;
            EventMessage message = new EventMessage();
            message.From = CURRENT_MODULE_ID;
            message.To = CURRENT_MODULE_ID;
            message.Sender = "ConvectorRibbonGroup";
            message.Recipient = "ProjectExplorer";
            message.ParameterName = "bldProject";
            message.Value = bld_project;// current_project;
            message.Time = DateTime.Now;
            // _eventAggregator.GetEvent<MessageConveyEvent>().Publish(message);
            var navParam = new NavigationParameters();
            //   navParam.Add("project", current_project);
            //  _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, "ProjectExplorerView", navParam);

            navParam.Add("bld_project", bld_project);
            _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, "ProjectExplorerView", navParam);

            //    Projects.Add(current_project);

        }

        private void LoadProjectFomDB()
        {
            AllProjectsContext = new bldProjectsGroup(_buildingUnitsRepository.Projects.GetAllAsync());
            EventMessage message = new EventMessage();
            bldProject project = new bldProject();
            CoreFunctions.SelectElementFromCollectionWhithDialog<bldProjectsGroup, bldProject>(AllProjectsContext,
                _dialogService, (result) =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         project = result.Parameters.GetValue<bldProject>("element"); //Получаем проект из диалогового окна выбора проектов
                         bldProject find_project = AllProjects.Where(pr => pr.Id == project.Id).FirstOrDefault(); //Если проект уже есть в списке загруженных 
                         if (find_project != null)//Спрашиваем загрузить ли его снова с заменой
                         {
                             CoreFunctions.ConfirmActionOnElementDialog<bldProject>(find_project,
                              "загрузить снова", "стротельный проект",
                              "Загузить",
                               "Не загузружать",
                               "Отмена", (result) =>
                             {
                                 if (result.Result == ButtonResult.Yes)
                                 {
                                     bldProject project_pointer = AllProjects.Where(pr => pr.Id == project.Id).FirstOrDefault();
                                     project_pointer = project;
                                 }

                             }, _dialogService);
                         }
                         else
                         {
                             AllProjects.Add(project);
                         }

                         //  var navParam = new NavigationParameters();
                         //  navParam.Add("bld_project", project);
                         //  _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, "ProjectExplorerView", navParam);
                     }
                 }, typeof(SelectProjectFromCollectionDialogView).Name,
                "Выберете проект для загрузки",
                "Форма для выбора проекта для загзузки из базы данных."
                , "Перечень проектов");

            foreach (bldProject prj in AllProjects)
            {
                var navParam = new NavigationParameters();
                navParam.Add("bld_project", prj);
                _regionManager.RequestNavigate(RegionNames.SolutionExplorerRegion, typeof(ProjectExplorerView).Name, navParam);
            }

        }
        private void OnGetMessage(EventMessage eventMessage)
        {
            switch (eventMessage.ParameterName)
            {
                case "Command":
                    {
                        string[] command_str = eventMessage.Value.ToString().Split(" ");
                        string command_name = command_str[0];
                        switch (command_name)
                        {
                            case "GetRibbonGroupEntity":
                                {
                                    EventMessage requestEventMessage = new EventMessage();
                                    requestEventMessage.From = CURRENT_MODULE_ID;
                                    requestEventMessage.To = eventMessage.From;
                                    requestEventMessage.ParameterName = "RibbonGroup";
                                    //    requestEventMessage.Value = ;
                                    //   _eventAggregator.GetEvent<MessageConveyEvent>().Publish(requestEventMessage);
                                }
                                break;
                        }

                    }
                    break;
            }
        }


    }

}