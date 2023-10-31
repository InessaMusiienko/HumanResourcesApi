using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HumanResourcesApi.Data;
using HumanResourcesApi.Models.Entities;
using HumanResourcesApi.Models.ApiModels;
using HumanResourcesApi.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using HumanResources.Models;

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
        public async Task<ActionResult<IEnumerable<Absence>>> GetAbsences()
        {
          if (_context.Absences == null)
          {
              return NotFound();
          }
            return await _context.Absences.ToListAsync();
        }

        // GET: api/Absences/5
        [HttpGet("{user}")]
        public async Task<ActionResult<IEnumerable<Absence>>> GetAbsence(string user)
        {
          if (_context.Absences == null)
          {
              return NotFound();
          }
            var absence = await _context.Absences.Where(a=>a.EmployeeId == int.Parse(user)).ToListAsync();

            if (absence == null)
            {
                return NotFound();
            }

            return absence;
        }

        // PUT: api/Absences/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsence(AbsenceDTO absence, int id)
        {
            _context.Entry(absence).State = EntityState.Modified;

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
        [HttpPost("{id}")]
        public async Task<ActionResult<AbsencePostModel>> PostAbsence(string id, AbsencePostModel absence)
        {
            //if (new string[] { absence.StartDate.ToString(), 
            //    absence.EndDate.ToString(), absence.TypeOfAbsence }.Any(x => string.IsNullOrEmpty(x)))
            //{
            //    return BadRequest();
            //}

            //Absence newAbsence = new Absence()
            //{
            //    EmployeeId = int.Parse(id),
            //    StartDate = absence.StartDate,
            //    Status = "Not approved",
            //    EndDate = absence.EndDate,
            //    Reason = absence.Reason,
            //    TypeOfAbsence = (int)Enum.Parse<TypeOfAbsence>(absence.TypeOfAbsence)
            //};

            //_context.Absences.Add(newAbsence);

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
        public async Task<IActionResult> DeleteAbsence(int id)
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
