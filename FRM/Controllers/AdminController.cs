// FRM/Controllers/AdminController.cs

using FRM.Core.Interfaces.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

[System.Web.Mvc.Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // GET: Admin
    public async Task<ActionResult> Index()
    {
        // Получаем СПИСОК пользователей
        var users = await _adminService.GetAllUsersAsync();

        // Передаем в представление именно СПИСОК
        return View(users);
    }
    // --- ДОБАВИТЬ ЭТОТ ACTION ---
    // GET: Admin/UserProfile/{id}
    [HttpGet]
    public async Task<ActionResult> UserProfile(Guid id)
    {
        var userProfile = await _adminService.GetUserProfileAsync(id);
        if (userProfile == null)
        {
            return HttpNotFound(); // Если пользователя с таким ID нет
        }
        return View(userProfile);
    }

    // POST: Admin/Ban/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Ban(Guid id)
    {
        await _adminService.BanUserAsync(id);
        // Перенаправляем обратно на ту страницу, с которой пришли
        return Redirect(Request.UrlReferrer.ToString());
    }

    // POST: Admin/Unban/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Unban(Guid id)
    {
        await _adminService.UnbanUserAsync(id);
        // Перенаправляем обратно на ту страницу, с которой пришли
        return Redirect(Request.UrlReferrer.ToString());
    }
}