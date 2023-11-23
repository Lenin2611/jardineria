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
    public class PedidoRepository : GenericRepositoryInt<Pedido>, IPedido
    {
        private readonly JardineriaContext _context;

        public PedidoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> GetNoATiempo()
        {
            var result = await _context.Pedidos.FromSqlRaw
            (
                @"SELECT p.*
                    FROM Pedido p
                    WHERE p.fecha_entrega > p.fecha_esperada && p.fecha_entrega != '1900-01-01'"
            ).ToListAsync();
            return result;
        }
    }
}