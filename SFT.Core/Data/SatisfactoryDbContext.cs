using Microsoft.EntityFrameworkCore;
using SFT.Core.Domain;

namespace SFT.Core.Data;

public class SatisfactoryDbContext(DbContextOptions<SatisfactoryDbContext> options) : DbContext(options)
{
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Mine> Mines => Set<Mine>();
    public DbSet<Factory> Factories => Set<Factory>();
    public DbSet<FactoryInput> FactoryInputs => Set<FactoryInput>();
    public DbSet<FactoryOutput> FactoryOutputs => Set<FactoryOutput>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resource>().HasIndex(r => r.Name).IsUnique();

        modelBuilder.Entity<Mine>()
            .HasOne(m => m.Resource)
            .WithMany()
            .HasForeignKey(m => m.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FactoryInput>()
            .HasOne(i => i.Factory)
            .WithMany(f => f.Inputs)
            .HasForeignKey(i => i.FactoryId);

        modelBuilder.Entity<FactoryInput>()
            .HasOne(i => i.Resource)
            .WithMany()
            .HasForeignKey(i => i.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FactoryOutput>()
            .HasOne(o => o.Factory)
            .WithMany(f => f.Outputs)
            .HasForeignKey(o => o.FactoryId);

        modelBuilder.Entity<FactoryOutput>()
            .HasOne(o => o.Resource)
            .WithMany()
            .HasForeignKey(o => o.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
