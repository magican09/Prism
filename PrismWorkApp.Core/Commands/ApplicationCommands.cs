namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
        NotifyCompositeCommand UnDoCommand { get; }
        NotifyCompositeCommand ReDoCommand { get; }
        NotifyCompositeCommand CreateNewWorkCommand { get; }
        NotifyCompositeCommand DeleteWorksCommand { get; }
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
        private NotifyCompositeCommand _createNewWorkCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand CreateNewWorkCommand
        {
            get { return _createNewWorkCommand; }
        }
        private NotifyCompositeCommand _deleteWorksCommand = new NotifyCompositeCommand();
        public NotifyCompositeCommand DeleteWorksCommand
        {
            get { return _deleteWorksCommand; }
        }
        // public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }



}