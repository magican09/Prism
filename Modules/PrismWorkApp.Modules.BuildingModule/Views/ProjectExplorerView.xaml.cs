using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrismWorkApp.Modules.BuildingModule.Views
{
    /// <summary>
    /// Interaction logic for ProjectExplorerView.xaml
    /// </summary>
    public partial class ProjectExplorerView : UserControl
    {
        public ProjectExplorerView()
        {
            InitializeComponent();
            treeViewProjectSolution.DataContextChanged += new DependencyPropertyChangedEventHandler(TreeViewItemsLoadEvent);
            treeViewProjectSolution.Loaded += TreeViewProjectSolution_Loaded;
            treeViewProjectSolution.SourceUpdated += TreeViewProjectSolution_SourceUpdated;
            treeViewProjectSolution.TargetUpdated += TreeViewProjectSolution_TargetUpdated;

        }

        private void TreeViewProjectSolution_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TreeViewProjectSolution_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void TreeViewProjectSolution_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TreeViewItemsLoadEvent(object sender, DependencyPropertyChangedEventArgs e)
        {
            bldProjectsGroup projects = treeViewProjectSolution.DataContext as bldProjectsGroup;
        }

        private void trw_Products_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();
            DirectoryInfo dir;
            if (item.Tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)item.Tag;
                dir = drive.RootDirectory;
            }
            else dir = (DirectoryInfo)item.Tag;
            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Tag = subDir;
                    newItem.Header = subDir.ToString();
                    newItem.Items.Add("*");
                    item.Items.Add(newItem);
                }
            }
            catch
            { }
        }

        private void trw_Products_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void TreeViewItemsLoad()
        {
            bldProjectsGroup projects = treeViewProjectSolution.DataContext as bldProjectsGroup;

        }

    }
}
