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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ConvertersViewModel : LocalBindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public NotifyCommand LoadProjectFromExcelCommand { get; private set; }
        public NotifyCommand CreateProjectStructureCommand { get; private set; }
        public NotifyCommand LoadProjectFromDBCommand { get; private set; }
        public NotifyCommand SaveDataToDBCommand { get; private set; }
        public NotifyCommand CreateAOSRCommand { get; private set; }
        public NotifyCommand UnDoCommand { get; private set; }

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

       


        public ConvertersViewModel(IRegionManager regionManager, IModulesContext modulesContext, IEventAggregator eventAggregator,
                                    IBuildingUnitsRepository buildingUnitsRepository, IDialogService dialogService, IApplicationCommands applicationCommands,
                                    IUnDoReDoSystem unDoReDo)
        {

         //   UnDoReDo = new UnDoReDoSystem(Id);

            _regionManager = regionManager;
            var quickAccessTollBar = new QuickAccessToolBarView();
            quickAccessTollBar.Items.Add(new QuickAccessToolBar());
            quickAccessTollBar.DataContext = this;
            _regionManager.Regions[RegionNames.RibbonQuickAccessToolBarRegion].Add(quickAccessTollBar);//Доабвяем кнопку сохраниять все на панель панель быстрого вызова
            ModulesContext = modulesContext;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _buildingUnitsRepository = buildingUnitsRepository;
            ApplicationCommands = applicationCommands;
            ModuleInfo = ModulesContext.ModulesInfoData.Where(mi => mi.Id == CURRENT_MODULE_ID).FirstOrDefault();
 
            LoadProjectFromExcelCommand = new NotifyCommand(LoadProjectFromExcel, CanLoadAllProjects);
            LoadProjectFromDBCommand = new NotifyCommand(LoadProjectFomDB, CanLoadProjectFromDb);
            SaveDataToDBCommand = new NotifyCommand(SaveDataToDB, CanSaveDataToDB)
                .ObservesProperty(() => AllChangesIsDone);
            AllChangesIsDone = true;
            ApplicationCommands.SaveAllCommand.SetLastCommand(SaveDataToDBCommand);
            }
     

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

            var pr_var = _buildingUnitsRepository.Projects.GetAll().FirstOrDefault();
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
            AllProjectsContext = new bldProjectsGroup(_buildingUnitsRepository.Projects.GetProjectsAsync());
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