using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebHostProject.DataAccess.Weather
{
    public partial class weatherContext : DbContext
    {
        public weatherContext()
        {
        }

        public weatherContext(DbContextOptions<weatherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Prediction> Predictions { get; set; }
        public virtual DbSet<SchemaMigration> SchemaMigrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Prediction>(entity =>
            {
                entity.HasKey(e => e.Day)
                    .HasName("pred_pk");

                entity.ToTable("predictions", "weather_predictions");

                entity.Property(e => e.Day)
                    .HasColumnType("date")
                    .HasColumnName("day");

                entity.Property(e => e.Temperature).HasColumnName("temperature");
            });

            modelBuilder.Entity<SchemaMigration>(entity =>
            {
                entity.HasKey(e => e.InstalledRank)
                    .HasName("schema_migrations_pk");

                entity.ToTable("schema_migrations");

                entity.HasIndex(e => e.Success, "schema_migrations_s_idx");

                entity.Property(e => e.InstalledRank)
                    .ValueGeneratedNever()
                    .HasColumnName("installed_rank");

                entity.Property(e => e.Checksum).HasColumnName("checksum");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.ExecutionTime).HasColumnName("execution_time");

                entity.Property(e => e.InstalledBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("installed_by");

                entity.Property(e => e.InstalledOn)
                    .HasColumnName("installed_on")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Script)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("script");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("type");

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .HasColumnName("version");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
