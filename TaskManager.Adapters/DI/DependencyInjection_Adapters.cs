using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Adapters.Adapters.Messaging;
using TaskManager.Adapters.Adapters.ReadServices;
using TaskManager.Adapters.Adapters.Space;
using TaskManager.Adapters.Adapters.Task;
using TaskManager.Adapters.Adapters.TaskCategory;
using TaskManager.Adapters.Adapters.User;
using TaskManager.Adapters.Auth;
using TaskManager.Adapters.Caching;
using TaskManager.Adapters.ExternalServices.AI;
using TaskManager.Adapters.ExternalServices.Email;
using TaskManager.Adapters.ExternalServices.Messaging;
using TaskManager.Adapters.Persistence;
using TaskManager.Adapters.Security;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Emails;
using TaskManager.Core.Ports.Messaging;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;

namespace TaskManager.Adapters.DI
{
    public static class DependencyInjection_Adapters
    {
        public static IServiceCollection AddAdapters(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DbContextTaskManager>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Security
            services.AddScoped<ICurrentUserPort, HttpCurrentUserAdapter>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtGeneratorPort, JwtGenerator>();

            //user
            services.AddScoped<ICreateUserPort, CreateUserAdapter>();
            services.AddScoped<ILoginUserPort, LoginUserAdapter>();
            services.AddScoped<IUpdateUserPasswordPort, UpdateUserPasswordAdapter>();
            services.AddScoped<IDeleteUserPort, DeleteUserAdapter>();
            services.AddScoped<IGetDataUserProfilePort, GetDataUserProfileAdapter>();

            //Task
            services.AddScoped<ICreateTaskPort, CreateTaskAdapter>();
            services.AddScoped<IUpdateTaskDetailsPort, UpdateTaskDetailsAdapter>();
            services.AddScoped<IDeleteTaskPort, DeleteTaskAdapter>();
            services.AddScoped<IGetTaskByIdPort, GetTaskByIdAdapter>();
            services.AddScoped<ISearchTaskPort, SearchTaskAdapter>();

            //TaskCategory
            services.AddScoped<ICreateTaskCategoryPort, CreateTaskCategoryAdapter>();
            services.AddScoped<IUpdateTaskCategoryPort, UpdateTaskCategoryAdapter>();
            services.AddScoped<IDeleteTaskCategoryPort, DeleteTaskCategoryAdapter>();
            services.AddScoped<IGetAllTaskCategoryPort, GetAllTaskCategoryPort>();

            //Space
            services.AddScoped<ICreateSpacePort, CreateSpaceAdapter>();
            services.AddScoped<IDeleteSpacePort, DeleteSpaceAdapter>();
            services.AddScoped<IGetSpaceDataSideBarPort, GetSpaceDataSideBarAdapter>();
            services.AddScoped<IGetSpaceDetailsByIdPort, GetSpaceDetailsByIdAdapter>();
            services.AddScoped<IAddMembersSpacePort, AddMembersSpaceAdapter>();
            services.AddScoped<IRemoveMembersSpacePort, RemoveMembersSpaceAdapter>();
            services.AddScoped<ILeaveSpacePort, LeaveSpaceAdapter>();
            services.AddScoped<IUpdateSpacePort, UpdateSpaceAdapter>();

            //Helpers
            services.AddScoped<IUserQueryPort, UserQueryAdapter>();
            services.AddScoped<ISpaceMembershipQueryPort, SpaceMembershipQueryAdapter>();
            services.AddScoped<IGetUsersBySpaceIdPort, GetUsersBySpaceIdAdapter>();
            services.AddScoped<IGetAllTasksBySpaceIdPort, GetAllTasksBySpaceIdAdapter>();

            services.AddScoped<IEmailSenderPort, EmailSenderAdapter>();
            services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));

            services.AddHostedService<TwoFactorEmailConsumer>();

            services.AddSingleton<RabbitMqConnectionProvider>();
            services.AddScoped<IMessagePublisherPort, RabbitMqPublisherAdapter>();

            services.AddScoped<ICachingPort, CachingService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = 
                configuration["Redis:ConnectionString"] ?? "localhost:6379";

                options.InstanceName =
                    configuration["Redis:InstanceName"] ?? "TaskManager";
            });

            services.Configure<RabbitMqSettings>(
                configuration.GetSection("RabbitMq"));

            services.AddHttpClient<IOllamaProviderPort, OllamaProviderAdapter>(client =>
            {
                var baseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434/";
                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}