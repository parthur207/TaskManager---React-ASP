using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Prompts;
using TaskManager.Core.UseCases.AI;
using TaskManager.Core.UseCases.AI.Interfaces;
using TaskManager.Core.UseCases.Space;
using TaskManager.Core.UseCases.Space.Interfaces;
using TaskManager.Core.UseCases.Task;
using TaskManager.Core.UseCases.Task.Interfaces;
using TaskManager.Core.UseCases.TaskCategory;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;
using TaskManager.Core.UseCases.User;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.DI
{
    public static class DependencyInjection_Core
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // Space
            services.AddScoped<ICreateSpaceUseCase, CreateSpaceUseCase>();
            services.AddScoped<IDeleteSpaceUseCase, DeleteSpaceUseCase>();
            services.AddScoped<IAddMembersSpaceUseCase, AddMembersSpaceUseCase>();
            services.AddScoped<IRemoveMembersSpaceUseCase, RemoveMembersSpaceUseCase>();
            services.AddScoped<ILeaveSpaceUseCase, LeaveSpaceUseCase>();
            services.AddScoped<IUpdateSpaceUseCase, UpdateSpaceUseCase>();
            services.AddScoped<IGetSpaceDataSideBarUseCase, GetSpaceDataSideBarUseCase>();
            services.AddScoped<IGetSpaceDetailsByIdUseCase, GetSpaceDetailsByIdUseCase>();
            services.AddScoped<IGetUsersBySpaceIdUseCase, GetUsersBySpaceIdUseCase>();

            // Task 
            services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
            services.AddScoped<IUpdateTaskDetailsUseCase, UpdateTaskDetailsUseCase>();
            services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
            services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
            services.AddScoped<ISearchTaskUseCase, SearchTaskUseCase>();

            // TaskCategory
            services.AddScoped<ICreateTaskCategoryUseCase, CreateTaskCategoryUseCase>();
            services.AddScoped<IUpdateTaskCategoryUseCase, UpdateTaskCategoryUseCase>();
            services.AddScoped<IDeleteTaskCategoryUseCase, DeleteTaskCategoryUseCase>();
            services.AddScoped<IGetAllTaskCategoryUseCase, GetAllTaskCategoryUseCase>();

            // User
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
            services.AddScoped<IUpdateUserPasswordUseCase, UpdateUserPasswordUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddScoped<IGetDataUserProfileUseCase, GetDataUserProfileUseCase>();


            //IA
            services.AddScoped<IAI_PriorityTasksUseCase, AI_PriorityTasksUseCase>();
            services.AddScoped<IAI_GenerateExecutionPlanUseCase, AI_GenerateExecutionPlanUseCase>();
            services.AddScoped<IAI_GenerateChildTaskUseCase, AI_GenerateChildTaskUseCase>();
            services.AddScoped<IAI_RefineTaskAttributesUseCase, AI_RefineTaskAttributesUseCase>();

            services.AddSingleton<TaskPriorityPrompt>();
            services.AddSingleton<ChildTaskPrompt>();
            services.AddSingleton<TaskExecutionPlanPrompt>();
            services.AddSingleton<RefineTaskAttributesPrompt>();

            return services;
        }
    }
}