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

        public virtual DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("USER_INFO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthenticationKey)
                    .IsRequired()
                    .HasMaxLength(512);

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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
