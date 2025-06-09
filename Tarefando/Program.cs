using Tarefando.Application.Extensions;
using Tarefando.Filters;
using Tarefando.Infrastructure.Data;
using Tarefando.Infrastructure.Extensions;
using Tarefando.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UserValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Tarefando API", Version = "v1" });
    c.AddServer(new() { Url = "http://localhost:8080", Description = "Development Server" });
    c.AddServer(new() { Url = "http://localhost:8080", Description = "Production Server" });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tarefando API V1");
});
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TarefandoDbContext>();
    try
    {
        context.Database.EnsureCreated();
        app.Logger.LogInformation("Database ensured created successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.Logger.LogInformation("Tarefando API started successfully!");
app.Run();