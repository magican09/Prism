using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismWorkApp.Core;
using PrismWorkApp.Core.Commands;
using PrismWorkApp.Core.Events;
using PrismWorkApp.Modules.BuildingModule.Views;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;


namespace PrismWorkApp.Modules.BuildingModule.ViewModels
{
    public class ProjectExplorerViewModel : LocalBindableBase, INotifyPropertyChanged, INavigationAware//, IConfirmNavigationRequest
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public Dictionary<string, Node> _nodeDictionary;
        public Dictionary<string, Node> NodeDictionary
        {
            get { return _nodeDictionary; }
            set { SetProperty(ref _nodeDictionary, value); OnPropertyChanged("NodeDictionary"); }
        }

        private string _title = "Менеджер проектов";
        //private Work _selectedWork;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _testText = "Тестовый текст";
        public string TestText
        {
            get { return _testText; }
            set { SetProperty(ref _testText, value); }
        }
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        //private ObservableCollection<Project> _projects;
        //public ObservableCollection<Project> Projects
        //{ get { return _projects; } set { _projects = value; OnPropertyChanged("Projects"); } }
        private bldProjectsGroup _bldprojects = new bldProjectsGroup();
        public bldProjectsGroup bld_Projects
        {
            get { return _bldprojects; }
            set { SetProperty(ref _bldprojects, value); }
        }

        //public oldNode _rootnode;
        //public oldNode RootNode { get { return _rootnode; } set { _rootnode = value; OnPropertyChanged("RootNode"); } }
        //public Work _testWork;
        //public Work TestWork { get { return _testWork; } set { _testWork = value; OnPropertyChanged("TestWork"); } }

        public ObservableCollection<Node> _nodes;
        public ObservableCollection<Node> Nodes { get { return _nodes; } set { _nodes = value; OnPropertyChanged("Nodes"); } }

        //private Project _selectedProject;
        //public Project SelectedProject { get { return _selectedProject; } set { _selectedProject = value; SentProjectCommand.RaiseCanExecuteChanged(); OnPropertyChanged("SelectedProject"); } }
        private IDialogService _dialogService;

