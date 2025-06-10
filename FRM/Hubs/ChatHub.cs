using FRM.Core.Entities;
using FRM.Core.Interfaces;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace FRM.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string userName, string messageText)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(messageText))
                return;

            var message = new Message
            {
                UserName = userName,
                Text = messageText
            };

            // 1. Сохраняем сообщение в БД
            await _chatService.SaveMessageAsync(message);

            // 2. Рассылаем сообщение всем подключенным клиентам,
            // вызывая у них клиентский метод addNewMessageToPage
            Clients.All.addNewMessageToPage(message.UserName, message.Text, message.Timestamp.ToString("g"));
        }
    }
}