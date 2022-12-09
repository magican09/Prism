namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
        NotifyCompositeCommand UnDoCommand { get; }
        NotifyCompositeCommand ReDoCommand { get; }
        NotifyCompositeCommand CreateCommand { get; }
        NotifyCompositeCommand CreateFromTemplateCommand { get; }
        NotifyCompositeCommand DeleteCommand { get; }
        NotifyCompositeCommand SaveExecutionDocumentationCommand { get; }
        NotifyCompositeCommand MoveCommand { get; }
        NotifyCompositeCommand AddCommand { get; }

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
        private NotifyCompositeCommand _createCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand CreateCommand
        {
            get { return _createCommand; }
        }
        private NotifyCompositeCommand _createFromTemplateCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand CreateFromTemplateCommand
        {
            get { return _createFromTemplateCommand; }
        }
        private NotifyCompositeCommand _deleteCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand DeleteCommand
        {
            get { return _deleteCommand; }
        }
        private NotifyCompositeCommand _saveExecutionDocumentationCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand SaveExecutionDocumentationCommand
        {
            get { return _saveExecutionDocumentationCommand; }
        }
        private NotifyCompositeCommand _moveCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand MoveCommand
        {
            get { return _moveCommand; }
        }
        private NotifyCompositeCommand _addCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand AddCommand
        {
            get { return _addCommand; }
        }
      
        
    }



}