using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    public class EmpleadoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JardineriaContext _context;

        public EmpleadoController(IUnitOfWork unitOfWork, IMapper mapper, JardineriaContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
        {
            var results = await _unitOfWork.Empleados.GetAllAsync();
            return _mapper.Map<List<EmpleadoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Get(int id)
        {
            var result = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<EmpleadoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmpleadoDto>> Post(EmpleadoDto resultDto)
        {
            var result = _mapper.Map<Empleado>(resultDto);
            _unitOfWork.Empleados.Add(result);
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
        public async Task<ActionResult<EmpleadoDto>> Put(int id, [FromBody] EmpleadoDto resultDto)
        {
            var exists = await _unitOfWork.Empleados.GetByIdAsync(id);
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
            return _mapper.Map<EmpleadoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Empleados.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        // [HttpGet("clientesPagosRepresentantesCiudad")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public IActionResult GetClientesPagosRepresentantesCiudad()
        // {
        //     var query = (from empleado in _context.Empleados
        //                     join cliente in _context.Clientes on empleado.Id equals cliente.IdEmpleadoRepresentanteVentasFk
        //                     join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id
        //                     where empleado.Id 
        //                     select new
        //                     {
        //                         NombreCliente = cliente.Nombre,
        //                         NombreRepresentante = representante.Nombre,
        //                         CiudadRepresentante = ciudadRepresentante.Nombre
        //                     }).Distinct();
        
        //     List<object> result = query.ToList<object>();
        
        //     return Ok(result);
        // }
    }
}