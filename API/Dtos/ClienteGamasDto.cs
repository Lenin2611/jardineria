using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ClienteGamasDto
    {
        public string Nombre { get; set; }
        public List<GamaProductoDto> MyProperty { get; set; }
    }
}