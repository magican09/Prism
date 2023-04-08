namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate void UnDoReDoCommandCreateEventHandler(object sender, UnDoReDoCommandCreateEventsArgs e);
    /// <summary>
    /// Класс для передачи события UnDoReDoCommandCreated вызываемого после применения команды  IUnDoRedoCommand
    /// внутри объекта, реализующего IJornalable.
    /// </summary>
    public class UnDoReDoCommandCreateEventsArgs
    {
        public string CommandName { get; set; }///Название примененной команды
        public IUnDoRedoCommand Command { get; set; }///Применненая команда
        public UnDoReDoCommandCreateEventsArgs(IUnDoRedoCommand command, string command_name = "")
        {
            Command = command;
            CommandName = command_name;
        }
    }
}