        public ProjectExplorerViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService)
        {
            //  Projects = new ObservableCollection<Project>(); ;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _dialogService = dialogService;
            //  SentProjectCommand = new NotifyCommand(SentProject, CanSentProject);
            TreeViewItemSelectedCommand = new NotifyCommand<object>(OnTreeViewItemSelected);
            TreeViewItemExpandedCommand = new NotifyCommand<object>(onTreeViewItemExpanded);
            _eventAggregator.GetEvent<MessageConveyEvent>().Subscribe(OnGetMessage,
             ThreadOption.PublisherThread, false,
             message => message.Recipient == "ProjectExplorer");

        }

        private void onTreeViewItemExpanded(object obj)
        {

        }

        private void OnGetMessage(EventMessage message)
        {
            /*switch (message.ParameterName)
            {
                case "Project":
                    if (Projects.Where(pr => pr.Id == ((Project)message.Value).Id).FirstOrDefault() == null)
                        Projects.Add((Project)message.Value);
                    else
                    {

                    }
                    break;
            }*/
        }

        private void OnTreeViewItemSelected(object clc_node)
        {
            System.Type node_value_type;
            object clicked_node;
            if (clc_node.GetType() == typeof(Node))
            {
                node_value_type = ((Node)clc_node).Value?.GetType();
                clicked_node = ((Node)clc_node).Value;
            }
            else
            {
                node_value_type = clc_node?.GetType();
                clicked_node = clc_node;
            }
            NavigationParameters navParam = new NavigationParameters();
            switch (node_value_type?.Name)
            {
                case (nameof(bldMaterial)):
                    {
                        navParam.Add("bld_material", (new ConveyanceObject((bldMaterial)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(MaterialView).Name, navParam);
                        break;
                    }
                case (nameof(bldWork)):
                    {
                        navParam.Add("bld_work", (new ConveyanceObject((bldWork)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        navParam.Add("bld_construction", (new ConveyanceObject(((bldWork)clicked_node).bldConstruction, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        navParam.Add("bld_object", (new ConveyanceObject(((bldWork)clicked_node).bldConstruction.bldObject, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(WorkView).Name, navParam);
                        break;
                    }
                case (nameof(bldProject)):
                    {
                        //navParam.Add("project", ((bldProject)clicked_node));
                        navParam.Add("bld_project", (new ConveyanceObject((bldProject)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ProjectView).Name, navParam);
                        //  _regionManager.RequestNavigate(RegionNames.ContentRegion, "ProjectView",  NavgationCoplete);

                        break;
                    }
                case (nameof(bldParticipant)):
                    {
                        navParam.Add("bld_participant", new ConveyanceObject((bldParticipant)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ParticipantView).Name, navParam);
                        break;
                    }
                case (nameof(bldObject)):
                    {
                        navParam.Add("bld_object", new ConveyanceObject((bldObject)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ObjectView).Name, navParam);
                        break;
                    }
                case (nameof(bldConstruction)):
                    {
                        navParam.Add("bld_construction", new ConveyanceObject((bldConstruction)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ConstructionView).Name, navParam);
                        break;
                    }
                case (nameof(bldResponsibleEmployee)):
                    {
                        navParam.Add("responsible_employee", new ConveyanceObject((bldResponsibleEmployee)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ResponsibleEmployeeView).Name, navParam);
                        break;
                    }
                case (nameof(bldAOSRDocument)):
                    {
                        navParam.Add("bld_aosr_document", new ConveyanceObject((bldAOSRDocument)clicked_node, ConveyanceObjectModes.EditMode.FOR_EDIT));
                        //  navParam.Add("bld_work", (new ConveyanceObject(((bldAOSRDocument)clicked_node).bldWork, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        // navParam.Add("bld_project", (new ConveyanceObject(((bldAOSRDocument)clicked_node).bldWork.bldConstruction.bldObject.bldProject, ConveyanceObjectModes.EditMode.FOR_EDIT)));
                        _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(AOSRDocumentView).Name, navParam);
                        break;
                    }
                case (nameof(bldWorksGroup)):
                    {
                        if (((bldWorksGroup)clicked_node).Parents != null)
                        {
                            navParam.Add("bld_construction", new ConveyanceObject(((bldWorksGroup)clicked_node).Parents, ConveyanceObjectModes.EditMode.FOR_EDIT));
                            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(WorksGroupView).Name, navParam);
                        }
                        break;
                    }
                case (nameof(bldParticipantsGroup)):
                    {
                        if (((bldParticipantsGroup)clicked_node).Parents != null)
                        {
                            navParam.Add("bld_project", new ConveyanceObject(((bldParticipantsGroup)clicked_node).Parents, ConveyanceObjectModes.EditMode.FOR_EDIT));
                            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(ParticipantsGroupView).Name, navParam);
                        }
                        break;
                    }
                default:
                    break;
            }

        }

        private void NavgationCoplete(NavigationResult obj)
        {
            //      throw new NotImplementedException();
        }

        //private bool CanSentProject()
        //{
        //    return SelectedProject != null;
        //}

        public NotifyCommand SentProjectCommand { get; private set; }
        //   public NotifyCommand LoadProjectCommand { get; private set; }
        public NotifyCommand<object> TreeViewItemSelectedCommand { get; private set; }
        public NotifyCommand<object> TreeViewItemExpandedCommand { get; private set; }
        //private void SentProject()
        //{
        //    _eventAggregator.GetEvent<ProjectSentEvent>().Publish(SelectedProject);
        //}

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            bldProject bld_project = (bldProject)navigationContext.Parameters["bld_project"];
            //  if (bld_project != null && bld_Projects.Where(pr => pr.Id == bld_project.Id).FirstOrDefault() == null && bld_project != null)
            if (bld_project != null && bld_Projects.Where(pr => pr.Id == bld_project.Id).FirstOrDefault() == null)
            {
                bld_Projects.Add(bld_project);
            }
            else if (bld_project != null)
            {
                bldProject temp_prj = bld_Projects.Where(pr => pr.Id == bld_project.Id).FirstOrDefault();
                temp_prj = bld_project;
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }
    }
}
