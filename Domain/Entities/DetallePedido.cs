using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class DetallePedido : BaseEntityInt
{

    public int IdPedidoFk { get; set; }

    public string IdProductoFk { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnidad { get; set; }

    public short NumeroLinea { get; set; }

    public virtual Pedido IdPedidoFkNavigation { get; set; } = null!;

    public virtual Producto IdProductoFkNavigation { get; set; } = null!;
}
