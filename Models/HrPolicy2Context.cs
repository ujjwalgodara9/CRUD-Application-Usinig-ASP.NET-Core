using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hr_policy.Models;

public partial class HrPolicy2Context : DbContext
{
    public HrPolicy2Context()
    {
    }

    public HrPolicy2Context(DbContextOptions<HrPolicy2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<HrPolicy> HrPolicies { get; set; }

    public virtual DbSet<MasterRole> MasterRoles { get; set; }

    public virtual DbSet<PolicyDocument> PolicyDocuments { get; set; }

    public virtual DbSet<PolicyTopic> PolicyTopics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-4PUST3H; database=HrPolicy_2; trusted_connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HrPolicy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HrPolicy__3214EC07DF30F167");

            entity.ToTable("HrPolicy");

            entity.Property(e => e.ArchivedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ArchivedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InsertedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PolicyName).HasMaxLength(4000);
            entity.Property(e => e.PolicyRefNo).HasMaxLength(200);
            entity.Property(e => e.Remarks).HasMaxLength(4000);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Topic).WithMany(p => p.HrPolicies)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HrPolicy_PolicyTopic");
        });

        modelBuilder.Entity<MasterRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__MasterRo__8AFACE1A8240A0F7");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(200);
        });

        modelBuilder.Entity<PolicyDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PolicyDo__3214EC0709529380");

            entity.Property(e => e.ArchivedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ArchivedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContentType).HasMaxLength(500);
            entity.Property(e => e.DocumentCaption).HasMaxLength(2000);
            entity.Property(e => e.Extension).HasMaxLength(500);
            entity.Property(e => e.FileHash).HasMaxLength(500);
            entity.Property(e => e.FileName).HasMaxLength(2000);
            entity.Property(e => e.InsertedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InsertedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).HasMaxLength(4000);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WordFileName).HasMaxLength(2000);
            entity.Property(e => e.WordFileUpdatedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.WordFileUpdatedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Policy).WithMany(p => p.PolicyDocuments)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PolicyDocuments_HrPolicy");

            entity.HasOne(d => d.Topic).WithMany(p => p.PolicyDocuments)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PolicyDocuments_PolicyTopic");
        });

        modelBuilder.Entity<PolicyTopic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PolicyTo__3214EC07011A4D04");

            entity.ToTable("PolicyTopic");

            entity.Property(e => e.InsertedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InsertedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(2000);
            entity.Property(e => e.Remarks).HasMaxLength(2000);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Id");

            entity.ToTable("User");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_MasterRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
