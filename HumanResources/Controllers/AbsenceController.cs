using HumanResources.Models;
using HumanResources.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        private HttpService _http;

        public AbsenceController(HttpService http)
        {
            this._http = http;
        }

        [HttpGet]
        public IActionResult GetAllAbsences()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            string apiRoute = $"/absences/getabsences";

            if (User.IsInRole("Member"))
                {
                    var user = User.FindFirstValue(ClaimTypes.Email);
                    apiRoute+=$"/{user}";
            }
            var absenceList = _http.Get<List<AbsenceAllView>>(apiRoute);
            if (absenceList == null)
            {
                return BadRequest();
            }
            
            return View(absenceList);
        }

        [Authorize(Roles = "Member")]
        [HttpGet]
        public IActionResult Create()
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

        [Authorize(Roles = "Member")]
        [HttpPost]
        public IActionResult Create(AbsencePostModel model)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            var apiRoute = $"/absences/postabsence/{user}";

            var success = _http.Post<AbsencePostModel>(apiRoute, model);

            if (success)
            {
                return RedirectToAction("GetAllAbsences");
            }

             return View();  
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiRoute = $"/absences/getabsencebyid/{id}";
 
            var absence = _http.Get<AbsenceAllView>(apiRoute);
            if (absence != null)
            {
                return View(absence);
            }

            return View();
                     
        }

        [Authorize(Roles = "Hr")]
        [HttpPost, ActionName("Edit")]
        public IActionResult EditConfirmed(int id)
        {
            var apiRoute = $"/absences/approve/{id}";

            var success = _http.Delete(apiRoute);

            if (success)
            {
                return RedirectToAction("GetAllAbsences");
            }  

            return View();

        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            var apiRoute = $"/absences/getabsencebyid/{id}";

            var absence = _http.Get<AbsenceAllView>(apiRoute);
            if (absence != null)
            {
                return View(absence);
            }

            return View();
        }



        [Authorize(Roles = "Hr")]
        [HttpPost, ActionName("Cancel")]
        public IActionResult CancelConfirmed(int id)
        {
            var apiRoute = $"/absences/cancel/{id}";

            var success = _http.Delete(apiRoute);

            if (success)
            {
                return RedirectToAction("GetAllAbsences");
            }
            return View();
        }

        [Authorize(Roles = "Hr")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiRoute = $"/absences/getabsencebyid/{id}";

            var absence = _http.Get<AbsenceAllView>(apiRoute);

            if (absence != null) {

               return View(absence);
            }

            return View();

        }

        [Authorize(Roles = "Hr")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var apiRoute = $"/absences/delete/{id}";        

            var success = _http.Delete(apiRoute);

            if (success)
            {
                return RedirectToAction("GetAllAbsences");
            }
     
            return View();

        }

    }
}
