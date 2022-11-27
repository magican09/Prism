namespace PrismWorkApp.OpenWorkLib.Data.Service.UnDoReDo
{
    public delegate void UnDoReDoCommandCreateEventHandler(object sender, UnDoReDoCommandCreateEventsArgs e);

    public class UnDoReDoCommandCreateEventsArgs
    {
        public string CommandName { get; set; }
        public IUnDoRedoCommand Command { get; set; }
        public UnDoReDoCommandCreateEventsArgs(IUnDoRedoCommand command, string command_name = "")
        {
            Command = command;
            CommandName = command_name;
        }
    }
}
