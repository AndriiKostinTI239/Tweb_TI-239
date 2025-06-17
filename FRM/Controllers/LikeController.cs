// FRM.Controllers/LikeController.cs

// --- НЕОБХОДИМЫЕ USING ДИРЕКТИВЫ ---
using FRM.BuisnessLogic.Services;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

// --- ОБЪЯВЛЕНИЕ NAMESPACE ---
namespace FRM.Controllers
{
    [Authorize] // Лайкать могут только авторизованные пользователи
    public class LikeController : BaseController // Наследуемся от BaseController
    {
        private readonly ILikeService _likeService;

        public LikeController()
        {
            // Ручное создание зависимостей
            _likeService = new LikeService(new AppDbContext());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Toggle(Guid commentId)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                var newLikeCount = await _likeService.ToggleLikeAsync(commentId, userId);
                return Json(new { success = true, likeCount = newLikeCount });
            }
            catch (Exception ex)
            {
                // В реальном проекте здесь должно быть логирование ошибки
                // logger.Error(ex, "Ошибка при переключении лайка");
                return Json(new { success = false, message = "Произошла ошибка" });
            }
        }
    }
}