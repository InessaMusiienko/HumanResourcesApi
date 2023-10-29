using HumanResources.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace HumanResources.Controllers
{
    [Authorize]
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
             
            List<AllEmployeeViewModel> employeeList = new List<AllEmployeeViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/employees/getemployees").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                employeeList = JsonConvert.DeserializeObject<List<AllEmployeeViewModel>>(data);
            }

            return View(employeeList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEmployee(AllEmployeeViewModel model)
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
            //return View();
            return this.RedirectToAction("GetAllEmployees");
        }

        [HttpGet]
        public IActionResult Edit(int id)
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

        [HttpPost]
        public IActionResult Edit(int id, AllEmployeeViewModel model)
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

        [HttpGet]
        public IActionResult Delete(int id)
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

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
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

    }
}
