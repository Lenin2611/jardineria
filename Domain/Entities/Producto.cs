using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Producto : BaseEntityString
{

    public string Nombre { get; set; } = null!;

    public string IdGamaFk { get; set; } = null!;

    public string Dimensiones { get; set; }

    public string Proveedor { get; set; }

    public string Descripcion { get; set; }

    public short CantidadEnStock { get; set; }

    public decimal PrecioVenta { get; set; }

    public decimal? PrecioProveedor { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual GamaProducto IdGamaFkNavigation { get; set; } = null!;
}
