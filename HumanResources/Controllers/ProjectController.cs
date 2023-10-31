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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel model)
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
            //return View();
            return this.RedirectToAction("GetAllProjects");
        }
    }
}
