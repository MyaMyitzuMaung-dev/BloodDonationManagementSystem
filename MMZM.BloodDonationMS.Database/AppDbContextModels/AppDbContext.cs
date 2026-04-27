using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MMZM.BloodDonationMS.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BloodDonation> BloodDonations { get; set; }

    public virtual DbSet<BloodRequest> BloodRequests { get; set; }

    public virtual DbSet<RequestComment> RequestComments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BloodDonation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__BloodDon__C5082EFB991E0911");

            entity.Property(e => e.DonationDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Accepted");

            entity.HasOne(d => d.Donor).WithMany(p => p.BloodDonations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donation_Donor");

            entity.HasOne(d => d.Request).WithMany(p => p.BloodDonations)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donation_Request");
        });

        modelBuilder.Entity<BloodRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BloodReq__33A8517A50C2FDB4");

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.HospitalName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PatientName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Urgency)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Requester).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_User");
        });

        modelBuilder.Entity<RequestComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__RequestC__C3B4DFCA88C52DA2");

            entity.Property(e => e.CommentText)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestComments)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Request");

            entity.HasOne(d => d.User).WithMany(p => p.RequestComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1ACE5AC79F");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CD63EB952");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534933DC333").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
