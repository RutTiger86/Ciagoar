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

        public virtual DbSet<AuthInfo> AuthInfos { get; set; }
        public virtual DbSet<DeliveryInfo> DeliveryInfos { get; set; }
        public virtual DbSet<DeliveryInventory> DeliveryInventories { get; set; }
        public virtual DbSet<DeliveryItem> DeliveryItems { get; set; }
        public virtual DbSet<InventoryInfo> InventoryInfos { get; set; }
        public virtual DbSet<InventoryStorage> InventoryStorages { get; set; }
        public virtual DbSet<ItemInfo> ItemInfos { get; set; }
        public virtual DbSet<OrderInfo> OrderInfos { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<RelativeCo> RelativeCos { get; set; }
        public virtual DbSet<RelativeCoStaff> RelativeCoStaffs { get; set; }
        public virtual DbSet<ReturnInfo> ReturnInfos { get; set; }
        public virtual DbSet<StorageInfo> StorageInfos { get; set; }
        public virtual DbSet<UserAuth> UserAuths { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthInfo>(entity =>
            {
                entity.ToTable("auth_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasColumnName("field_name");

                entity.Property(e => e.FieldValue)
                    .IsRequired()
                    .HasColumnName("field_value");

                entity.Property(e => e.TypeCode).HasColumnName("type_code");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DeliveryInfo>(entity =>
            {
                entity.ToTable("delivery_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplyUserId).HasColumnName("apply_user_id");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeliveryDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("delivery_datetime");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.MgrUserId).HasColumnName("mgr_user_id");

                entity.Property(e => e.OderStep).HasColumnName("oder_step");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.RelativeCoId).HasColumnName("relative_co_id");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.ApplyUser)
                    .WithMany(p => p.DeliveryInfoApplyUsers)
                    .HasForeignKey(d => d.ApplyUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_info_TO_delivery_info_1");

                entity.HasOne(d => d.MgrUser)
                    .WithMany(p => p.DeliveryInfoMgrUsers)
                    .HasForeignKey(d => d.MgrUserId)
                    .HasConstraintName("FK_user_info_TO_delivery_info_2");

                entity.HasOne(d => d.RelativeCo)
                    .WithMany(p => p.DeliveryInfos)
                    .HasForeignKey(d => d.RelativeCoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_relative_co_TO_delivery_info_1");
            });

            modelBuilder.Entity<DeliveryInventory>(entity =>
            {
                entity.ToTable("delivery_inventory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeliveryItemId).HasColumnName("delivery_item_id");

                entity.Property(e => e.InveontoryStorageId).HasColumnName("inveontory_storage_id");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.HasOne(d => d.DeliveryItem)
                    .WithMany(p => p.DeliveryInventories)
                    .HasForeignKey(d => d.DeliveryItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_delivery_item_TO_delivery_inventory_1");

                entity.HasOne(d => d.InveontoryStorage)
                    .WithMany(p => p.DeliveryInventories)
                    .HasForeignKey(d => d.InveontoryStorageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_storage_TO_delivery_inventory_1");
            });

            modelBuilder.Entity<DeliveryItem>(entity =>
            {
                entity.ToTable("delivery_item");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeliveryInfoId).HasColumnName("delivery_info_id");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.ItemInfoId).HasColumnName("item_info_id");

                entity.Property(e => e.OrderPrice)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("order_price");

                entity.Property(e => e.OrderQty)
                    .HasColumnType("date")
                    .HasColumnName("order_qty");

                entity.Property(e => e.PriceUnitType).HasColumnName("price_unit_type");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.DeliveryInfo)
                    .WithMany(p => p.DeliveryItems)
                    .HasForeignKey(d => d.DeliveryInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_delivery_info_TO_delivery_item_1");

                entity.HasOne(d => d.ItemInfo)
                    .WithMany(p => p.DeliveryItems)
                    .HasForeignKey(d => d.ItemInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_item_info_TO_delivery_item_1");
            });

            modelBuilder.Entity<InventoryInfo>(entity =>
            {
                entity.ToTable("inventory_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("date")
                    .HasColumnName("expiry_date");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ItemInfoId).HasColumnName("item_info_id");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.MfgDate)
                    .HasColumnType("date")
                    .HasColumnName("mfg_date");

                entity.Property(e => e.OrderInfoId).HasColumnName("order_info_id");

                entity.Property(e => e.ReceiptDate)
                    .HasColumnType("date")
                    .HasColumnName("receipt_date");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.ItemInfo)
                    .WithMany(p => p.InventoryInfos)
                    .HasForeignKey(d => d.ItemInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_item_info_TO_inventory_info_1");

                entity.HasOne(d => d.OrderInfo)
                    .WithMany(p => p.InventoryInfos)
                    .HasForeignKey(d => d.OrderInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_info_TO_inventory_info_1");
            });

            modelBuilder.Entity<InventoryStorage>(entity =>
            {
                entity.ToTable("inventory_storage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InventoryInfoId).HasColumnName("inventory_info_id");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SerialNo)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("serial_no");

                entity.Property(e => e.StorageInfoId).HasColumnName("storage_info_id");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.InventoryInfo)
                    .WithMany(p => p.InventoryStorages)
                    .HasForeignKey(d => d.InventoryInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_info_TO_inventory_storage_1");

                entity.HasOne(d => d.StorageInfo)
                    .WithMany(p => p.InventoryStorages)
                    .HasForeignKey(d => d.StorageInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_storage_info_TO_inventory_storage_1");
            });

            modelBuilder.Entity<ItemInfo>(entity =>
            {
                entity.ToTable("item_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BasePrice).HasColumnName("base_price");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("item_code");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("item_name");

                entity.Property(e => e.ItemNameEng)
                    .HasMaxLength(150)
                    .HasColumnName("item_name_eng");

                entity.Property(e => e.ModelCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("model_code");

                entity.Property(e => e.PriceUnitType).HasColumnName("price_unit_type");

                entity.Property(e => e.RelativeCoId).HasColumnName("relative_co_id");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("unit");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.RelativeCo)
                    .WithMany(p => p.ItemInfos)
                    .HasForeignKey(d => d.RelativeCoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_relative_co_TO_item_info_1");
            });

            modelBuilder.Entity<OrderInfo>(entity =>
            {
                entity.ToTable("order_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.OderStep).HasColumnName("oder_step");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.RelativeCoId).HasColumnName("relative_co_id");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.Property(e => e.UserInfoId).HasColumnName("user_info_id");

                entity.HasOne(d => d.RelativeCo)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.RelativeCoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_relative_co_TO_order_info_1");

                entity.HasOne(d => d.UserInfo)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.UserInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_info_TO_order_info_1");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_item");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.ItemInfoId).HasColumnName("item_info_id");

                entity.Property(e => e.OrderInfoId).HasColumnName("order_info_ID");

                entity.Property(e => e.OrderPrice)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("order_price");

                entity.Property(e => e.OrderQty)
                    .HasColumnType("date")
                    .HasColumnName("order_qty");

                entity.Property(e => e.PriceUnitType).HasColumnName("price_unit_type");

                entity.HasOne(d => d.ItemInfo)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ItemInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_item_info_TO_order_item_1");

                entity.HasOne(d => d.OrderInfo)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_info_TO_order_item_1");
            });

            modelBuilder.Entity<RelativeCo>(entity =>
            {
                entity.ToTable("relative_co");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CoAddress)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("co_address");

                entity.Property(e => e.CoName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("co_name");

                entity.Property(e => e.ConnectUrl)
                    .HasMaxLength(10)
                    .HasColumnName("connect_url");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");
            });

            modelBuilder.Entity<RelativeCoStaff>(entity =>
            {
                entity.ToTable("relative_co_staff");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("phone_number");

                entity.Property(e => e.RelativeCoId).HasColumnName("relative_co_id");

                entity.Property(e => e.StaffName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("staff_name");

                entity.Property(e => e.StaffRank)
                    .HasMaxLength(10)
                    .HasColumnName("staff_rank");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.RelativeCo)
                    .WithMany(p => p.RelativeCoStaffs)
                    .HasForeignKey(d => d.RelativeCoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_relative_co_TO_relative_co_staff_1");
            });

            modelBuilder.Entity<ReturnInfo>(entity =>
            {
                entity.ToTable("return_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplyUserId).HasColumnName("apply_user_id");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeliveryInventoryId).HasColumnName("delivery_inventory_id");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.MgrUserId).HasColumnName("mgr_user_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.RetrunType).HasColumnName("retrun_type");

                entity.Property(e => e.ReturnDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("return_datetime");

                entity.Property(e => e.ReturnStep).HasColumnName("return_step");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.HasOne(d => d.ApplyUser)
                    .WithMany(p => p.ReturnInfoApplyUsers)
                    .HasForeignKey(d => d.ApplyUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_info_TO_return_info_1");

                entity.HasOne(d => d.DeliveryInventory)
                    .WithMany(p => p.ReturnInfos)
                    .HasForeignKey(d => d.DeliveryInventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_delivery_inventory_TO_return_info_1");

                entity.HasOne(d => d.MgrUser)
                    .WithMany(p => p.ReturnInfoMgrUsers)
                    .HasForeignKey(d => d.MgrUserId)
                    .HasConstraintName("FK_user_info_TO_return_info_2");
            });

            modelBuilder.Entity<StorageInfo>(entity =>
            {
                entity.ToTable("storage_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Memo)
                    .HasMaxLength(400)
                    .HasColumnName("memo");

                entity.Property(e => e.StorageCode)
                    .HasMaxLength(150)
                    .HasColumnName("storage_code");

                entity.Property(e => e.StorageLocation)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("storage_location");

                entity.Property(e => e.StorageName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("storage_name");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.ToTable("user_auth");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthKey)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("auth_key");

                entity.Property(e => e.AuthStep).HasColumnName("auth_step");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TypeCode).HasColumnName("type_code");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");

                entity.Property(e => e.UserInfoId).HasColumnName("user_info_id");

                entity.HasOne(d => d.UserInfo)
                    .WithMany(p => p.UserAuths)
                    .HasForeignKey(d => d.UserInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_info_TO_user_auth_1");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("user_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createtime)
                    .HasColumnType("datetime")
                    .HasColumnName("createtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Isdelete).HasColumnName("isdelete");

                entity.Property(e => e.Isuse)
                    .IsRequired()
                    .HasColumnName("isuse")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("nickname");

                entity.Property(e => e.TypeCode).HasColumnName("type_code");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("updatetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
