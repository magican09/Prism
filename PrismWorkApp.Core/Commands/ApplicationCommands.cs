namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
        NotifyCompositeCommand UnDoCommand { get; }
        NotifyCompositeCommand ReDoCommand { get; }
        NotifyCompositeCommand CreateNewCommand { get; }
        NotifyCompositeCommand CreateBasedOnCommand { get; }
        NotifyCompositeCommand DeleteCommand { get; }
        NotifyCompositeCommand MoveToCommand { get; }
        NotifyCompositeCommand MoveFromCommand { get; }
        NotifyCompositeCommand AddCommand { get; }

        //NotifyCompositeCommand CreateWorkCommand { get; }
        //NotifyCompositeCommand CreateWorkFromTemplateCommand { get; }
        //NotifyCompositeCommand DeleteWorkCommand { get; }
        //NotifyCompositeCommand MoveWorkCommand { get; }
        //NotifyCompositeCommand AddWorkCommand { get; }

        NotifyCompositeCommand SaveExecutiveDocumentsCommand { get; }
        NotifyCompositeCommand LoadMaterialsFromAccessCommand { get; }
        NotifyCompositeCommand OpenAppSettingsDialogCommand { get; }


    }
    public class ApplicationCommands : IApplicationCommands
    {
        private NotifyCompositeCommand _saveAllCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand SaveAllCommand
        {
            get { return _saveAllCommand; }
        }
        private NotifyCompositeCommand _unDoCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand UnDoCommand
        {
            get { return _unDoCommand; }
        }
        private NotifyCompositeCommand _reDoCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand ReDoCommand
        {
            get { return _reDoCommand; }
        }
        //private NotifyCompositeCommand _createWorkCommand = new NotifyCompositeCommand(true);
        //public NotifyCompositeCommand CreateWorkCommand
        //{
        //    get { return _createWorkCommand; }
        //}
        //private NotifyCompositeCommand _createWorkFromTemplateCommand = new NotifyCompositeCommand(true);
        //public NotifyCompositeCommand CreateWorkFromTemplateCommand
        //{
        //    get { return _createWorkFromTemplateCommand; }
        //}
        //private NotifyCompositeCommand _deleteWorkCommand = new NotifyCompositeCommand(true);
        //public NotifyCompositeCommand DeleteWorkCommand
        //{
        //    get { return _deleteWorkCommand; }
        //}
  
        private NotifyCompositeCommand _moveWorkCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand MoveWorkCommand
        {
            get { return _moveWorkCommand; }
        }
        private NotifyCompositeCommand _addWorkCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddWorkCommand
        {
            get { return _addWorkCommand; }
        }
      
        private NotifyCompositeCommand _saveExecutiveDocumentsCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand SaveExecutiveDocumentsCommand
        {
            get { return _saveExecutiveDocumentsCommand; }
        }
        private NotifyCompositeCommand _loadMaterialsFromAccessCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand LoadMaterialsFromAccessCommand
        {
            get { return _loadMaterialsFromAccessCommand; }
        }
        private NotifyCompositeCommand _openAppSettingsDialogCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand OpenAppSettingsDialogCommand
        {
            get { return _openAppSettingsDialogCommand; }
        }
      
        private NotifyCompositeCommand _createNewCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand CreateNewCommand
        {
            get { return _createNewCommand; }
        }

        private NotifyCompositeCommand _createBasedOnCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand CreateBasedOnCommand
        {
            get { return _createBasedOnCommand; }
        }

        private NotifyCompositeCommand _deleteCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand DeleteCommand
        {
            get { return _deleteCommand; }
        }
   
        private NotifyCompositeCommand _moveToCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand MoveToCommand
        {
            get { return _moveToCommand; }
        }
        private NotifyCompositeCommand _moveFromCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand MoveFromCommand
        {
            get { return _moveFromCommand; }
        }
        private NotifyCompositeCommand _addCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand AddCommand
        {
            get { return _addCommand; }
        }
      

    }



}