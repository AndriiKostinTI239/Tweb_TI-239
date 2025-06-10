using FRM.Core.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FRM.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<ActionResult> Index()
        {
            // Получаем историю сообщений из сервиса
            var messageHistory = await _chatService.GetMessageHistoryAsync();
            return View(messageHistory);
        }
    }
}