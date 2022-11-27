using PrismWorkApp.Services.Interfaces;

namespace PrismWorkApp.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {


            return "Hello from the Message Service";

        }
    }
}
