using RatmonAPI.Domain;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace RatmonAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Device> Devices => Set<Device>();
        public DbSet<Measurement> Measurements => Set<Measurement>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .OwnsOne(d => d.Configuration);


          modelBuilder.Entity<Measurement>()
         .HasDiscriminator<string>("MeasurementType")
         .HasValue<Mouse2Measurement>("Mouse2")
         .HasValue<Mouse2BMeasurement>("Mouse2B")
         .HasValue<MouseComboMeasurement>("MouseCombo")
         .HasValue<Mas2Measurement>("Mas2");


            modelBuilder.Entity<MouseComboMeasurement>()
      .OwnsMany(m => m.Reflectograms, r =>
      {
          r.WithOwner().HasForeignKey("MouseComboMeasurementId"); //fk key for refle
          r.Property<int>("Id"); 
          r.HasKey("Id");
      });

            base.OnModelCreating(modelBuilder);
        }

    }
}
