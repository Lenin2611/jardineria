using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Empleado : BaseEntityInt
{

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; }

    public string Extension { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string IdOficinaFk { get; set; } = null!;

    public int? IdEmpleadoJefeFk { get; set; }

    public string Puesto { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Empleado IdEmpleadoJefeFkNavigation { get; set; }

    public virtual Oficina IdOficinaFkNavigation { get; set; } = null!;

    public virtual ICollection<Empleado> InverseIdEmpleadoJefeFkNavigation { get; set; } = new List<Empleado>();
}
