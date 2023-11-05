using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HumanResourcesApi.Data;
using HumanResourcesApi.Models.Entities;
using HumanResourcesApi.Models.ApiModels;
using System.Globalization;
using HumanResources.Models;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Authorization;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly HrappDbContext _context;
        public EmployeesController(HrappDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllEmployeeViewModel>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.Select(e=> new AllEmployeeViewModel
            {
                Id = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.Department.DepartmentName,
                JobTitle = e.JobTitle.JobName,
                Email = e.Email,
                ContactNumber = e.ContactNumber,
                Adress = e.Adress
            })              
            .ToListAsync();

            return employees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AllEmployeeViewModel>> GetEmployee(int id)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }

            var employees = await _context.Employees.Select(e => new AllEmployeeViewModel
            {
                Id = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.Department.DepartmentName,
                JobTitle = e.JobTitle.JobName,
                Email = e.Email,
                ContactNumber = e.ContactNumber,
                Adress = e.Adress
            })
              .ToListAsync();

            var employee = employees.FirstOrDefault(e => e.Id == id);
            return employee;
        }

        [HttpGet("{user}")]
        public async Task<ActionResult<AllEmployeeViewModel>> GetEmployeeInfo(string user)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            
            var employees = await _context.Employees.Select(e => new AllEmployeeViewModel
            {
                Id = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.Department.DepartmentName,
                JobTitle = e.JobTitle.JobName,
                Email = e.Email,
                ContactNumber = e.ContactNumber,
                Adress = e.Adress
            })
              .ToListAsync();

            var employee = employees.FirstOrDefault(e => e.Email == user);
            return employee;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AllEmployeeViewModel>>> GetDepartmentEmployees(int id)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null) { return BadRequest(); }

            var employees = await _context.Employees.Select(e => new AllEmployeeViewModel
            {
                Id = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.Department.DepartmentName,
                JobTitle = e.JobTitle.JobName,
                Email = e.Email,
                ContactNumber = e.ContactNumber,
                Adress = e.Adress
            })
            .ToListAsync();

            var employeesByDepartment = employees.Where(e => e.Department == department.DepartmentName).ToList();

            return employeesByDepartment;
        }


        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, AllEmployeeViewModel updatedEmployee)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee==null)
            {
                return BadRequest();
            }

            //check if null or empty

            var dep = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == updatedEmployee.Department.ToLower());
            var jobtitle = await _context.JobTitles
                .FirstOrDefaultAsync(x => x.JobName.ToLower() == updatedEmployee.JobTitle.ToLower());

            if(dep ==null || jobtitle == null) { return BadRequest(); }

            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Department = dep;
            employee.JobTitle = jobtitle;
            employee.ContactNumber = updatedEmployee.ContactNumber;
            employee.Email = updatedEmployee.Email;
            employee.Adress = updatedEmployee.Adress;

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

            if (depart == null || jobtitle == null)
            {
                return BadRequest();
            }

            var employeeAlreadyExist = await _context.Employees
                .FirstOrDefaultAsync(e => e.FirstName == employee.FirstName);
            if(employeeAlreadyExist != null)
            {
                if (employeeAlreadyExist.LastName == employee.LastName)
                {
                    return BadRequest();
                }
            }

            //var isHireDateValid = IsValidDate(employee.HireDate.ToString());            

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
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
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
