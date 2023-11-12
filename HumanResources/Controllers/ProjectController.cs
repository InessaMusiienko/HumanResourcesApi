using HumanResources.Models;
using HumanResources.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace HumanResources.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private HttpService _http;

        public ProjectController(HttpService http)
        {
            this._http = http;
        }

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }
            string apiRoute = "/projects/getprojects";

            var projectList = _http.Get<List<ProjectViewModel>>(apiRoute);

            return View(projectList);
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult AddEmployee(string projectId)
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }
            var apiRoute = $"/employees/getemployeesbyproject/{projectId}";

            var employeeList = _http.Get<List<AllEmployeeViewModel>>(apiRoute);
            
            ViewBag.ProjectId = projectId;
            
            return View(employeeList);
        }

        [Authorize(Roles = "Hr")]
        [HttpGet] 
        public IActionResult AddEmployeeToProject(int id, int projectId) //check
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }

            EmployeeProjectDataModel model = new EmployeeProjectDataModel
            {
                EmployeeId = id,
                ProjectId = projectId
            };

            var apiRoute = "/projects/addemployeetoproject";
            var success = _http.Post<EmployeeProjectDataModel>(apiRoute, model);

            if (success)
            {
                ViewBag.ProjectId = projectId;
                return RedirectToAction("Details", model.ProjectId);
            }

            return RedirectToAction("GetAllProjects");
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpPost]
        public IActionResult Create(ProjectViewModel model)
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }

            var apiRoute = "/projects/postproject";
            var success = _http.Post<ProjectViewModel>(apiRoute, model);

            if (success)
            {
                return RedirectToAction("GetAllProjects");
            }

            return View();            
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }

            var apiRoute = $"/employees/getprojectemployees/{id}";
            var employees = _http.Get<List<AllEmployeeViewModel>>(apiRoute);

            ViewBag.ProjectId = id;
            return View(employees);
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiRoute = $"/projects/getproject/{id}";

            var project = _http.Get<ProjectViewModel>(apiRoute);

            if (project != null)
            {
                return View(project);
            }

            return View();

        }

        [Authorize(Roles = "Hr")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var apiRoute = $"/projects/delete/{id}";

            var success = _http.Delete(apiRoute);

            if (success)
            {
                return RedirectToAction("GetAllProjects");
            }

            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiRoute = $"/projects/getproject/{id}";

            var project = _http.Get<ProjectViewModel>(apiRoute);
            if (project != null)
            {
                return View(project);
            }

            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpPost]
        public IActionResult Edit(int id, ProjectViewModel model)
        {
            var apiRoute = $"/projects/putproject/{id}";

            var success = _http.Put<ProjectViewModel>(apiRoute, model);
            if (success)
            {
                return RedirectToAction("GetAllProjects");
            }

            return View();
        }
    }
}
