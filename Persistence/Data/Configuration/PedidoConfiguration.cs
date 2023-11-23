using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("pedido");

            builder.HasIndex(e => e.IdClienteFk, "idClienteFk");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            builder.Property(e => e.Comentarios)
                .HasColumnType("text")
                .HasColumnName("comentarios");
            builder.Property(e => e.Estado)
                .HasMaxLength(15)
                .HasColumnName("estado");
            builder.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            builder.Property(e => e.FechaEsperada).HasColumnName("fecha_esperada");
            builder.Property(e => e.FechaPedido).HasColumnName("fecha_pedido");
            builder.Property(e => e.IdClienteFk).HasColumnName("idClienteFk");

            builder.HasOne(d => d.IdClienteFkNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdClienteFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idClienteFk");
        }
    }
}