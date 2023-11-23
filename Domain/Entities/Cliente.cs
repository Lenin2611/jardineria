using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cliente : BaseEntityInt
{

    public string NombreCliente { get; set; } = null!;

    public string NombreContacto { get; set; }

    public string ApellidoContacto { get; set; }

    public string Telefono { get; set; } = null!;

    public string Fax { get; set; } = null!;

    public string LineaDireccion1 { get; set; } = null!;

    public string LineaDireccion2 { get; set; }

    public string Ciudad { get; set; } = null!;

    public string Region { get; set; }

    public string Pais { get; set; }

    public string CodigoPostal { get; set; }

    public int? IdEmpleadoRepresentanteVentasFk { get; set; }

    public decimal? LimiteCredito { get; set; }

    public virtual Empleado IdEmpleadoRepresentanteVentasFkNavigation { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
