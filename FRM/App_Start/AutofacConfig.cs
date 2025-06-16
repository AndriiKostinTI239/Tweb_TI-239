using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using FRM.BuisnessLogic.Helper;
using FRM.BuisnessLogic.Services;
using FRM.Core.Interfaces;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using FRM.Domain.Repositories;
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

            // Получаем сборку, где находятся контроллеры и хабы (наш проект FRM)
            var assembly = Assembly.GetExecutingAssembly();

            // --- ШАГ 1: РЕГИСТРАЦИЯ ВСЕХ КОМПОНЕНТОВ ---

            // Регистрируем контекст базы данных. InstancePerLifetimeScope() - хороший выбор.
            builder.RegisterType<AppDbContext>().InstancePerLifetimeScope();

            // Регистрируем все репозитории
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ThreadRepository>().As<IThreadRepository>().InstancePerLifetimeScope();
            // Добавьте сюда другие ваши репозитории, если они есть

            // Регистрируем все сервисы
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<AdminService>().As<IAdminService>().InstancePerLifetimeScope(); // <-- Ваша регистрация
            builder.RegisterType<ChatService>().As<IChatService>().InstancePerLifetimeScope();
            builder.RegisterType<LayoutService>().As<ILayoutService>().InstancePerLifetimeScope();
            builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerLifetimeScope();
            builder.RegisterType<ThreadService>().As<IThreadService>().InstancePerLifetimeScope();
            // Добавьте сюда другие ваши сервисы

            // Регистрируем хелперы
            builder.RegisterType<Hasher>().AsSelf().InstancePerLifetimeScope();

            // Регистрируем все контроллеры в сборке FRM
            builder.RegisterControllers(assembly);

            // Регистрируем все хабы SignalR в сборке FRM
            builder.RegisterHubs(assembly);


            // --- ШАГ 2: СБОРКА КОНТЕЙНЕРА ---
            // Этот шаг должен быть ПОСЛЕ всех регистраций
            var container = builder.Build();


            // --- ШАГ 3: УСТАНОВКА РАСПОЗНАВАТЕЛЕЙ ЗАВИСИМОСТЕЙ ---
            // Этот шаг должен быть ПОСЛЕ сборки контейнера

            // Устанавливаем распознаватель для MVC
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));

            // Устанавливаем распознаватель для SignalR
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
        }
    }
}