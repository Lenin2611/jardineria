using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repositories
{
    public class OficinaRepository : GenericRepositoryString<Oficina>, IOficina
    {
        private readonly JardineriaContext _context;

        public OficinaRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Oficina>> GetFrutales()
        {
            var result = await (from oficina in _context.Oficinas
                            join empleado in _context.Empleados on oficina.Id equals empleado.IdOficinaFk
                            join cliente in _context.Clientes on empleado.Id equals cliente.IdEmpleadoRepresentanteVentasFk
                            join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk
                            join detallepedido in _context.DetallePedidos on pedido.Id equals detallepedido.IdPedidoFk
                            join producto in _context.Productos on detallepedido.IdProductoFk equals producto.Id
                            where producto.IdGamaFk.Trim().ToLower() != "frutales"
                            select oficina).ToListAsync();
            return result;
        }
    }
}