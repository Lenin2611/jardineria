using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("detalle_pedido");

            builder.HasIndex(e => e.IdPedidoFk, "idPedidoFk");

            builder.HasIndex(e => e.IdProductoFk, "idProductoFk");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Cantidad).HasColumnName("cantidad");
            builder.Property(e => e.IdPedidoFk).HasColumnName("idPedidoFk");
            builder.Property(e => e.IdProductoFk)
                .HasMaxLength(15)
                .HasColumnName("idProductoFk");
            builder.Property(e => e.NumeroLinea).HasColumnName("numero_linea");
            builder.Property(e => e.PrecioUnidad)
                .HasPrecision(15, 2)
                .HasColumnName("precio_unidad");

            builder.HasOne(d => d.IdPedidoFkNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedidoFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idPedidoFk");

            builder.HasOne(d => d.IdProductoFkNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProductoFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idProductoFk");
        }
    }
}