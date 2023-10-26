using HumanResources.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

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
    }
}
