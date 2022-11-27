using Prism.Services.Dialogs;
using System.Windows;

namespace PrismWorkApp.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class CommonDialogWindow : Window, IDialogWindow
    {
        public CommonDialogWindow()
        {
            InitializeComponent();
        }

        private IDialogResult _result;

        public IDialogResult Result
        {
            get { return _result; }
            set { _result = value; }
        }



        /*   
           private object _content;
           public object Content
           {
               get { return _content; }
               set { _content = value; }
           }
        */
        private Window _owner;
        public Window Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private object _dataContext;
        public object DataContext
        {
            get { return _dataContext; }
            set { _dataContext = value; }
        }


        private Style _style;

        public Style Style
        {
            get { return _style; }
            set { _style = value; }
        }

        /*

           private  event RoutedEventHandler _loaded;
           public event RoutedEventHandler Loaded
           {
               add
               {
                   _loaded += value;
               }
               remove
               {
                   _loaded -= value;
               }
           }
           public event CancelEventHandler _cancelEventHandler;
           public event CancelEventHandler CancelEventHandler
           {
               add
               {
                   _cancelEventHandler += value;
               }
               remove
               {
                   _cancelEventHandler -= value;
               }
           }
           public event CancelEventHandler _closing;
           public event CancelEventHandler Closing
           {
               add
               {
                   _closing += value;
               }
               remove
               {
                   _closing -= value;
               }
           }

           public void Close()
           {

           }

           public void Show()
           {
               base.Show();  
           }

           public bool? ShowDialog()
           {
               base.ShowDialog();
               return true;
           }
           */
    }
}
