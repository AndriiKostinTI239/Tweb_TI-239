using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // Для FormsAuthentication
using System.Web.Mvc;
using System.Web.Security; // Для FormsAuthentication
using labTW.BusinessLogic; // Для BusinessLogic фасада
using labTW.BusinessLogic.Interfaces; // Для IAccountService (нужно создать)
using labTW.Domain.ViewModels; // Для RegisterViewModel
// using labTW.Domain.Entities; // Для User (если сервис возвращает)

namespace WebA.Controllers
{
    public class AccountController : Controller
    {
        // --- Зависимости ---
        // В идеале, сюда нужно внедрять IAccountService через Dependency Injection.
        // Пока используем временный вариант с прямым созданием BusinessLogic фасада.

        // private readonly IAccountService _accountService; // <-- Идеальный вариант через DI
      
        // --- Действие для отображения формы регистрации ---
        // GET: /Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            // Просто отображаем представление Register.cshtml
            // Макет (_Layout_Blank.cshtml) будет указан внутри самого представления.
            return View();
        }

        // --- Действие для обработки данных формы регистрации ---
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от CSRF
        public ActionResult Register(RegisterViewModel model) // Принимаем данные формы через ViewModel
        {
            // Проверяем валидность данных, полученных от пользователя (на основе атрибутов в RegisterViewModel)
            if (ModelState.IsValid)
            {
                // --- Вызов бизнес-логики для регистрации пользователя ---

                // !!! ЗАГЛУШКА: Замените этот блок на реальный вызов вашего сервиса регистрации !!!
                // Вам нужно создать IAccountService и AccountService в labTW.BusinessLogic
                // и вызвать метод вроде RegisterUser, который вернет результат.
                // Пример вызова:
                // var registrationResult = _accountService.RegisterUser(model.Username, model.Email, model.Password);

                // Временная заглушка для демонстрации потока:
                bool registrationSuccess = true; // Предполагаем успех
                string errorMessage = string.Empty; // Сообщение об ошибке (если есть)

                // Анализируем результат от BLL
                // if (registrationResult.Success) // Как могло бы быть с реальным сервисом
                if (registrationSuccess)
                {
                    // Регистрация прошла успешно.

                    // Автоматически аутентифицируем пользователя (создаем cookie)
                    FormsAuthentication.SetAuthCookie(model.Username, false); // false - cookie не постоянная

                    // Перенаправляем пользователя на главную страницу или другую целевую страницу
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Регистрация не удалась (например, имя пользователя или email заняты).
                    // Добавляем ошибку модели, чтобы показать ее пользователю в представлении.
                    // Используем сообщение об ошибке, полученное от BLL.
                    ModelState.AddModelError("", errorMessage ?? "Произошла ошибка при регистрации.");
                    // ModelState.AddModelError("", registrationResult.ErrorMessage); // Как могло бы быть
                }
                // Если регистрация не удалась, код дойдет до конца метода и вернет View(model)
            }

            // Если ModelState не валиден (например, пользователь не заполнил поле или пароли не совпали)
            // или если регистрация в BLL не удалась,
            // возвращаем то же самое представление Register.cshtml, передавая обратно модель (model).
            // Это нужно, чтобы пользователь увидел введенные им данные и сообщения об ошибках валидации.
            return View(model);
        }

        // Можно добавить другие действия, связанные с аккаунтом, например:
        // - Подтверждение Email
        // - Сброс пароля
        // - Управление профилем
    }
}