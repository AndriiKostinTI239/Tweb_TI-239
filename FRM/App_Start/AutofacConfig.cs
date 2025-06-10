using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using FRM.BuisnessLogic.Services; // <-- Путь к вашему ChatService
using FRM.Core.Interfaces;       // <-- Путь к вашему IChatService
using FRM.Domain;                // <-- Путь к вашему AppDbContext
using Microsoft.AspNet.SignalR;
using System.Reflection;
using System.Web.Mvc;

namespace FRM
{
    public class AutofacConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            // 1. Получаем текущую сборку (наш проект FRM)
            var assembly = Assembly.GetExecutingAssembly();

            // 2. Регистрируем наш контекст базы данных (AppDbContext)
            // InstancePerRequest означает, что для каждого веб-запроса будет создан ОДИН экземпляр AppDbContext.
            // Это стандартная и правильная практика для Entity Framework в вебе.
            builder.RegisterType<AppDbContext>().InstancePerLifetimeScope();

            // 3. Регистрируем наш сервис
            // "Когда кто-то (контроллер или хаб) попросит IChatService, создай для него ChatService"
            builder.RegisterType<ChatService>().As<IChatService>().InstancePerLifetimeScope();

            // 4. Регистрируем ВСЕ контроллеры в нашем проекте FRM
            // Autofac сам найдет ChatController и передаст в его конструктор IChatService.
            builder.RegisterControllers(assembly);

            // 5. Регистрируем ВСЕ хабы SignalR в нашем проекте FRM
            // Autofac сам найдет ChatHub и передаст в его конструктор IChatService.
            builder.RegisterHubs(assembly);

            // 6. Собираем контейнер
            var container = builder.Build();

            // 7. Устанавливаем этот контейнер как основной распознаватель зависимостей для MVC
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));

            // 8. Устанавливаем этот контейнер как основной распознаватель зависимостей для SignalR
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);

        }
    }
}