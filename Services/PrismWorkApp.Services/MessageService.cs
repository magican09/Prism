using PrismWorkApp.Services.Interfaces;
using System.Threading.Tasks;

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
