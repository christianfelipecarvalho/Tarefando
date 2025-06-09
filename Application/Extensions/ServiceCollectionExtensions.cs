using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Tarefando.Application.Services;

namespace Tarefando.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ProjectService>();
        services.AddScoped<TaskService>();
        services.AddScoped<ReportService>();
        services.AddScoped<UserService>();

        return services;
    }
}