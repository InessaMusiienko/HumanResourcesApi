using HumanResources.Models;
using HumanResources.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace HumanResources.Controllers
{
    public class EmployeeController : Controller
    {
        private HttpService _http;

        public EmployeeController(HttpService http)
        {
            this._http = http;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            
            if (User.IsInRole("Member"))
            {
                var user = User.FindFirstValue(ClaimTypes.Email);

                string apiRoute = $"/employees/getemployeeinfo/{user}";
                var employee = _http.Get<AllEmployeeViewModel>(apiRoute);
                
                if(employee != null)
                {
                    return View("GetEmployeeInfo", employee);
                }
                 
            }
            else
            {
                string apiRoute = "/employees/getemployees";
                var employeeList = _http.Get<List<AllEmployeeViewModel>>(apiRoute);

                if(employeeList != null)
                {
                    return View(employeeList);
                }  
            }
            return BadRequest();            
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
            
        }

        [Authorize(Roles = "Hr")]
        [HttpPost]
        public IActionResult CreateEmployee(AllEmployeeViewModel model)
        {
            var apiRoute = "/employees/postemployee";
            var success = _http.Post<AllEmployeeViewModel>(apiRoute, model);

            if (success)
            {
                return RedirectToAction("GetAllEmployees");
            }
                
            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiRoute = $"/employees/getemployee/{id}";

            var employee = _http.Get<AllEmployeeViewModel>(apiRoute);
            if(employee != null)
            {
                return View(employee);
            }

            return View();    
        }

        [Authorize(Roles = "Hr")]
        [HttpPost]
        public IActionResult Edit(int id, AllEmployeeViewModel model)
        {
            var apiRoute = $"/employees/putemployee/{id}";

            var success = _http.Put<AllEmployeeViewModel>(apiRoute, model);
            if(success)
            {
                return RedirectToAction("GetAllEmployees");
            }

            return View();           
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiRoute = $"/employees/getemployee/{id}";

            var employee = _http.Get<AllEmployeeViewModel>(apiRoute);

            if (employee != null)
            {
                return View(employee);
            }

            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var apiRoute = $"/employees/delete/{id}";

            var success = _http.Delete(apiRoute);

            if (success)
            {
                return RedirectToAction("GetAllEmployees");
            }

            return View();
        }

    }
}
