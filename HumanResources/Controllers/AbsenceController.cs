using HumanResources.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace HumanResources.Controllers
{
    [Authorize]
    public class AbsenceController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7175/api");
        private readonly HttpClient _client;

        public AbsenceController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        private string GetUserId()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return id;
        }

        [HttpGet]
        public IActionResult GetAllAbsences()
        {
            var id = GetUserId();

            List<AbsenceViewModel> absenceList = new List<AbsenceViewModel>();
            HttpResponseMessage responce = _client.GetAsync(_client.BaseAddress + "/absences/getabsence" + id).Result;

            if (responce.IsSuccessStatusCode)
            {
                string data = responce.Content.ReadAsStringAsync().Result;
                absenceList = JsonConvert.DeserializeObject<List<AbsenceViewModel>>(data);
            }

            return View(absenceList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AbsenceViewModel model)
        {
            var id = GetUserId();

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress 
                    + "/absences/postabsence/" + id, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Absence Added. Wait for approval!";
                    return RedirectToAction("GetAllAbsences");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }

            return View();
        }
    }
}
