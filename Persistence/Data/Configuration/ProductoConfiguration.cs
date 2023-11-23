using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("producto");

            builder.HasIndex(e => e.IdGamaFk, "idGamaFk");

            builder.Property(e => e.Id)
                .HasMaxLength(15)
                .HasColumnName("id");
            builder.Property(e => e.CantidadEnStock).HasColumnName("cantidad_en_stock");
            builder.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            builder.Property(e => e.Dimensiones)
                .HasMaxLength(25)
                .HasColumnName("dimensiones");
            builder.Property(e => e.IdGamaFk)
                .HasMaxLength(50)
                .HasColumnName("idGamaFk");
            builder.Property(e => e.Nombre)
                .HasMaxLength(70)
                .HasColumnName("nombre");
            builder.Property(e => e.PrecioProveedor)
                .HasPrecision(15, 2)
                .HasColumnName("precio_proveedor");
            builder.Property(e => e.PrecioVenta)
                .HasPrecision(15, 2)
                .HasColumnName("precio_venta");
            builder.Property(e => e.Proveedor)
                .HasMaxLength(50)
                .HasColumnName("proveedor");

            builder.HasOne(d => d.IdGamaFkNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdGamaFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idGamaFk");
        }
    }
}