using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GamaProductoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamaProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
        {
            var results = await _unitOfWork.GamaProductos.GetAllAsync();
            return _mapper.Map<List<GamaProductoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GamaProductoDto>> Get(string id)
        {
            var result = await _unitOfWork.GamaProductos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<GamaProductoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GamaProductoDto>> Post(GamaProductoDto resultDto)
        {
            var result = _mapper.Map<GamaProducto>(resultDto);
            _unitOfWork.GamaProductos.Add(result);
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
        public async Task<ActionResult<GamaProductoDto>> Put(string id, [FromBody] GamaProductoDto resultDto)
        {
            var exists = await _unitOfWork.GamaProductos.GetByIdAsync(id);
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
            return _mapper.Map<GamaProductoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.GamaProductos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.GamaProductos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}