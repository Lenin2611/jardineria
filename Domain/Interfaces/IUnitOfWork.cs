using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public ICliente Clientes { get; }
        public IDetallePedido DetallePedidos { get; }
        public IEmpleado Empleados { get; }
        public IGamaProducto GamaProductos { get; }
        public IOficina Oficinas { get; }
        public IPago Pagos { get; }
        public IPedido Pedidos { get; }
        public IProducto Productos { get; }

        Task<int> SaveAsync();
    }
}