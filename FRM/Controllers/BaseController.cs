// FRM.Controllers/BaseController.cs
using FRM.BuisnessLogic.Services;
using FRM.Core.Interfaces.Services; // Этот using нужен для ILayoutService
using FRM.Domain;
using FRM.Domain.Repositories;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace FRM.Controllers
{
    // Наследуемся от обычного Controller
    public abstract class BaseController : Controller
    {
        // Поле для хранения сервиса
        private readonly ILayoutService _layoutService;

        protected BaseController()
        {
            // Создаем все зависимости здесь
            var context = new AppDbContext();
            var userRepo = new UserRepository(context);
            var threadRepo = new ThreadRepository(context);
            _layoutService = new LayoutService(userRepo, threadRepo);
        }

        // Используем простой, синхронный OnActionExecuting
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User != null && User.Identity.IsAuthenticated)
            {
                var userId = GetCurrentUserId();
                if (userId != Guid.Empty)
                {
                    // Вызываем СИНХРОННЫЙ метод
                    var layoutUserData = _layoutService.GetLayoutUserData(userId);
                    ViewBag.LayoutUserData = layoutUserData;
                }
            }
        }

        protected Guid GetCurrentUserId()
        {
            var formsIdentity = User.Identity as FormsIdentity;
            if (formsIdentity == null || !formsIdentity.IsAuthenticated)
            {
                return Guid.Empty;
            }
            var ticket = formsIdentity.Ticket;
            if (ticket == null) return Guid.Empty;
            var userData = ticket.UserData;
            if (string.IsNullOrEmpty(userData)) return Guid.Empty;

            var userDataParts = userData.Split('|');
            if (userDataParts.Length > 0 && Guid.TryParse(userDataParts[0], out Guid userId))
            {
                return userId;
            }
            return Guid.Empty;
        }
    }
}