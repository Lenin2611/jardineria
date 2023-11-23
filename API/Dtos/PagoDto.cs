using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class PagoDto : BaseDtoString
    {
        public int IdClienteFk { get; set; }

        public string FormaPago { get; set; } = null!;

        public DateOnly FechaPago { get; set; }

        public decimal Total { get; set; }
    }
}