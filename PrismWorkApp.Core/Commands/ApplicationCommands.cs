namespace PrismWorkApp.Core.Commands
{
    public interface IApplicationCommands
    {
        NotifyCompositeCommand SaveAllCommand { get; }
        NotifyCompositeCommand UnDoCommand { get; }
        NotifyCompositeCommand ReDoCommand { get; }
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
        // public CompositeCommand LoadProjectFromExcell { get; } = new CompositeCommand();
    }



}