using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repositories
{
    public class ClienteRepository : GenericRepositoryInt<Cliente>, ICliente
    {
        private readonly JardineriaContext _context;

        public ClienteRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        // public async Task<List<Cliente>> GetGamas()
        // {
        //     var result = await (from cliente in _context.Clientes
        //                     join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk
        //                     join detallepedido in _context.DetallePedidos on pedido.Id equals detallepedido.IdPedidoFk
        //                     join producto in _context.Productos on detallepedido.IdProductoFk equals producto.Id
        //                     join gama in _context.GamaProductos on producto.IdGamaFk equals gama.Id
        //                     select new 
        //                     {
        //                         cliente = cliente.NombreCliente,
        //                         gama = gama.Id
        //                     }).ToListAsync();
        //     foreach (var x in result)
        //     {

        //     }

        // }

        public async Task<List<Cliente>> GetPedidoATiempo()
        {
            var result = await (from cliente in _context.Clientes
                                join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk
                                where pedido.FechaEntrega > pedido.FechaEsperada 
                                select cliente).ToListAsync();
            return result;
        }
    }
}