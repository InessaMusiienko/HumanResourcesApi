﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HumanResourcesApi.Data;
using HumanResourcesApi.Models.Entities;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly HrappDbContext _context;

        public SalariesController(HrappDbContext context)
        {
            _context = context;
        }

        // GET: api/Salaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salary>>> GetSalaries()
        {
          if (_context.Salaries == null)
          {
              return NotFound();
          }
            return await _context.Salaries.ToListAsync();
        }

        // GET: api/Salaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salary>> GetSalary(int id)
        {
          if (_context.Salaries == null)
          {
              return NotFound();
          }
            var salary = await _context.Salaries.FindAsync(id);

            if (salary == null)
            {
                return NotFound();
            }

            return salary;
        }

        // PUT: api/Salaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalary(int id, Salary salary)
        {
            if (id != salary.SalaryId)
            {
                return BadRequest();
            }

            _context.Entry(salary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(id))
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

        // POST: api/Salaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Salary>> PostSalary(Salary salary)
        {
          if (_context.Salaries == null)
          {
              return Problem("Entity set 'HrappDbContext.Salaries'  is null.");
          }
            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalary", new { id = salary.SalaryId }, salary);
        }

        // DELETE: api/Salaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            if (_context.Salaries == null)
            {
                return NotFound();
            }
            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalaryExists(int id)
        {
            return (_context.Salaries?.Any(e => e.SalaryId == id)).GetValueOrDefault();
        }
    }
}
