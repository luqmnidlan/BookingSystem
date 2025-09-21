using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<AppointmentFeedbackModel> AppointmentFeedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BookingModel
            modelBuilder.Entity<BookingModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CustomerPhone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ServiceType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AppointmentTime).IsRequired().HasMaxLength(10);
                entity.Property(e => e.BarberName).HasMaxLength(50);
                entity.Property(e => e.SpecialRequests).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Pending");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configure ServiceModel
            modelBuilder.Entity<ServiceModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Configure EmployeeModel
            modelBuilder.Entity<EmployeeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Specialty).HasMaxLength(200);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Aktif");
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configure AppointmentFeedbackModel
            modelBuilder.Entity<AppointmentFeedbackModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BookingId).IsRequired();
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
                
                // Foreign key relationship
                entity.HasOne(e => e.Booking)
                      .WithMany()
                      .HasForeignKey(e => e.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
