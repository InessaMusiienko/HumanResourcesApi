using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HumanResourcesApi.Data;
using HumanResourcesApi.Models.Entities;
using HumanResourcesApi.Models.ApiModels;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly HrappDbContext _context;

        public DepartmentsController(HrappDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
          if (_context.Departments == null)
          {
              return NotFound();
          }
          
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Departments/departmentName
        [HttpGet("{departmentName}")]
        public async Task<ActionResult<Department>> GetDepartment(string departmentName)
        {
          if (_context.Departments == null)
          {
              return NotFound();
          }
            var department = await _context.Departments.Include(x=>x.Employees)
                .FirstOrDefaultAsync(x => x.DepartmentName.ToLower() == departmentName.ToLower());

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDTO updatedDepartment)
        {
            if (!_context.Departments.Any(d=>d.DepartmentId == id))
            {
                return BadRequest();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            department!.DepartmentName = updatedDepartment.DepartmentName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDTO department)
        {
            Department newDepartment = new Department()
            {
                DepartmentName = department.DepartmentName
            };

            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();

            return Ok(newDepartment);
        }

        // DELETE: api/Departments/departmentName

        [HttpDelete("{departmentName}")]
        public async Task<IActionResult> DeleteDepartment(string departmentName)
        {
            if (_context.Departments == null)
            {
                return NotFound();
            }
            var department = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName.ToLower() == departmentName.ToLower());
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool DepartmentExists(int id)
        {
            return (_context.Departments?.Any(e => e.DepartmentId == id)).GetValueOrDefault();
        }
    }
}
