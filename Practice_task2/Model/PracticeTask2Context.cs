
using Microsoft.EntityFrameworkCore;

namespace Practice_task2.Model;

public partial class PracticeTask2Context : DbContext
{
    public PracticeTask2Context() { }

    public PracticeTask2Context(DbContextOptions<PracticeTask2Context> options)
        : base(options) { }

    public virtual DbSet<AddressInLocation> AddressInLocations { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<ManagementCompany> ManagementCompanies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressInLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("address_in_location_pkey");

            entity.ToTable("address_in_location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FiasCityCode).HasMaxLength(3).HasColumnName("fias_city_code");
            entity.Property(e => e.FiasHouseCode).HasMaxLength(4).HasColumnName("fias_house_code");
            entity
                .Property(e => e.FiasRegionCode)
                .HasMaxLength(2)
                .HasColumnName("fias_region_code");
            entity
                .Property(e => e.FiasStreetCode)
                .HasMaxLength(4)
                .HasColumnName("fias_street_code");
            entity.Property(e => e.LocationId).HasColumnName("location_id");

            // entity.HasOne(d => d.Location).WithMany(p => p.AddressInLocations)
            //     .HasForeignKey(d => d.LocationId)
            //     .OnDelete(DeleteBehavior.Cascade)
            //     .HasConstraintName("address_in_location_location_id_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_pkey");

            entity.ToTable("location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ManagementCompanyId).HasColumnName("management_company_id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");

            // entity.HasOne(d => d.ManagementCompany).WithMany(p => p.Locations)
            //     .HasForeignKey(d => d.ManagementCompanyId)
            //     .OnDelete(DeleteBehavior.Cascade)
            //     .HasConstraintName("location_management_company_id_fkey");
        });

        modelBuilder.Entity<ManagementCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("management_company_pkey");

            entity.ToTable("management_company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
