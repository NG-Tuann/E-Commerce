﻿using System;
using ElectronicCommerce.Areas.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<Geomancy> Geomancies { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<PromotionDetail> PromotionDetails { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StoneType> StoneTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<RolesModel> RolesModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=DA_TMDT;Trusted_Connection=False;User ID=sa;Password=0903123884Abc");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<RolesModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("CARTS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CARTS_CUSTOMERS");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_CARTS_ORDER_PRODUCTS");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CARTS_PRODUCTS");
            });

            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.ToTable("CATEGORY_PRODUCTS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("NAME");

                entity.Property(e => e.ParentId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PARENT_ID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__CATEGORY___PAREN__3A4CA8FD");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("CUSTOMERS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Address)
                    .HasMaxLength(80)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.CustomerTypeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_TYPE_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.IdCard)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("ID_CARD")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PHONE")
                    .IsFixedLength(true);

                entity.Property(e => e.ScorePay).HasColumnName("SCORE_PAY");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_CUSTOMERS_CUSTOMER_TYPES");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.ToTable("CUSTOMER_TYPES");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerTypeName)
                    .HasMaxLength(30)
                    .HasColumnName("CUSTOMER_TYPE_NAME");

                entity.Property(e => e.DiscountUnit)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DISCOUNT_UNIT")
                    .IsFixedLength(true);

                entity.Property(e => e.DiscountValue).HasColumnName("DISCOUNT_VALUE");
            });

            modelBuilder.Entity<Geomancy>(entity =>
            {
                entity.ToTable("GEOMANCIES");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Description).HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("IMAGES");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Levels).HasColumnName("LEVELS");

                entity.Property(e => e.NameImages)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME_IMAGES");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_ID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_IMAGES_PRODUCTS");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OrderId })
                    .HasName("PK_ORDER_DETAIL");

                entity.ToTable("ORDER_DETAILS");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDER_DETAILS_ORDER_PRODUCTS");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.ToTable("ORDER_PRODUCTS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.AddressDelivery)
                    .HasMaxLength(100)
                    .HasColumnName("ADDRESS_DELIVERY");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.DateCreated)
                    .HasColumnType("date")
                    .HasColumnName("DATE_CREATED");

                entity.Property(e => e.DatePay)
                    .HasColumnType("date")
                    .HasColumnName("DATE_PAY");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_USER")
                    .IsFixedLength(true);

                entity.Property(e => e.NameCusNonAccount)
                    .HasMaxLength(50)
                    .HasColumnName("NAME_CUS_NON_ACCOUNT");

                entity.Property(e => e.OrderState)
                    .HasMaxLength(30)
                    .HasColumnName("ORDER_STATE");

                entity.Property(e => e.Pay).HasColumnName("PAY");

                entity.Property(e => e.PayType)
                    .HasMaxLength(20)
                    .HasColumnName("PAY_TYPE");

                entity.Property(e => e.PhoneNonAccount)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NON_ACCOUNT")
                    .IsFixedLength(true);

                entity.Property(e => e.ShipDate)
                    .HasColumnType("date")
                    .HasColumnName("SHIP_DATE");

                entity.Property(e => e.ShipFee).HasColumnName("SHIP_FEE");

                entity.Property(e => e.TotalPay).HasColumnName("TOTAL_PAY");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ORDER_PRODUCTS_CUSTOMERS");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_ORDER_PRODUCTS_USERS");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCTS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.BestSeller).HasColumnName("BEST_SELLER");

                entity.Property(e => e.CatId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CAT_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Color)
                    .HasMaxLength(12)
                    .HasColumnName("COLOR");

                entity.Property(e => e.GeomancyId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("GEOMANCY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.HomeFlag).HasColumnName("HOME_FLAG");

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.ImportQuantity).HasColumnName("IMPORT_QUANTITY");

                entity.Property(e => e.MainStoneId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MAIN_STONE_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("NAME");

                entity.Property(e => e.Note)
                    .HasMaxLength(250)
                    .HasColumnName("NOTE");

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.Property(e => e.Size).HasColumnName("SIZE");

                entity.Property(e => e.SubStoneId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SUB_STONE_ID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_PRODUCTS_CATEGORY_PRODUCTS");

                entity.HasOne(d => d.Geomancy)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.GeomancyId)
                    .HasConstraintName("FK_PRODUCTS_GEOMANCIES");

                entity.HasOne(d => d.MainStone)
                    .WithMany(p => p.ProductMainStones)
                    .HasForeignKey(d => d.MainStoneId)
                    .HasConstraintName("FK__PRODUCTS__MAIN_S__3C34F16F");

                entity.HasOne(d => d.SubStone)
                    .WithMany(p => p.ProductSubStones)
                    .HasForeignKey(d => d.SubStoneId)
                    .HasConstraintName("FK__PRODUCTS__SUB_ST__3D2915A8");
            });

            modelBuilder.Entity<ProductDiscount>(entity =>
            {
                entity.ToTable("PRODUCT_DISCOUNTS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.DateCreated)
                    .HasColumnType("date")
                    .HasColumnName("DATE_CREATED");

                entity.Property(e => e.DiscountUnit)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DISCOUNT_UNIT")
                    .IsFixedLength(true);

                entity.Property(e => e.DiscountValue).HasColumnName("DISCOUNT_VALUE");

                entity.Property(e => e.IsRedeem)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_REDEEM")
                    .IsFixedLength(true);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.ValidUntil)
                    .HasColumnType("date")
                    .HasColumnName("VALID_UNTIL");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDiscounts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_PRODUCT_DISCOUNTS_PRODUCTS");
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.Size })
                    .HasName("PRODUCT_PRICES_PK");

                entity.ToTable("PRODUCT_PRICES");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Size).HasColumnName("SIZE");

                entity.Property(e => e.BasePrice).HasColumnName("BASE_PRICE");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.InActive)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IN_ACTIVE")
                    .IsFixedLength(true);

                entity.Property(e => e.SalePrice).HasColumnName("SALE_PRICE");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPrices)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRODUCT_PRICES_PRODUCTS");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("PROMOTIONS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DiscountUnit)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DISCOUNT_UNIT")
                    .IsFixedLength(true);

                entity.Property(e => e.DiscountValue).HasColumnName("DISCOUNT_VALUE");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.MaxDiscount).HasColumnName("MAX_DISCOUNT");

                entity.Property(e => e.MinOrder).HasColumnName("MIN_ORDER");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("NAME");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("START_DATE");
            });

            modelBuilder.Entity<PromotionDetail>(entity =>
            {
                entity.HasKey(e => e.IdPromotionDetail)
                    .HasName("PK__PROMOTIO__486F6DC1627BF251");

                entity.ToTable("PROMOTION_DETAIL");

                entity.Property(e => e.IdPromotionDetail)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_PROMOTION_DETAIL")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.PromotionId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PROMOTION_ID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.PromotionDetails)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_PROMOTION_DETAIL_CUSTOMERS");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionDetails)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PROMOTION_DETAIL_PROMOTIONS");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("REVIEWS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Rate).HasColumnName("RATE");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("TITLE");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_REVIEWS_CUSTOMERS");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_REVIEWS_PRODUCTS");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLES");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<StoneType>(entity =>
            {
                entity.ToTable("STONE_TYPES");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.IdCard)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("ID_CARD")
                    .IsFixedLength(true);

                entity.Property(e => e.IdRole)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_ROLE")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .HasColumnName("USERNAME");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_USERS_ROLES");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}