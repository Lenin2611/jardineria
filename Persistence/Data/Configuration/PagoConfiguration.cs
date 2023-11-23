using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("pago");

            builder.HasIndex(e => e.IdClienteFk, "idClienteFkk");

            builder.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            builder.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            builder.Property(e => e.FormaPago)
                .HasMaxLength(40)
                .HasColumnName("forma_pago");
            builder.Property(e => e.IdClienteFk).HasColumnName("idClienteFk");
            builder.Property(e => e.Total)
                .HasPrecision(15, 2)
                .HasColumnName("total");

            builder.HasOne(d => d.IdClienteFkNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdClienteFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idClienteFkk");
        }
    }
}