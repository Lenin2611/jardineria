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
    public class ProductoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JardineriaContext _context;

        public ProductoController(IUnitOfWork unitOfWork, IMapper mapper, JardineriaContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            var results = await _unitOfWork.Productos.GetAllAsync();
            return _mapper.Map<List<ProductoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(string id)
        {
            var result = await _unitOfWork.Productos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<ProductoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Post(ProductoDto resultDto)
        {
            var result = _mapper.Map<Producto>(resultDto);
            _unitOfWork.Productos.Add(result);
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
        public async Task<ActionResult<ProductoDto>> Put(string id, [FromBody] ProductoDto resultDto)
        {
            var exists = await _unitOfWork.Productos.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (resultDto.Id == string.Empty)
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
            return _mapper.Map<ProductoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.Productos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Productos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

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

        [HttpGet("nopedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNoPedidos()
        {
            var query = (from producto in _context.Productos
                            join detallepedido in _context.DetallePedidos on producto.Id equals detallepedido.IdProductoFk
                            join gama in _context.GamaProductos on producto.IdGamaFk equals gama.Id
                            where! _context.Productos.Any(x => x.Id == detallepedido.IdProductoFk)
                            select new
                            {
                                NombreCliente = producto.Nombre,
                                NombreRepresentante = gama.DescripcionTexto,
                                CiudadRepresentante = gama.Imagen
                            });
            List<object> result = query.ToList<object>();
            return Ok(result);
        }


        // [HttpGet("20masvendidos")] // 2611
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public async Task<ActionResult<List<ProductoCantidadPrecioDto>>> GetMas3000()
        // {
        //     var results = await (from producto in _context.Productos
        //                         join detallepedido in _context.DetallePedidos on producto.Id equals detallepedido.IdProductoFk
        //                         select new ProductoCantidadPrecioDto
        //                         {
        //                             Nombre = producto.Nombre,
        //                             Cantidad = detallepedido.Cantidad,
        //                             Precio = producto.PrecioVenta
        //                         }).ToListAsync();

        //     var productoTotal = new List<ProductoCantidadPrecioDto>();
        //     foreach (var p in results)
        //     {
        //         var productoExistente = productoTotal.FirstOrDefault(x => x.Nombre == p.Nombre);

        //         if (productoExistente != null)
        //         {
        //             productoExistente.Cantidad += p.Cantidad;
        //         }
        //         else
        //         {
        //             productoTotal.Add(new ProductoCantidadPrecioDto
        //             {
        //                 Nombre = p.Nombre,
        //                 Cantidad = 0
        //             });
        //         }
        //     }
        //     var z = productoTotal.OrderByDescending(x => x.Cantidad).Take(20).ToList();
        //     return z;
        // }
    }
}
