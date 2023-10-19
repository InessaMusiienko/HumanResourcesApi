using HumanResources.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumanResources.Controllers
{
    public class DepartmentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7175/api");
        private readonly HttpClient _client;

        public DepartmentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<DepartmentViewModel> departmentList = new List<DepartmentViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/departments/GetDepartments").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                departmentList = JsonConvert.DeserializeObject<List<DepartmentViewModel>>(data);
            }

            return View(departmentList);
        }
    }
}
