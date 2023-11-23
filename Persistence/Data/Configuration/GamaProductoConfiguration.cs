using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class GamaProductoConfiguration : IEntityTypeConfiguration<GamaProducto>
    {
        public void Configure(EntityTypeBuilder<GamaProducto> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("gama_producto");

            builder.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            builder.Property(e => e.DescripcionHtml)
                .HasColumnType("text")
                .HasColumnName("descripcion_html");
            builder.Property(e => e.DescripcionTexto)
                .HasColumnType("text")
                .HasColumnName("descripcion_texto");
            builder.Property(e => e.Imagen)
                .HasMaxLength(256)
                .HasColumnName("imagen");
        }
    }
}