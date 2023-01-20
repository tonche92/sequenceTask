using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace testTask.Models;

public partial class TestTaskDbContext : DbContext
{
    public TestTaskDbContext()
    {
    }

    public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sequence> Sequences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LAPTOP-AO4R2AGN; Database=testTaskDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sequence__3213E83F3B30C531");

            entity.ToTable("sequence");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentValue)
                .HasMaxLength(50)
                .HasDefaultValueSql("('')")
                .HasColumnName("currentValue");
            entity.Property(e => e.MaxCounter).HasColumnName("maxCounter");
            entity.Property(e => e.MinDigits).HasColumnName("minDigits");
            entity.Property(e => e.Prefix)
                .HasMaxLength(30)
                .HasDefaultValueSql("('')")
                .HasColumnName("prefix");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
