using labTW.BusinessLogic.Interfaces; // Правильный namespace для ISession
using labTW.Domain.Entities.Users;
using System;
using System.Deployment.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebA.Models;
using BusinessLogic.DBModel;

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
        public ActionResult Index(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                ULoginData data = new ULoginData
                {
                    Credential = login.UserName,
                    Password = login.Password,
                    LoginIP = Request.UserHostAddress,
                    LoginDateTime = DateTime.Now

                };

                var loginResponse = _session.UserLogin(login.UserName, login.Password); // Переименовал переменную для ясности

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
            return View(login);
        }

        internal UserLoginResp UserLoginAction(ULoginData data)
        {
            UDbTable user;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Username == data.Credential);
            }

            using (var db = new UserContext())
            {
                user = (from u in db.Users where u.Username == data.Credential select u).FirstOrDefault();
            }

            return null;
        }

        }
            // Если модель не валидна или вход не удался, возвращаем ту же View

    }
