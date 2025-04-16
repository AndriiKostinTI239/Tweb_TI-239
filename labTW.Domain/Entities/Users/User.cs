using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labTW.Domain.Entities.Users
{
   
        /// <summary>
        /// Представляет пользователя системы.
        /// </summary>
        public class User
        {
            /// <summary>
            /// Уникальный идентификатор пользователя.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Имя пользователя (логин). Используется для входа.
            /// </summary>
            public string Username { get; set; } // Соответствует 'Credential' из ULoginData

            /// <summary>
            /// Адрес электронной почты пользователя.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Роль пользователя в системе (например, "Admin", "User", "Moderator").
            /// </summary>
            public string Role { get; set; }

            /// <summary>
            /// Полное имя пользователя (опционально).
            /// </summary>
            public string FullName { get; set; }

            /// <summary>
            /// Дата последней активности (опционально).
            /// </summary>
            public System.DateTime? LastLoginDate { get; set; }

            /// <summary>
            /// Статус активности пользователя (опционально).
            /// </summary>
            public bool IsActive { get; set; }

            // Важно: НЕ ДОБАВЛЯЙТЕ сюда свойства для пароля (Password, PasswordHash, PasswordSalt).
            // Пароль (или его хэш) должен оставаться на уровне хранения данных (база данных)
            // и не передаваться без необходимости между слоями, особенно в объект,
            // который может быть сохранен в сессии или передан на уровень представления.
        }
    
}
