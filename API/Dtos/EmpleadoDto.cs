using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class EmpleadoDto : BaseDtoInt
    {
        public string Nombre { get; set; } = null!;

        public string Apellido1 { get; set; } = null!;

        public string Apellido2 { get; set; }

        public string Extension { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string IdOficinaFk { get; set; } = null!;

        public int? IdEmpleadoJefeFk { get; set; }

        public string Puesto { get; set; }
    }
}