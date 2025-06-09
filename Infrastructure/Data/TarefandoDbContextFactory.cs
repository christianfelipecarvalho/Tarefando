using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Tarefando.Infrastructure.Data;

namespace Tarefando.Infrastructure;

public class TarefandoDbContextFactory : IDesignTimeDbContextFactory<TarefandoDbContext>
{
    public TarefandoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TarefandoDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new TarefandoDbContext(optionsBuilder.Options);
    }
}
