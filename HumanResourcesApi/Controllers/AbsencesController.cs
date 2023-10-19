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
using HumanResourcesApi.Models;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Absence>> GetAbsence(int id)
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

            return absence;
        }

        // PUT: api/Absences/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsence(int id, AbsenceDTO absence)
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
        [HttpPost]
        public async Task<ActionResult<Absence>> PostAbsence(AbsenceDTO absence)
        {
            if (new string[] { absence.EmployeeFullName, absence.StartDate.ToString(), 
                absence.EndDate.ToString(), absence.TypeOfAbsence }.Any(x => string.IsNullOrEmpty(x)))
            {
                return BadRequest();
            }

            //var employeeName = absence.EmployeeFullName[0].ToString();
            //var employeeSurname = absence.EmployeeFullName[1].ToString();
            //var employee = await _context.Employees
            //    .FirstOrDefaultAsync((e => e.FirstName.ToLower() == employeeName.ToLower())
            //    && (e => e.LastName.ToLower() == employeeSurname.ToLower()));

            //if (employee == null)
            //{
            //    return BadRequest();
            //}

            //Absence newAbsence = new Absence()
            //{
            //    //EmployeeId = employee,
            //    StartDate = absence.StartDate,
            //    Status = absence.Status,
            //    EndDate = absence.EndDate,
            //    Reason = absence.Reason,
            //    //TypeOfAbsence = Enum.Parse<TypeOfAbsence>(absence.TypeOfAbsence)
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
