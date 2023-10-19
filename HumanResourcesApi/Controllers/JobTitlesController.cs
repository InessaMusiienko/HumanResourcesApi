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
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController : ControllerBase
    {
        private readonly HrappDbContext _context;

        public JobTitlesController(HrappDbContext context)
        {
            _context = context;
        }

        // GET: api/JobTitles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTitle>>> GetJobTitles()
        {
          if (_context.JobTitles == null)
          {
              return NotFound();
          }
            return await _context.JobTitles.ToListAsync();
        }

        // GET: api/JobTitles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTitle>> GetJobTitle(int id)
        {
          if (_context.JobTitles == null)
          {
              return NotFound();
          }
            var jobTitle = await _context.JobTitles.FindAsync(id);

            if (jobTitle == null)
            {
                return NotFound();
            }

            return jobTitle;
        }

        // PUT: api/JobTitles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTitle(int id, JobTitle jobTitle)
        {
            if (id != jobTitle.JobTitleId)
            {
                return BadRequest();
            }

            _context.Entry(jobTitle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTitleExists(id))
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

        // POST: api/JobTitles
        [HttpPost]
        public async Task<ActionResult<JobTitle>> PostJobTitle(JobTitleDTO jobTitle)
        {
            JobTitle newJobTitle = new JobTitle()
            {
                JobName = jobTitle.JobName,
                Salary = new Salary { Amount=jobTitle.Salary },
                WorkingHours = jobTitle.WorkingHours
                //StartDate = Convert.ToDateTime(jobTitle.StartDate)
            };

            _context.JobTitles.Add(newJobTitle);
            await _context.SaveChangesAsync();

            return Ok(newJobTitle);
        }

        // DELETE: api/JobTitles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTitle(int id)
        {
            if (_context.JobTitles == null)
            {
                return NotFound();
            }
            var jobTitle = await _context.JobTitles.FindAsync(id);
            if (jobTitle == null)
            {
                return NotFound();
            }

            _context.JobTitles.Remove(jobTitle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobTitleExists(int id)
        {
            return (_context.JobTitles?.Any(e => e.JobTitleId == id)).GetValueOrDefault();
        }
    }
}
