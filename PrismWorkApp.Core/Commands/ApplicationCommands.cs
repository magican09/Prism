namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
        NotifyCompositeCommand UnDoCommand { get; }
        NotifyCompositeCommand ReDoCommand { get; }
        NotifyCompositeCommand CreateWorkCommand { get; }
        NotifyCompositeCommand CreateWorkFromTemplateCommand { get; }
        NotifyCompositeCommand DeleteWorkCommand { get; }
        NotifyCompositeCommand MoveWorkCommand { get; }
        NotifyCompositeCommand AddWorkCommand { get; }
        NotifyCompositeCommand SaveExecutiveDocumentsCommand { get; }

    }
    public class ApplicationCommands : IApplicationCommands
    {
        private NotifyCompositeCommand _saveAllCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand SaveAllCommand
        {
            get { return _saveAllCommand; }
        }
        private NotifyCompositeCommand _unDoCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand UnDoCommand
        {
            get { return _unDoCommand; }
        }
        private NotifyCompositeCommand _reDoCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand ReDoCommand
        {
            get { return _reDoCommand; }
        }
        private NotifyCompositeCommand _createWorkCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand CreateWorkCommand
        {
            get { return _createWorkCommand; }
        }
        private NotifyCompositeCommand _createWorkFromTemplateCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand CreateWorkFromTemplateCommand
        {
            get { return _createWorkFromTemplateCommand; }
        }
        private NotifyCompositeCommand _deleteWorkCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand DeleteWorkCommand
        {
            get { return _deleteWorkCommand; }
        }
        //private NotifyCompositeCommand _saveExecutionDocumentationCommand = new NotifyCompositeCommand();
        //public NotifyCompositeCommand SaveExecutionDocumentationCommand
        //{
        //    get { return _saveExecutionDocumentationCommand; }
        //}
        private NotifyCompositeCommand _moveWorkCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand MoveWorkCommand
        {
            get { return _moveWorkCommand; }
        }
        private NotifyCompositeCommand _addWorkCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand AddWorkCommand
        {
            get { return _addWorkCommand; }
        }
        private NotifyCompositeCommand _saveExecutiveDocumentsCommand = new NotifyCompositeCommand(true);
        public NotifyCompositeCommand SaveExecutiveDocumentsCommand
        {
            get { return _saveExecutiveDocumentsCommand; }
        }
    }



}