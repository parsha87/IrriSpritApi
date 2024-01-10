using System;
using System.Collections.Generic;
using GISApi.Data.GlobalEntities;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data;

public partial class GlobalDBContext : DbContext
{
    public GlobalDBContext()
    {
    }

    public GlobalDBContext(DbContextOptions<GlobalDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<ControllerMaster> ControllerMasters { get; set; }

    public virtual DbSet<ControllerTimeSetting> ControllerTimeSettings { get; set; }

    public virtual DbSet<CyclicSequenceSetting> CyclicSequenceSettings { get; set; }

    public virtual DbSet<FilterSequenceSetting> FilterSequenceSettings { get; set; }

    public virtual DbSet<MaxFiltervalveSetting> MaxFiltervalveSettings { get; set; }

    public virtual DbSet<SequenceSetting> SequenceSettings { get; set; }

    public virtual DbSet<UserControllerMapping> UserControllerMappings { get; set; }

    public virtual DbSet<ValveSetting> ValveSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
