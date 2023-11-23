using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class Pedido2Dto
    {
        public int Id { get; set; }
        public int IdClienteFk { get; set; }
        public DateOnly FechaEntrega { get; set; }
        public DateOnly FechaEsperada { get; set; }
    }
}