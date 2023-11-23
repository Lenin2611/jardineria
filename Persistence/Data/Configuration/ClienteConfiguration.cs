using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("cliente");

            builder.HasIndex(e => e.IdEmpleadoRepresentanteVentasFk, "idEmpleadoRepresentanteVentasFk");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            builder.Property(e => e.ApellidoContacto)
                .HasMaxLength(30)
                .HasColumnName("apellido_contacto");
            builder.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .HasColumnName("ciudad");
            builder.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .HasColumnName("codigo_postal");
            builder.Property(e => e.Fax)
                .HasMaxLength(15)
                .HasColumnName("fax");
            builder.Property(e => e.IdEmpleadoRepresentanteVentasFk).HasColumnName("idEmpleadoRepresentanteVentasFk");
            builder.Property(e => e.LimiteCredito)
                .HasPrecision(15, 2)
                .HasColumnName("limite_credito");
            builder.Property(e => e.LineaDireccion1)
                .HasMaxLength(50)
                .HasColumnName("linea_direccion1");
            builder.Property(e => e.LineaDireccion2)
                .HasMaxLength(50)
                .HasColumnName("linea_direccion2");
            builder.Property(e => e.NombreCliente)
                .HasMaxLength(50)
                .HasColumnName("nombre_cliente");
            builder.Property(e => e.NombreContacto)
                .HasMaxLength(30)
                .HasColumnName("nombre_contacto");
            builder.Property(e => e.Pais)
                .HasMaxLength(50)
                .HasColumnName("pais");
            builder.Property(e => e.Region)
                .HasMaxLength(50)
                .HasColumnName("region");
            builder.Property(e => e.Telefono)
                .HasMaxLength(15)
                .HasColumnName("telefono");

            builder.HasOne(d => d.IdEmpleadoRepresentanteVentasFkNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdEmpleadoRepresentanteVentasFk)
                .HasConstraintName("idEmpleadoRepresentanteVentasFk");
        }
    }
}