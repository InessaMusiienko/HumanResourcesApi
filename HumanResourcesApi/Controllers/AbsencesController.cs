using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HumanResourcesApi.Data;
using HumanResourcesApi.Models.Entities;
using HumanResourcesApi.Models.ApiModels;
using HumanResourcesApi.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using HumanResources.Models;
using System.Globalization;

namespace HumanResourcesApi.Controllers
{
    public enum Types
    {
        [Display(Name = "Select type of absence")]
        SelectType = 0,
        BasicPaid = 1,
        Unpaid = 2,
        GeneralDisease = 3
    }

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AbsencesController : ControllerBase
    {
        private readonly HrappDbContext _context;

        public AbsencesController(HrappDbContext context)
        {
            _context = context;
        }

        // GET: api/Absences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbsenceAllView>>> GetAbsences()
        {
          if (_context.Absences == null)
          {
              return NotFound();
          }
            var absences =  await _context.Absences.Select(x => new AbsenceAllView
            {
                Id = x.AbsenceId,
                FirstName = x.Employee.FirstName,
                LastName = x.Employee.LastName,
                EmployeeId = x.Employee.EmployeeId,
                Status = x.Status,
                Reason = x.Reason,
                StartDate = x.StartDate.ToString(),
                EndDate = x.EndDate.ToString(),
                TypeOfAbsence = ((Types) x.TypeOfAbsence).ToString()
            }).ToListAsync();

            return absences;
        }

        // GET: api/Absences/5
        [HttpGet("{user}")]
        public async Task<ActionResult<IEnumerable<AbsencePostModel>>> GetAbsence(string user)
        {
            if (_context.Absences == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user);
            var absences = await _context.Absences.Where(a=>a.EmployeeId == employee.EmployeeId)
                .Select(a => new AbsencePostModel
                {

                    Status = a.Status,
                    StartDate = a.StartDate.ToString(),
                    EndDate = a.EndDate.ToString(),
                    TypeOfAbsence = a.TypeOfAbsence,
                    Reason = a.Reason
                }).ToListAsync();

            if (absences == null)
            {
                return NotFound();
            }

            return absences;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AbsenceAllView>> GetAbsenceById(int id)
        {
            if (_context.Absences == null)
            {
                return NotFound();
            }

            var absences = await _context.Absences
                .Select(x => new AbsenceAllView
                {
                    Id = x.AbsenceId,
                    FirstName = x.Employee.FirstName,
                    LastName = x.Employee.LastName,
                    EmployeeId = x.Employee.EmployeeId,
                    Status = x.Status,
                    Reason = x.Reason,
                    StartDate = x.StartDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    TypeOfAbsence = ((Types)x.TypeOfAbsence).ToString()
                }).ToListAsync();

            var absence = absences.FirstOrDefault(a => a.Id == id);

            return absence;
        }

        // PUT: api/Absences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var absence = await _context.Absences.FirstOrDefaultAsync(a => a.AbsenceId == id);

            if (absence == null)
            {
                return BadRequest();
            }

            absence.Status = "Approved";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbsenceExists(id))
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

        // POST: api/Absences
        [HttpPost("{user}")]
        public async Task<ActionResult<AbsencePostModel>> PostAbsence(string user, AbsencePostModel absence)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user);

            Absence newAbsence = new Absence()
            {
                EmployeeId = employee.EmployeeId,
                StartDate = DateTime.ParseExact(absence.StartDate,"yyyy-MM-dd",CultureInfo.InvariantCulture),
                Status = "Waiting",
                EndDate = DateTime.ParseExact(absence.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Reason = absence.Reason,
                TypeOfAbsence = absence.TypeOfAbsence,
            };

            _context.Absences.Add(newAbsence);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: api/Absences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Absences == null)
            {
                return NotFound();
            }
            var absence = await _context.Absences.FindAsync(id);
            if (absence == null)
            {
                return NotFound();
            }

            _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AbsenceExists(int id)
        {
            return (_context.Absences?.Any(e => e.AbsenceId == id)).GetValueOrDefault();
        }
    }
}
