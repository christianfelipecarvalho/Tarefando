using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Tarefando.Domain.Entities;
using Tarefando.Domain.Enums;

namespace Tarefando.Infrastructure.Data;

public class TarefandoDbContext : DbContext
{
    public TarefandoDbContext(DbContextOptions<TarefandoDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Project Configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).HasMaxLength(1000);

            entity.HasOne(e => e.User)
                  .WithMany(e => e.Projects)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TaskItem Configuration
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).HasMaxLength(1000);
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.Priority).HasConversion<int>();

            entity.HasOne(e => e.Project)
                  .WithMany(e => e.Tasks)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TaskHistory Configuration
        modelBuilder.Entity<TaskHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Alteracao).IsRequired().HasMaxLength(500);

            entity.HasOne(e => e.Task)
                  .WithMany(e => e.History)
                  .HasForeignKey(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // TaskComment Configuration
        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comentario).IsRequired().HasMaxLength(1000);

            entity.HasOne(e => e.Task)
                  .WithMany(e => e.Comments)
                  .HasForeignKey(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed Data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "admin@tarefando.com", Nome = "Admin", Role = UserRole.Gerente },
            new User { Id = 2, Email = "usuario@tarefando.com", Nome = "Usuário Teste", Role = UserRole.Usuario }
        );
    }
}
