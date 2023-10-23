using HumanResources.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

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
            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/employees/GetEmployees").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                employeeList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
            }

            return View(employeeList);
        }
    }
}
