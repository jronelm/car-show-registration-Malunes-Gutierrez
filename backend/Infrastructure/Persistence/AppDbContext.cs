namespace Infrastructure.Persistence;

using Application.Abstractions.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RegistrationNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.ParticipantName).IsRequired().HasMaxLength(150);
            entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(11);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
            entity.Property(e => e.CarMakeModel).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PlateNumber).IsRequired().HasMaxLength(15);

            // Store enums as readable text ("Competitor") instead of numbers
            entity.Property(e => e.RegistrationType).HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);

            entity.Property(e => e.RegistrationFee).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);

            // One car = one registration, and numbers must never repeat
            entity.HasIndex(e => e.PlateNumber).IsUnique();
            entity.HasIndex(e => e.RegistrationNumber).IsUnique();

            // SQLite does not keep the "UTC" marker, so mark dates as UTC when reading
            entity.Property(e => e.RegistrationDate)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        });
    }
}