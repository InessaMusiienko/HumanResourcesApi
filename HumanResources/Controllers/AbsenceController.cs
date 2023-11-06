﻿using HumanResources.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace HumanResources.Controllers
{
    public enum Types
    {
        [Display(Name = "Select type of absence")]
        SelectType = 0,
        BasicPaid = 1,
        Unpaid = 2,
        GeneralDisease = 3
    }

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
            var userId = GetUserId();

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Member"))
                {
                    var user = User.FindFirstValue(ClaimTypes.Email);

                    List<AbsenceAllView> absenceList = new List<AbsenceAllView>();
                    HttpResponseMessage responce = _client
                        .GetAsync(_client.BaseAddress + $"/absences/getabsence/{user}").Result;

                    if (responce.IsSuccessStatusCode)
                    {
                        string data = responce.Content.ReadAsStringAsync().Result;
                        absenceList = JsonConvert.DeserializeObject<List<AbsenceAllView>>(data);
                    }

                    return View(absenceList);
                }
                else
                {
                    List<AbsenceAllView> absenceList = new List<AbsenceAllView>();
                    HttpResponseMessage responce = _client
                        .GetAsync(_client.BaseAddress + "/absences/getabsences").Result;

                    if (responce.IsSuccessStatusCode)
                    {
                        string data = responce.Content.ReadAsStringAsync().Result;
                        absenceList = JsonConvert.DeserializeObject<List<AbsenceAllView>>(data);
                    }

                    return View(absenceList);
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
            if (User.IsInRole("Member"))
            {
                var types = from Types e in Enum.GetValues(typeof(Types))
                            select new
                            {
                                Id = (int)e,
                                Name = e.ToString()
                            };
                SelectList selectLists = new SelectList(types, "Id", "Name");
                ViewBag.Types = selectLists;
                return View();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create(AbsencePostModel model)
        {
            if (User.IsInRole("Member"))
            {

                var user = User.FindFirstValue(ClaimTypes.Email);

                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = _client.PostAsync(_client.BaseAddress
                        + $"/absences/postabsence/{user}", content).Result;

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
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    AbsenceAllView absence = new AbsenceAllView();
                    HttpResponseMessage response = _client
                        .GetAsync(_client.BaseAddress + "/absences/getabsencebyid/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        absence = JsonConvert.DeserializeObject<AbsenceAllView>(data);
                    }
                    return View(absence);
                }
                catch (Exception)
                {
                    return View();
                }
            }
            else { return BadRequest(); }
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditConfirmed(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    HttpResponseMessage response = _client.DeleteAsync
                        (_client.BaseAddress + $"/absences/approve/{id}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetAllAbsence");
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
                    AbsenceAllView absence = new AbsenceAllView();
                    HttpResponseMessage response = _client
                        .GetAsync(_client.BaseAddress + "/absences/getabsencebyid/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        absence = JsonConvert.DeserializeObject<AbsenceAllView>(data);
                    }
                    return View(absence);
                }
                catch (Exception)
                {
                    return View();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Hr"))
            {
                try
                {
                    HttpResponseMessage responsw = _client
                            .DeleteAsync(_client.BaseAddress + "/absences/delete/" + id).Result;

                    if (responsw.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetAllAbsences");
                    }
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();
                }
                return View();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
