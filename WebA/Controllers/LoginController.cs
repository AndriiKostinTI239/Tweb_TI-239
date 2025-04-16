using labTW.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using labTW.BusinessLogic.Interfaces; // Правильный namespace для ISession
using labTW.Domain.Entities.Users;

namespace WebA.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISession _session;

        public LoginController()
        {
            var bl = new labTW.BusinessLogic.BusinessLogic();
            _session = bl.GetSession();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ULoginData login)
        {
            if (ModelState.IsValid)
            {
                var loginResponse = _session.UserLogin(login.Credential, login.Password); // Переименовал переменную для ясности

                // 2. Проверяем свойство LoginSuccessful внутри объекта ответа
                if (loginResponse.LoginSuccessful)
                {
                    // Вход успешен
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Вход не удался
                    // 3. Используем сообщение из ответа BLL, если оно есть
                    ModelState.AddModelError("", loginResponse.Message ?? "Неверные учетные данные");
                }
            }

            // Если модель не валидна или вход не удался, возвращаем ту же View
            return View(login);
        }
        }
    }
