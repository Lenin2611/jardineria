using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("empleado");

            builder.HasIndex(e => e.IdEmpleadoJefeFk, "idEmpleadoJefeFk");

            builder.HasIndex(e => e.IdOficinaFk, "idOficinaFk");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            builder.Property(e => e.Apellido1)
                .HasMaxLength(50)
                .HasColumnName("apellido1");
            builder.Property(e => e.Apellido2)
                .HasMaxLength(50)
                .HasColumnName("apellido2");
            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            builder.Property(e => e.Extension)
                .HasMaxLength(10)
                .HasColumnName("extension");
            builder.Property(e => e.IdEmpleadoJefeFk).HasColumnName("idEmpleadoJefeFk");
            builder.Property(e => e.IdOficinaFk)
                .HasMaxLength(10)
                .HasColumnName("idOficinaFk");
            builder.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            builder.Property(e => e.Puesto)
                .HasMaxLength(50)
                .HasColumnName("puesto");

            builder.HasOne(d => d.IdEmpleadoJefeFkNavigation).WithMany(p => p.InverseIdEmpleadoJefeFkNavigation)
                .HasForeignKey(d => d.IdEmpleadoJefeFk)
                .HasConstraintName("idEmpleadoJefeFk");

            builder.HasOne(d => d.IdOficinaFkNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdOficinaFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idOficinaFk");
        }
    }
}