using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Tarefando.Infrastructure.Data;
using Tarefando.Infrastructure.Repositories;

namespace Tarefando.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TarefandoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
            {
                // Configurações de pool de conexão
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            }

            ));
        services.Configure<NpgsqlConnectionStringBuilder>(builder =>
        {
            builder.Pooling = true;
            builder.MinPoolSize = 1;
            builder.MaxPoolSize = 20;
            builder.ConnectionLifetime = 300; // 5 minutos
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();


        return services;
    }
}