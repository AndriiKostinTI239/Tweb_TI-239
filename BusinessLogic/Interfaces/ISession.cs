using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Добавляем using для типов данных, которые будут использоваться в методах интерфейса
// Предполагается, что классы User и UserLoginResp находятся (или будут)
// в проекте labTW.Domain, например, в папке Entities.
using labTW.Domain.Entities;
using labTW.Domain.Entities.Users; // Или где у вас определен UserLoginResp
using labTW.BusinessLogic.Interfaces; // Правильный namespace для ISession
// Возможно, понадобятся другие using для Domain и т.д.
using System.Web; // Если используете HttpContext
namespace labTW.BusinessLogic.Interfaces // Убедитесь, что пространство имен совпадает с папкой
{
    public interface ISession
    {
        /// <summary>
        /// Выполняет аутентификацию пользователя по имени и паролю.
        /// </summary>
        /// <param name="username">Имя пользователя (или credential).</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Объект UserLoginResp с результатом операции.</returns>
        UserLoginResp UserLogin(string username, string password);

        /// <summary>
        /// Завершает текущую сессию пользователя (выход).
        /// </summary>
        void UserLogout();

        /// <summary>
        /// Получает данные текущего аутентифицированного пользователя.
        /// </summary>
        /// <returns>Объект User, если пользователь вошел, иначе null.</returns>
        User GetCurrentUser(); // Убедитесь, что класс User определен в labTW.Domain.Entities

        /// <summary>
        /// Проверяет, аутентифицирован ли пользователь в данный момент.
        /// </summary>
        /// <returns>True, если пользователь вошел, иначе false.</returns>
        bool IsUserLoggedIn();

        // Сюда можно добавить другие методы, если они понадобятся для работы с сессией
    }

    // --- Вспомогательные классы (DTO - Data Transfer Objects) ---
    // Эти классы ЛУЧШЕ разместить в проекте labTW.Domain,
    // но для простоты можно временно оставить здесь или создать их в Domain.

    /// <summary>
    /// Класс для передачи результата операции входа.
    /// </summary>
    public class UserLoginResp
    {
        public bool LoginSuccessful { get; set; }
        public string Message { get; set; }
        public User UserData { get; set; } // Ссылка на данные пользователя, если вход успешен
    }

    /*
    // Убедитесь, что у вас есть класс User в проекте labTW.Domain.Entities
    // Пример определения, если его нет:
    namespace labTW.Domain.Entities
    {
        public class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            // Пароль (хеш+соль) не должен быть здесь, он для хранения в БД
            // public string PasswordHash { get; set; }
            // public string PasswordSalt { get; set; }
        }
    }
    */
}
