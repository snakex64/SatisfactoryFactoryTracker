using Microsoft.EntityFrameworkCore;
using SFT.Core.Domain;

namespace SFT.Core.Data;

public class SatisfactoryDbContext(DbContextOptions<SatisfactoryDbContext> options) : DbContext(options)
{
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Mine> Mines => Set<Mine>();
    public DbSet<MineOutput> MineOutputs => Set<MineOutput>();
    public DbSet<MiningStation> MiningStations => Set<MiningStation>();
    public DbSet<Factory> Factories => Set<Factory>();
    public DbSet<FactoryLevel> FactoryLevels => Set<FactoryLevel>();
    public DbSet<FactoryInput> FactoryInputs => Set<FactoryInput>();
    public DbSet<FactoryOutput> FactoryOutputs => Set<FactoryOutput>();
    public DbSet<ProductionRecipe> ProductionRecipes => Set<ProductionRecipe>();
    public DbSet<ProductionRecipeResource> ProductionRecipeResources => Set<ProductionRecipeResource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resource>().HasIndex(r => r.Name).IsUnique();
        modelBuilder.Entity<Resource>().HasIndex(r => r.KeyName).IsUnique();

        modelBuilder.Entity<Mine>()
            .HasOne(m => m.Resource)
            .WithMany()
            .HasForeignKey(m => m.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MineOutput>()
            .HasOne(o => o.Mine)
            .WithMany(m => m.Outputs)
            .HasForeignKey(o => o.MineId);

        modelBuilder.Entity<MineOutput>()
            .HasOne(o => o.Resource)
            .WithMany()
            .HasForeignKey(o => o.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MineOutput>()
            .HasIndex(o => new { o.MineId, o.ResourceId })
            .IsUnique();

        modelBuilder.Entity<MiningStation>()
            .HasOne(s => s.Mine)
            .WithMany(m => m.MiningStations)
            .HasForeignKey(s => s.MineId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MiningStation>()
            .HasIndex(s => new { s.MineId, s.MinerMk, s.OverclockLevel })
            .IsUnique();

        modelBuilder.Entity<FactoryLevel>()
            .HasOne(l => l.Factory)
            .WithMany(f => f.Levels)
            .HasForeignKey(l => l.FactoryId);

        modelBuilder.Entity<FactoryLevel>()
            .HasIndex(l => new { l.FactoryId, l.Identifier })
            .IsUnique();

        modelBuilder.Entity<FactoryInput>()
            .HasOne(i => i.FactoryLevel)
            .WithMany(l => l.Inputs)
            .HasForeignKey(i => i.FactoryLevelId);

        modelBuilder.Entity<FactoryInput>()
            .HasOne(i => i.Resource)
            .WithMany()
            .HasForeignKey(i => i.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FactoryInput>()
            .HasOne(i => i.SourceMine)
            .WithMany()
            .HasForeignKey(i => i.SourceMineId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FactoryInput>()
            .HasOne(i => i.SourceFactoryLevel)
            .WithMany()
            .HasForeignKey(i => i.SourceFactoryLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FactoryInput>()
            .HasIndex(i => new { i.FactoryLevelId, i.ResourceId })
            .IsUnique();

        modelBuilder.Entity<FactoryInput>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_FactoryInputs_Source",
                "(\"SourceMineId\" IS NULL) <> (\"SourceFactoryLevelId\" IS NULL)"));

        modelBuilder.Entity<FactoryOutput>()
            .HasOne(o => o.FactoryLevel)
            .WithMany(l => l.Outputs)
            .HasForeignKey(o => o.FactoryLevelId);

        modelBuilder.Entity<FactoryOutput>()
            .HasOne(o => o.Resource)
            .WithMany()
            .HasForeignKey(o => o.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FactoryOutput>()
            .HasIndex(o => new { o.FactoryLevelId, o.ResourceId })
            .IsUnique();

        modelBuilder.Entity<ProductionRecipe>()
            .HasIndex(r => r.KeyName)
            .IsUnique();

        modelBuilder.Entity<ProductionRecipeResource>()
            .HasOne(r => r.ProductionRecipe)
            .WithMany(p => p.Resources)
            .HasForeignKey(r => r.ProductionRecipeId);

        modelBuilder.Entity<ProductionRecipeResource>()
            .HasOne(r => r.Resource)
            .WithMany(resource => resource.RecipeUsages)
            .HasForeignKey(r => r.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductionRecipeResource>()
            .HasIndex(r => new { r.ProductionRecipeId, r.ResourceId, r.IsInput });
    }
}
