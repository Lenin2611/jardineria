using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repositories
{
    public class EmpleadoRepository : GenericRepositoryInt<Empleado>, IEmpleado
    {
        private readonly JardineriaContext _context;

        public EmpleadoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
    }
}