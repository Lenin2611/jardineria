# Jardinería

1. Devuelve el listado de clientes indicando el nombre del cliente y cuántos pedidos ha realizado. Tenga en cuenta que pueden existir clientes que no han realizado ningún pedido

```c#
        [HttpGet("cantidadpedidos")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClientesCantidadPedidoDto>>> GetCantidadPedidos()
        {
            var results = await (from cliente in _context.Clientes
                            join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk
                            select new ClientesCantidadPedidoDto
                            {
                                Nombre = cliente.NombreCliente,
                                Cantidad = 0
                            })
                            .ToListAsync();
            var clientesPedidos = new List<ClientesCantidadPedidoDto>();
            foreach (var p in results)
            {
                var clienteExistente = clientesPedidos.FirstOrDefault(x => x.Nombre == p.Nombre);
        
                if (clienteExistente != null)
                {
                    clienteExistente.Cantidad += 1;
                }
                else
                {
                    clientesPedidos.Add(new ClientesCantidadPedidoDto
                    {
                        Nombre = p.Nombre,
                        Cantidad = 1
                    });
                }
            }
            return clientesPedidos;
            }
```

Se retorna un Dto que contiene el nombre del cliente y la cantidad de pedidos de cada uno, usando linq para encontrar los clientes y la cantidad se iguala a 0, luego se crea una lista vacia de ese Dto y luego se busca en un foreach sin el cliente ya existe, si existe se agrega 1 a la cantidad y si no se agrega el cliente a la lista con cantidad 1.

2. Devuelve un listado con el código de pedido, código de cliente, fecha esperada y fecha de entrega de los pedidos que no han sido entregados a tiempo.

```c#
        [HttpGet("noatiempo")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Pedido2Dto>>> GetNoATiempo()
        {
            var results = await _unitOfWork.Pedidos.GetNoATiempo();
            return _mapper.Map<List<Pedido2Dto>>(results);
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
```

Se retorna un Dto con lo pedido en el enunciado, primero se usa FromSqlRaw en pedidos para retornar los pedidos en que la fecha de entrega sea mayor que la fecha esperada y también se mira si en realidad se entregó el pedido comparando con la fecha 1900-01-01 que es un default para decir que una fecha es null.

3. Devuelve las oficinas donde no trabajan ninguno de los empleados que hayan sido los representantes de ventas de algún cliente que haya realizado la compra de algún producto de la gama "Frutales".

```c#
        [HttpGet("frutales")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OficinaDto>>> GetFrutales()
        {
            var results = await _unitOfWork.Oficinas.GetFrutales();
            return _mapper.Map<List<OficinaDto>>(results);
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
```

Se retorna el Dto de oficina, Primero se busca por linq las oficinas que cumplan lo pedido en el enunciado, luego se pone en una lista y se retorna.

4. Devuelve el nombre del producto del que se han vendido más unidades.

```c#
        [HttpGet("masvendido")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductosCantidadVendidaDto>>> GetMasVendido()
        {
            var results = await (from producto in _context.Productos
                                join detallepedido in _context.DetallePedidos on producto.Id equals detallepedido.IdProductoFk
                                select new ProductosCantidadVendidaDto
                                {
                                    Nombre = producto.Nombre,
                                    Cantidad = detallepedido.Cantidad
                                }).ToListAsync();

            var productoTotal = new List<ProductosCantidadVendidaDto>();
            foreach (var p in results)
            {
                var productoExistente = productoTotal.FirstOrDefault(x => x.Nombre == p.Nombre);

                if (productoExistente != null)
                {
                    productoExistente.Cantidad += p.Cantidad;
                }
                else
                {
                    productoTotal.Add(new ProductosCantidadVendidaDto
                    {
                        Nombre = p.Nombre,
                        Cantidad = 0
                    });
                }
            }
            var z = productoTotal.OrderByDescending(x => x.Cantidad).Take(1).ToList();
            return z;
        }
```

Se retorna un Dto con el nombre del producto y la cantidad del producto más vendido, usando linq se buscan los productos, luego se crea una lista vacía del Dto y con un foreach se mira si existe el nombre del producto en la lista nueva, si existe, se le suma la cantidad de productos de ese pedido a la suma de productos total, y si no existe se agrega el nuevo producto con cantidad 0, luego se usa linq para mirar cual es el mayor con OrderByDescending y Take(1), lo convierto a lista y lo retorno.

5. Devuelve un listado de los 20 productos más vendidos y el número total de unidades que se han vendido de cada uno. El listado deberá estar ordenado por el número total de unidades vendidas.

```c#
[HttpGet("20masvendidos")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductosCantidadVendidaDto>>> Get20MasVendidos()
        {
            var results = await (from producto in _context.Productos
                                join detallepedido in _context.DetallePedidos on producto.Id equals detallepedido.IdProductoFk
                                select new ProductosCantidadVendidaDto
                                {
                                    Nombre = producto.Nombre,
                                    Cantidad = detallepedido.Cantidad
                                }).ToListAsync();

            var productoTotal = new List<ProductosCantidadVendidaDto>();
            foreach (var p in results)
            {
                var productoExistente = productoTotal.FirstOrDefault(x => x.Nombre == p.Nombre);

                if (productoExistente != null)
                {
                    productoExistente.Cantidad += p.Cantidad;
                }
                else
                {
                    productoTotal.Add(new ProductosCantidadVendidaDto
                    {
                        Nombre = p.Nombre,
                        Cantidad = 0
                    });
                }
            }
            var z = productoTotal.OrderByDescending(x => x.Cantidad).Take(20).ToList();
            return z;
        }
```

Se retorna un Dto con el nombre del producto y la cantidad del producto más vendido, usando linq se buscan los productos, luego se crea una lista vacía del Dto y con un foreach se mira si existe el nombre del producto en la lista nueva, si existe, se le suma la cantidad de productos de ese pedido a la suma de productos total, y si no existe se agrega el nuevo producto con cantidad 0, luego se usa linq para mirar cuales son los 20 con mayor cantidad vendida con OrderByDescending y Take(20), lo convierto a lista y lo retorno.

6. Devuelve el nombre de los clientes a los que no se les ha entregado a tiempo un pedido.

```c#
        [HttpGet("pedidonoatiempo")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetPedidoNoATiempo()
        {
            var results = await _unitOfWork.Clientes.GetPedidoATiempo();
            return _mapper.Map<List<ClienteDto>>(results);
        }

        public async Task<List<Cliente>> GetPedidoATiempo()
        {
            var result = await (from cliente in _context.Clientes
                                join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk
                                where pedido.FechaEntrega > pedido.FechaEsperada 
                                select cliente).ToListAsync();
            return result;
        }
```

Se retorna un listado de Dtos de clientes, donde se busca por linq los clientes a los que se les haya entregado un pedido tarde usando el mayor que.