namespace Fourier.Data;
using Microsoft.EntityFrameworkCore;
using Fourier.Models;

public class FourierDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public FourierDbContext(DbContextOptions<FourierDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Task> Tasks { get; set; }

    public DbSet<CancellationToken> CancellationTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(e => e.HashedPassword)
                  .IsRequired();
            entity.Property(e => e.CreatedAt)
                  .IsRequired();

            entity.HasIndex(e => e.Username)
                  .IsUnique();

            entity.HasMany(e => e.Tasks)
                  .WithOne(e => e.User)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(e => e.Input)
                  .IsRequired();
            entity.Property(e => e.Progress)
                  .IsRequired();
            entity.Property(e => e.Result)
                  .HasMaxLength(1000);

            entity.HasIndex(e => e.UserId);

            entity.HasOne(e => e.CancellationToken)
                  .WithOne(e => e.Task)
                  .HasForeignKey<CancellationToken>(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CancellationToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsCancelled)
                  .IsRequired();

            entity.HasIndex(e => e.TaskId)
                  .IsUnique();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("DefaultSource"));
        }
    }
}