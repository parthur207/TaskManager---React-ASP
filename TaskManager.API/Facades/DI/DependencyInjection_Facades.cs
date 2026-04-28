using Microsoft.Extensions.DependencyInjection;
using TaskManager.API.Facades;

namespace TaskManager.API.Facades.DI
{
    public static class DependencyInjection_Facades
    {
        public static IServiceCollection AddFacades(this IServiceCollection services)
        {
            services.AddScoped<UserUseCaseFacade>();
            services.AddScoped<TaskUseCaseFacade>();
            services.AddScoped<TaskCategoryUseCaseFacade>();
            services.AddScoped<SpaceUseCaseFacade>();
            return services;
        }
    }
}