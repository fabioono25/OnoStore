using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OnoStore.Cart.API.Model;
using System.Linq;

namespace OnoStore.Cart.API.Data
{
    public sealed class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CustomerCart> CustomerCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.Entity<CustomerCart>()
                .HasIndex(c => c.CustomerId)
                .HasDatabaseName("IDX_Customer");

            modelBuilder.Entity<CustomerCart>()
                .Ignore(c => c.Voucher)
                .OwnsOne(c => c.Voucher, v =>
                {
                    v.Property(vc => vc.Codigo)
                        .HasColumnName("VoucherCodigo")
                        .HasColumnType("varchar(50)");

                    v.Property(vc => vc.TipoDesconto)
                        .HasColumnName("TipoDesconto");

                    v.Property(vc => vc.Percentual)
                        .HasColumnName("Percentual");

                    v.Property(vc => vc.ValorDesconto)
                        .HasColumnName("ValorDesconto");
                });

            modelBuilder.Entity<CustomerCart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.CustomerCart)
                .HasForeignKey(c => c.CartId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            //relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
    }
}