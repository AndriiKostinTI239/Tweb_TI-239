// FRM/Security/CustomRoleProvider.cs

using System;
using System.Linq;
using System.Web;
using System.Web.Security;

// Пространство имен будет автоматически FRM.Security, потому что файл лежит в папке Security
namespace FRM.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        // Этот метод проверяет, принадлежит ли пользователь к определенной роли
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Any(r => r.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        // Этот метод получает все роли для текущего залогиненного пользователя
        public override string[] GetRolesForUser(string username)
        {
            // Получаем cookie аутентификации из текущего запроса
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                // Если cookie нет, значит пользователь не аутентифицирован
                return new string[0];
            }

            try
            {
                // Расшифровываем "билет" аутентификации из cookie
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || string.IsNullOrWhiteSpace(ticket.UserData))
                {
                    return new string[0];
                }

                // Мы храним данные в формате "GUID|Role"
                // Разбиваем строку по разделителю '|'
                var userDataParts = ticket.UserData.Split('|');
                if (userDataParts.Length < 2)
                {
                    // Если формат неверный, возвращаем пустой массив ролей
                    return new string[0];
                }

                // Вторая часть (индекс 1) - это наша роль
                var userRole = userDataParts[1];
                return new[] { userRole };
            }
            catch
            {
                // Если произошла ошибка при расшифровке, считаем, что ролей нет
                return new string[0];
            }
        }

        #region Not Implemented Methods
        // Остальные методы интерфейса RoleProvider нам не нужны для данной задачи,
        // поэтому мы просто оставляем их нереализованными.
        public override string ApplicationName { get; set; }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames) => throw new NotImplementedException();
        public override void CreateRole(string roleName) => throw new NotImplementedException();
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) => throw new NotImplementedException();
        public override string[] FindUsersInRole(string roleName, string usernameToMatch) => throw new NotImplementedException();
        public override string[] GetAllRoles() => throw new NotImplementedException();
        public override string[] GetUsersInRole(string roleName) => throw new NotImplementedException();
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) => throw new NotImplementedException();
        public override bool RoleExists(string roleName) => throw new NotImplementedException();
        #endregion
    }
}