using HumanResources.Models;
using HumanResources.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HumanResources.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private HttpService _http;

        public DepartmentController(HttpService http)
        {
            this._http = http;
        }

        [HttpGet]
        public IActionResult GetAllDepartments(string SearchString="")
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }
            string apiRoute = $"/departments/getdepartments?searchstring={SearchString}";

            var departmentList = _http.Get<List<DepartmentViewModel>>(apiRoute);

            return View(departmentList);
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (!User.Identity.IsAuthenticated) { return BadRequest(); }
            var apiRoute = $"/employees/getdepartmentemployees/{id}";

            var employees = _http.Get<List<AllEmployeeViewModel>>(apiRoute);
               
            return View(employees);
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpPost]
        public IActionResult CreateDepartment(DepartmentViewModel model)
        {
            var apiRoute = "/departments/postdepartment";
            var success = _http.Post<DepartmentViewModel>(apiRoute, model);

            if (success)
            {
                return RedirectToAction("GetAllDepartments");
            }

            return RedirectToAction("GetAllDepartments");
        }
    }
}
