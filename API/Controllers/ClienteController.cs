using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers
{
    public class ClienteController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JardineriaContext _context;

        public ClienteController(IUnitOfWork unitOfWork, IMapper mapper, JardineriaContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
        {
            var results = await _unitOfWork.Clientes.GetAllAsync();
            return _mapper.Map<List<ClienteDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Get(int id)
        {
            var result = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<ClienteDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Post(ClienteDto resultDto)
        {
            var result = _mapper.Map<Cliente>(resultDto);
            _unitOfWork.Clientes.Add(result);
            await _unitOfWork.SaveAsync();
            if (result == null)
            {
                return BadRequest();
            }
            resultDto.Id = result.Id;
            return CreatedAtAction(nameof(Post), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Put(int id, [FromBody] ClienteDto resultDto)
        {
            var exists = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (resultDto.Id == 0)
            {
                resultDto.Id = id;
            }
            if (resultDto.Id != id)
            {
                return BadRequest();
            }
            // Update the properties of the existing entity with values from resultDto
            _mapper.Map(resultDto, exists);
            // The context is already tracking result, so no need to attach it
            await _unitOfWork.SaveAsync();
            // Return the updated entity
            return _mapper.Map<ClienteDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Clientes.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("pedidonoatiempo")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetPedidoNoATiempo()
        {
            var results = await _unitOfWork.Clientes.GetPedidoATiempo();
            return _mapper.Map<List<ClienteDto>>(results);
        }

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

        [HttpGet("cantidadpedidos")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClientesCantidadPedidoDto>>> GetClienteGamas()
        {
            var results = await (from cliente in _context.Clientes
                                 join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk
                                 join detallepedido in _context.DetallePedidos on pedido.Id equals detallepedido.IdPedidoFk
                                 join producto in _context.Productos on detallepedido.IdProductoFk equals producto.Id
                                 select new ClientesCantidadPedidoDto
                                 {
                                     Nombre = cliente.NombreCliente,
                                     Cantidad = 0
                                 })
                            .ToListAsync();
    }
}