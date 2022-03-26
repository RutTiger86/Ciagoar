using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CiagoarS.DataBase
{
    public partial class CiagoarContext : DbContext
    {
        public CiagoarContext()
        {
        }

        public CiagoarContext(DbContextOptions<CiagoarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserAuthentication> UserAuthentications { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuthentication>(entity =>
            {
                entity.ToTable("USER_AUTHENTICATION");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthenticationKey)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.UserInfoId).HasColumnName("UserInfoID");

                entity.HasOne(d => d.UserInfo)
                    .WithMany(p => p.UserAuthentications)
                    .HasForeignKey(d => d.UserInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_AUTHENTICATION_USER_INFO");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("USER_INFO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId).HasColumnName("UpdateUserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
