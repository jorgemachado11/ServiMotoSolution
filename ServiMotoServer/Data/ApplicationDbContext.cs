using Microsoft.EntityFrameworkCore;
using ServiMotoServer.Models;

namespace ServiMotoServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Service> Services { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Models.Motorcycle> Motorcycles { get; set; }
        public DbSet<Models.ServiceAssignment> ServiceAssignments { get; set; }
        public DbSet<Models.TaskAssignment> TaskAssignments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships
            modelBuilder.Entity<Models.User>()
                .HasMany(u => u.ServiceAssignments)
                .WithOne(sa => sa.User)
                .HasForeignKey(sa => sa.UserId);

            modelBuilder.Entity<Models.Service>()
                .HasMany(s => s.Tasks)
                .WithOne(t => t.Service)
                .HasForeignKey(t => t.ServiceId);

            modelBuilder.Entity<Models.Service>()
                .HasMany(s => s.ServiceAssignments)
                .WithOne(sa => sa.Service)
                .HasForeignKey(sa => sa.ServiceId);

            modelBuilder.Entity<Models.Motorcycle>()
                .HasMany(m => m.ServiceAssignments)
                .WithOne(sa => sa.Motorcycle)
                .HasForeignKey(sa => sa.MotorcycleId);

            modelBuilder.Entity<Models.Motorcycle>()
                .HasMany(m => m.TaskAssignments)
                .WithOne(ta => ta.Motorcycle)
                .HasForeignKey(ta => ta.MotorcycleId);

            modelBuilder.Entity<Models.Task>()
                .HasMany(t => t.TaskAssignments)
                .WithOne(ta => ta.Task)
                .HasForeignKey(ta => ta.TaskId);

            modelBuilder.Entity<Models.Service>()
                .HasIndex(s => s.ServiceName)
                .IsUnique();

            // Seed data
            var admin1Id = Guid.NewGuid();
            var admin2Id = Guid.NewGuid();
            var admin3Id = Guid.NewGuid();

            var service1Id = Guid.NewGuid();
            var service2Id = Guid.NewGuid();
            var service3Id = Guid.NewGuid();

            var motorcycle1Id = Guid.NewGuid();
            var motorcycle2Id = Guid.NewGuid();
            var motorcycle3Id = Guid.NewGuid();

            modelBuilder.Entity<Models.User>().HasData(
               new Models.User { Id = admin1Id, Username = "admin1", Email = "admin1@admin.com", Password = "admin123", IsAdministrator = true },
               new Models.User { Id = admin2Id, Username = "admin2", Email = "admin2@admin.com", Password = "admin123", IsAdministrator = true },
               new Models.User { Id = admin3Id, Username = "admin3", Email = "admin3@admin.com", Password = "admin123", IsAdministrator = true }
           );

            modelBuilder.Entity<Models.Service>().HasData(
               new Models.Service { Id = service1Id, ServiceName = "Service1", Description = "Description for Service 1" },
               new Models.Service { Id = service2Id, ServiceName = "Service2", Description = "Description for Service 2" },
               new Models.Service { Id = service3Id, ServiceName = "Service3", Description = "Description for Service 3" }
           );

            modelBuilder.Entity<Models.Motorcycle>().HasData(
                new Models.Motorcycle { Id = motorcycle1Id, MotorcycleName = "motorcycle1", Description = "Motorcycle 1", Password = "moto123" },
                new Models.Motorcycle { Id = motorcycle2Id, MotorcycleName = "motorcycle2", Description = "Motorcycle 2", Password = "moto123" },
                new Models.Motorcycle { Id = motorcycle3Id, MotorcycleName = "motorcycle3", Description = "Motorcycle 3", Password = "moto123" }
            );

            modelBuilder.Entity<Models.ServiceAssignment>().HasData(
               new Models.ServiceAssignment { Id = Guid.NewGuid(), UserId = admin1Id, ServiceId = service1Id },
               new Models.ServiceAssignment { Id = Guid.NewGuid(), UserId = admin2Id, ServiceId = service2Id },
               new Models.ServiceAssignment { Id = Guid.NewGuid(), UserId = admin3Id, ServiceId = service3Id },
               new Models.ServiceAssignment { Id = Guid.NewGuid(), MotorcycleId = motorcycle1Id, ServiceId = service1Id },
               new Models.ServiceAssignment { Id = Guid.NewGuid(), MotorcycleId = motorcycle2Id, ServiceId = service2Id },
               new Models.ServiceAssignment { Id = Guid.NewGuid(), MotorcycleId = motorcycle3Id, ServiceId = service3Id }
           );
        }
    }
}
