﻿using HumanResources.Models;
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
        Uri baseAddress = new Uri("https://localhost:7175/api");
        private readonly HttpClient _client;

        public ProjectController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        //[Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            List<ProjectViewModel> projectList = new List<ProjectViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/projects/GetProjects").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                projectList = JsonConvert.DeserializeObject<List<ProjectViewModel>>(data);
            }

            return View(projectList);
        }

        [HttpGet]
        public IActionResult AddEmployee(string projectId)
        {
            List<AllEmployeeViewModel> employeeList = new List<AllEmployeeViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/employees/getemployeesbyproject/" + projectId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                employeeList = JsonConvert.DeserializeObject<List<AllEmployeeViewModel>>(data);
            }

            return View(employeeList);
        }

        [HttpPost]
        public IActionResult AddEmployeeToProject(int id, int projectId)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            AllEmployeeViewModel employee = new AllEmployeeViewModel();
            HttpResponseMessage response = _client
            .GetAsync(_client.BaseAddress + $"/employees/getemployee/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                employee = JsonConvert.DeserializeObject<AllEmployeeViewModel>(data);
            }
            return View("AddEmployeeToProject", employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (User.IsInRole("Hr"))
            {
                return View();
            }
            else { return BadRequest(); }
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel model)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = _client
                        .PostAsync(_client.BaseAddress + "/projects/postproject", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetAllProjects");
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();
                }
                return View();
            }
            else { return BadRequest(); }
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                List<AllEmployeeViewModel> employees = new List<AllEmployeeViewModel>();
                HttpResponseMessage response = _client
                    .GetAsync(_client.BaseAddress + $"/employees/getprojectemployees/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.ProjectId = id;
                    string data = response.Content.ReadAsStringAsync().Result;
                    employees = JsonConvert.DeserializeObject<List<AllEmployeeViewModel>>(data);
                }
                return View(employees);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    ProjectViewModel project = new ProjectViewModel();
                    HttpResponseMessage response = _client
                        .GetAsync(_client.BaseAddress + "/projects/getproject/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        project = JsonConvert.DeserializeObject<ProjectViewModel>(data);
                    }
                    return View(project);
                }
                catch (Exception)
                {
                    return View();
                }
            }
            else { return BadRequest(); }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    HttpResponseMessage responsw = _client
                            .DeleteAsync(_client.BaseAddress + "/projects/delete/" + id).Result;

                    if (responsw.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Project deleted.";
                        return RedirectToAction("GetAllProjects");
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();
                }
                return View();
            }
            else { return BadRequest(); }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    ProjectViewModel project = new ProjectViewModel();
                    HttpResponseMessage response = _client
                        .GetAsync(_client.BaseAddress + "/projects/getproject/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        project = JsonConvert.DeserializeObject<ProjectViewModel>(data);
                    }
                    return View(project);
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Edit(int id, ProjectViewModel model)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _client
                        .PutAsync(_client.BaseAddress + $"/projects/putproject/{id}", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Project info updated";
                        return RedirectToAction("GetAllProjects");
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();
                }
                return View();
            }
            return BadRequest();
        }
    }
}
