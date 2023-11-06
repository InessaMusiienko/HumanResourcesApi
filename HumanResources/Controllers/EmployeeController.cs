using HumanResources.Models;
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
        Uri baseAddress = new Uri("https://localhost:7175/api");
        private readonly HttpClient _client;

        public EmployeeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Member"))
                {
                    var user = User.FindFirstValue(ClaimTypes.Email);
                    AllEmployeeViewModel employee = new AllEmployeeViewModel();
                    HttpResponseMessage response = _client
                    .GetAsync(_client.BaseAddress + $"/employees/getemployeeinfo/{user}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        employee = JsonConvert.DeserializeObject<AllEmployeeViewModel>(data);
                    }
                    return View("GetEmployeeInfo", employee);
                }
                else
                {
                    List<AllEmployeeViewModel> employeeList = new List<AllEmployeeViewModel>();
                    HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/employees/getemployees").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        employeeList = JsonConvert.DeserializeObject<List<AllEmployeeViewModel>>(data);
                    }

                    return View(employeeList);
                }
            }
            else
            {
                return BadRequest();
            }
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
        public IActionResult CreateEmployee(AllEmployeeViewModel model)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = _client
                        .PostAsync(_client.BaseAddress + "/employees/postemployee", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Employee Created.";
                        return RedirectToAction("GetAllEmployees");
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();
                }
                return this.RedirectToAction("GetAllEmployees");
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
                    AllEmployeeViewModel employee = new AllEmployeeViewModel();
                    HttpResponseMessage response = _client
                        .GetAsync(_client.BaseAddress + "/employees/getemployee/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        employee = JsonConvert.DeserializeObject<AllEmployeeViewModel>(data);
                    }
                    return View(employee);
                }
                catch (Exception)
                {
                    return View();
                }
            }
            else { return BadRequest(); }
        }

        [HttpPost]
        public IActionResult Edit(int id, AllEmployeeViewModel model)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _client
                        .PutAsync(_client.BaseAddress + $"/employees/putemployee/{id}", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Employee info updated";
                        return RedirectToAction("GetAllEmployees");
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
        public IActionResult Delete(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    AllEmployeeViewModel employee = new AllEmployeeViewModel();
                    HttpResponseMessage response = _client
                        .GetAsync(_client.BaseAddress + "/employees/getemployee/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        employee = JsonConvert.DeserializeObject<AllEmployeeViewModel>(data);
                    }
                    return View(employee);
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return BadRequest();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    HttpResponseMessage responsw = _client
                            .DeleteAsync(_client.BaseAddress + "/employees/delete/" + id).Result;

                    if (responsw.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Employee deleted.";
                        return RedirectToAction("GetAllEmployees");
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

    }
}
