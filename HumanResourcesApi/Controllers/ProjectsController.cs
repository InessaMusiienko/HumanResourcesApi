﻿using System;
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
    public class ProjectsController : ControllerBase
    {
        private readonly HrappDbContext _context;

        public ProjectsController(HrappDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDTO project)
        {
            if (new string[] { project.ProjectName, project.Duration.ToString(), project.Status }.Any(x => string.IsNullOrEmpty(x)))
            {
                return BadRequest();
            }

            var projectToAdd = new Project()
            {
                ProjectName = project.ProjectName,
                Duration = project.Duration,
                Status = project.Status,
                StartedOn = DateTime.UtcNow,
                EmployeesProjects = new List<EmployeeProject>()
            };

            _context.Projects.Add(projectToAdd);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction("GetProjects");
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
