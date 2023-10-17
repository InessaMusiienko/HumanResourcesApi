//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using HumanResourcesApi.Data;
//using HumanResourcesApi.Models.Entities;
//using HumanResourcesApi.Models.ApiModels;

//namespace HumanResourcesApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeesInfoesController : ControllerBase
//    {
//        private readonly HrappDbContext _context;

//        public EmployeesInfoesController(HrappDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/EmployeesInfoes
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<EmployeesInfo>>> GetEmployeesInfos()
//        {
//          if (_context.EmployeesInfos == null)
//          {
//              return NotFound();
//          }
//            return await _context.EmployeesInfos.ToListAsync();
//        }

//        // GET: api/EmployeesInfoes/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<EmployeesInfo>> GetEmployeesInfo(int id)
//        {
//          if (_context.EmployeesInfos == null)
//          {
//              return NotFound();
//          }
//            var employeesInfo = await _context.EmployeesInfos.FindAsync(id);

//            if (employeesInfo == null)
//            {
//                return NotFound();
//            }

//            return employeesInfo;
//        }

//        // PUT: api/EmployeesInfoes/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutEmployeesInfo(int id, EmployeesInfo employeesInfo)
//        {
//            if (id != employeesInfo.EmployeeId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(employeesInfo).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!EmployeesInfoExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/EmployeesInfoes
//        [HttpPost]
//        public async Task<ActionResult<EmployeesInfo>> PostEmployeesInfo(EmployeeDTO employee)
//        {
//            var depart = await _context.Departments
//                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == employee.Department.ToLower());

//            var jobtitle = await _context.JobTitles
//                .FirstOrDefaultAsync(j => j.JobName.ToLower() == employee.JobTitle.ToLower());

//            var isHireDateValid = IsDateBeforeOrToday(employee.HireDate.ToString());

//            if (depart == null || jobtitle == null || !isHireDateValid)
//            {
//                return BadRequest();
//            }

//            EmployeesInfo employeeinfo = new EmployeesInfo()
//            {
//                FirstName = employee.FirstName,
//                LastName = employee.LastName,
//                ContactNumber = employee.ContactNumber,
//                Email = employee.Email,
//                Adress = employee.Adress,
//                HireDate = employee.HireDate,
//                Department = depart,
//                JobTitle = jobtitle
//            };

//            _context.EmployeesInfos.Add(employeeinfo);

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                return NotFound();
//            }

//            return Ok(employeeinfo);
//        }

//        // DELETE: api/EmployeesInfoes/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEmployeesInfo(int id)
//        {
//            if (_context.EmployeesInfos == null)
//            {
//                return NotFound();
//            }
//            var employeesInfo = await _context.EmployeesInfos.FindAsync(id);
//            if (employeesInfo == null)
//            {
//                return NotFound();
//            }

//            _context.EmployeesInfos.Remove(employeesInfo);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool EmployeesInfoExists(int id)
//        {
//            return (_context.EmployeesInfos?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
//        }

//        public static bool IsDateBeforeOrToday(string input)
//        {
//            bool result = true;

//            if (input != null)
//            {
//                DateTime dTCurrent = DateTime.Now;
//                int currentDateValues = Convert.ToInt32(dTCurrent.ToString("MMddyyyy"));
//                int inputDateValues = Convert.ToInt32(input.Replace("/", ""));

//                result = inputDateValues <= currentDateValues;
//            }
//            else
//            {
//                result = true;
//            }

//            return result;
//        }
//    }
//}
