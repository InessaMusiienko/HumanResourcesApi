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
using System.Globalization;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly HrappDbContext _context;

        public EmployeesController(HrappDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employee)
        {
            var depart = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == employee.Department.ToLower());

            var jobtitle = await _context.JobTitles
                .FirstOrDefaultAsync(j => j.JobName.ToLower() == employee.JobTitle.ToLower());

            //var isHireDateValid = IsValidDate(employee.HireDate.ToString());

            if (depart == null || jobtitle == null)
            {
                return BadRequest();
            }

            Employee newEmployee = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ContactNumber = employee.ContactNumber,
                Email = employee.Email,
                Adress = employee.Adress,
                HireDate = DateTime.Now,
                Department = depart,
                JobTitle = jobtitle
            };

            _context.Employees.Add(newEmployee);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(newEmployee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }

        public static bool IsValidDate(string value)
        {
            string[] formats = { "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy H:mm:ss" };
            DateTime parsedDate;
            var isValidFormat = DateTime.TryParseExact(value, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);

            if (isValidFormat)
            {
                string.Format("{0:d/MM/yyyy}", parsedDate);
                return true;
            }
            else
            {
                return false;
            }
        }

        //private bool IsDateBeforeOrToday(string input)
        //{

        //    bool result = true;

        //    if (input != null)
        //    {
        //        DateTime dTCurrent = DateTime.Now;
        //        int currentDateValues = Convert.ToInt32(dTCurrent.ToString("MMddyyyy"));
        //        int inputDateValues = Convert.ToInt32(input.Replace("/", ""));

        //        result = inputDateValues <= currentDateValues;
        //    }
        //    else
        //    {
        //        result = true;
        //    }

        //    return result;
        //}
    }
}
